# DigiCard - Phase 2 / Step 1: occasions move from a hardcoded list into the database.
# Run from the Package Manager Console:   .\run-migration5.ps1
# Log: migration5-log.txt
#
# Creates the Occasions table in the templates schema, seeds the eleven occasions
# (six active wedding-side ones, five hidden until they have templates), applies it,
# then verifies and runs the tests.

$ErrorActionPreference = 'Continue'

if (-not $PSScriptRoot) {
    Write-Host "Run this as a file (.\run-migration5.ps1)." -ForegroundColor Yellow
    return
}
Set-Location -LiteralPath $PSScriptRoot

$log = Join-Path $PSScriptRoot 'migration5-log.txt'
if (Test-Path $log) { Remove-Item $log -Force }

function Write-Log {
    param([string]$Text)
    $Text | Tee-Object -FilePath $log -Append | Out-Host
}

function Invoke-Step {
    param([string]$Name, [scriptblock]$Action)
    Write-Log ""
    Write-Log "================================================================"
    Write-Log "STEP: $Name"
    Write-Log "================================================================"
    try {
        $output = & $Action 2>&1 | Out-String
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

Write-Log ("DigiCard Occasions migration - " + (Get-Date -Format 'yyyy-MM-dd HH:mm:ss'))

# ------------------------------------------------------------------ 0. build first
# A compile error here is far cheaper to read than the same error wrapped inside
# "dotnet ef", which reports it as a design-time assembly load failure.
$build = Invoke-Step "build solution" {
    dotnet build DigiCard.sln -c Release
}

if ($build -ne 0) {
    Write-Log ""
    Write-Log "BUILD FAILED - stopping. The database was not touched."
    if ($Host.Name -notmatch 'Package Manager') { Read-Host "Press Enter to close" }
    return
}

# ------------------------------------------------------------------ 1. create the migration
# --context is required: DbMigrator exposes three design-time factories.
$migration = Invoke-Step "create migration Occasions" {
    dotnet ef migrations add Occasions --context TemplatesDbContext --project ".\src\Modules\DigiCard.Modules.Templates" --startup-project ".\src\DigiCard.DbMigrator" --output-dir "Infrastructure\Migrations"
}

if ($migration -ne 0) {
    Write-Log ""
    Write-Log "MIGRATION NOT CREATED - stopping. The database was not touched."
    if ($Host.Name -notmatch 'Package Manager') { Read-Host "Press Enter to close" }
    return
}

# ------------------------------------------------------------------ 2. idempotent sql script
Invoke-Step "generate idempotent SQL script" {
    dotnet ef migrations script --context TemplatesDbContext --idempotent --project ".\src\Modules\DigiCard.Modules.Templates" --startup-project ".\src\DigiCard.DbMigrator" --output ".\docs\sql\templates.sql"
} | Out-Null

# ------------------------------------------------------------------ 3. apply all migrations
$applied = Invoke-Step "apply migrations (DbMigrator)" {
    dotnet run --project .\src\DigiCard.DbMigrator -c Release
}

if ($applied -ne 0) {
    Write-Log ""
    Write-Log "MIGRATOR FAILED - see the error above."
    if ($Host.Name -notmatch 'Package Manager') { Read-Host "Press Enter to close" }
    return
}

# ------------------------------------------------------------------ 4. verify against the db
# Reads the same connection string the app uses, so this cannot check a different database.
Invoke-Step "occasions now in the database" {
    $settings = Join-Path $PSScriptRoot 'src\DigiCard.Web\appsettings.json'
    $cs = (Get-Content $settings -Raw | ConvertFrom-Json).ConnectionStrings.DefaultConnection
    "connection: $cs"
    ""

    $conn = New-Object System.Data.SqlClient.SqlConnection $cs
    try {
        $conn.Open()
        $cmd = $conn.CreateCommand()
        $cmd.CommandText = "SELECT SortOrder, Slug, Title, IsActive, PrimaryLabel, SecondaryLabel FROM templates.Occasions ORDER BY SortOrder"
        $r = $cmd.ExecuteReader()
        $n = 0
        $active = 0
        while ($r.Read()) {
            $n++
            if ($r['IsActive']) { $active++ }
            "{0,3}  {1,-14} {2,-18} active={3,-6} {4} / {5}" -f `
                $r['SortOrder'], $r['Slug'], $r['Title'], $r['IsActive'], $r['PrimaryLabel'], $r['SecondaryLabel']
        }
        $r.Close()
        ""
        "total: $n occasions ($active active)"
        "expected: 11 occasions (6 active)"
    }
    finally { $conn.Dispose() }
} | Out-Null

# ------------------------------------------------------------------ 5. tests
Invoke-Step "run tests" { dotnet test DigiCard.sln -c Release } | Out-Null

Write-Log ""
Write-Log "DONE. Log saved to: $log"
Write-Log "Next: press F5 in Visual Studio, then open  /  and  /templates  ."

if ($Host.Name -notmatch 'Package Manager') {
    Write-Host ""
    Read-Host "Press Enter to close"
}
