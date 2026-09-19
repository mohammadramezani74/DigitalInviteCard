# DigiCard - photo upload: the image library, the media table, and the store on disk.
#
#   .\run-migration11.ps1
#
# The package version is not written by hand anywhere: dotnet picks a version compatible with
# net10.0 and records it in Directory.Packages.props, because this project uses central package
# management.
#
# A leftover SixLabors.ImageSharp line may remain in Directory.Packages.props after the remove
# below. An unused PackageVersion entry does nothing; delete it when you next tidy that file.

$ErrorActionPreference = 'Continue'

if (-not $PSScriptRoot) {
    Write-Host "Run this as a file (.\run-migration11.ps1)." -ForegroundColor Yellow
    return
}
Set-Location -LiteralPath $PSScriptRoot

$log = Join-Path $PSScriptRoot 'migration11-log.txt'
if (Test-Path $log) { Remove-Item $log -Force }

function Write-Log {
    param([string]$Text)
    $Text | Tee-Object -FilePath $log -Append | Out-Host
}

$script:LastOutput = ''

function Invoke-Step {
    param([string]$Name, [scriptblock]$Action)
    Write-Log ""
    Write-Log "================================================================"
    Write-Log "STEP: $Name"
    Write-Log "================================================================"
    try {
        $output = & $Action 2>&1 | Out-String
        $script:LastOutput = $output
        $code = $LASTEXITCODE
        Write-Log $output.TrimEnd()
        if ($null -eq $code) { $code = 0 }
        Write-Log ("-> exit code: {0}" -f $code)
        return $code
    }
    catch {
        Write-Log ("EXCEPTION: " + $_.Exception.Message)
        return 1
    }
}

Write-Log ("DigiCard media (SkiaSharp) - " + (Get-Date -Format 'yyyy-MM-dd HH:mm:ss'))

# ------------------------------------------------------------------ 1. the imaging library
# ImageSharp 4 refuses to compile without a purchased licence key, so it is removed again here.
# SkiaSharp is MIT over Google's Skia - the same engine Chrome draws with - and needs no key.
# The Linux native assets are a no-op on Windows and are what make a Linux server work later.
Invoke-Step "remove SixLabors.ImageSharp" {
    dotnet remove ".\src\Modules\DigiCard.Modules.Invitations\DigiCard.Modules.Invitations.csproj" package SixLabors.ImageSharp
} | Out-Null

$package = Invoke-Step "add SkiaSharp" {
    dotnet add ".\src\Modules\DigiCard.Modules.Invitations\DigiCard.Modules.Invitations.csproj" package SkiaSharp
    dotnet add ".\src\Modules\DigiCard.Modules.Invitations\DigiCard.Modules.Invitations.csproj" package SkiaSharp.NativeAssets.Linux
}

if ($package -ne 0) {
    Write-Log ""
    Write-Log "PACKAGE NOT ADDED - check the network or the NuGet source. Nothing else was changed."
    return
}

# ------------------------------------------------------------------ 2. build
$build = Invoke-Step "build solution" { dotnet build DigiCard.sln -c Release }
if ($build -ne 0) {
    Write-Log ""
    Write-Log "BUILD FAILED - stopping. The database was not touched."
    return
}

$unit = Invoke-Step "layout, calendar and draft tests" {
    dotnet test DigiCard.sln -c Release --no-build --filter "FullyQualifiedName~CardLayoutTests|FullyQualifiedName~PersianDateTests|FullyQualifiedName~DraftTests"
}
if ($unit -ne 0) {
    Write-Log ""
    Write-Log "TESTS FAILED - stopping before touching the database."
    return
}

# ------------------------------------------------------------------ 3. migration
$created = Invoke-Step "create migration MediaAssets (invitations)" {
    dotnet ef migrations add MediaAssets --context InvitationsDbContext --project ".\src\Modules\DigiCard.Modules.Invitations" --startup-project ".\src\DigiCard.DbMigrator" --output-dir "Infrastructure\Migrations"
}

if ($created -ne 0) {
    if ($script:LastOutput -match 'used by an existing migration') {
        Write-Log "-> already scaffolded on an earlier run; carrying on to apply it."
    }
    else {
        Write-Log ""
        Write-Log "MIGRATION NOT CREATED - stopping. The database was not touched."
        return
    }
}

$applied = Invoke-Step "apply migrations (DbMigrator)" { dotnet run --project .\src\DigiCard.DbMigrator -c Release }
if ($applied -ne 0) {
    Write-Log ""
    Write-Log "MIGRATOR FAILED - see the error above."
    return
}

# ------------------------------------------------------------------ 4. verify
Invoke-Step "the media table" {
    $settings = Join-Path $PSScriptRoot 'src\DigiCard.Web\appsettings.json'
    $cs = (Get-Content $settings -Raw | ConvertFrom-Json).ConnectionStrings.DefaultConnection

    $conn = New-Object System.Data.SqlClient.SqlConnection $cs
    try {
        $conn.Open()
        $cmd = $conn.CreateCommand()
        $cmd.CommandText = @"
SELECT name, TYPE_NAME(user_type_id) AS Type FROM sys.columns
WHERE object_id = OBJECT_ID('invitations.Media') ORDER BY column_id
"@
        $r = $cmd.ExecuteReader()
        $n = 0
        while ($r.Read()) { $n++; "invitations.Media.{0,-10} {1}" -f $r['name'], $r['Type'] }
        $r.Close()
        ""
        "columns: $n   (expected 7)"
    }
    finally { $conn.Dispose() }
} | Out-Null

Invoke-Step "run all tests" { dotnet test DigiCard.sln -c Release } | Out-Null

Write-Log ""
Write-Log "DONE. Log saved to: $log"
Write-Log ""
Write-Log "Uploaded images are written to src\DigiCard.Web\media-store\ - outside wwwroot, so"
Write-Log "nothing serves them without checking who is asking. Set Media:Root in appsettings to"
Write-Log "put them somewhere else on a real server."
