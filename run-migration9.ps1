# DigiCard - the whole card now survives Save: address column for drafts.
#
#   .\run-migration9.ps1
#
# Creating a migration whose name already exists is not treated as a failure here. The previous
# script stopped dead on that, which was wrong: a name that already exists means the work was
# already done, and the right thing is to carry on to applying it.

$ErrorActionPreference = 'Continue'

if (-not $PSScriptRoot) {
    Write-Host "Run this as a file (.\run-migration9.ps1)." -ForegroundColor Yellow
    return
}
Set-Location -LiteralPath $PSScriptRoot

$log = Join-Path $PSScriptRoot 'migration9-log.txt'
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

Write-Log ("DigiCard draft address - " + (Get-Date -Format 'yyyy-MM-dd HH:mm:ss'))

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

$created = Invoke-Step "create migration DraftAddress (invitations)" {
    dotnet ef migrations add DraftAddress --context InvitationsDbContext --project ".\src\Modules\DigiCard.Modules.Invitations" --startup-project ".\src\DigiCard.DbMigrator" --output-dir "Infrastructure\Migrations"
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

Invoke-Step "what a draft can hold now" {
    $settings = Join-Path $PSScriptRoot 'src\DigiCard.Web\appsettings.json'
    $cs = (Get-Content $settings -Raw | ConvertFrom-Json).ConnectionStrings.DefaultConnection

    $conn = New-Object System.Data.SqlClient.SqlConnection $cs
    try {
        $conn.Open()
        $cmd = $conn.CreateCommand()
        $cmd.CommandText = @"
SELECT name, TYPE_NAME(user_type_id) AS Type FROM sys.columns
WHERE object_id = OBJECT_ID('invitations.Drafts')
  AND name IN ('Elements','Address','EventDate','EventTime','EventEndTime')
ORDER BY name
"@
        $r = $cmd.ExecuteReader()
        $n = 0
        while ($r.Read()) { $n++; "invitations.Drafts.{0,-13} {1}" -f $r['name'], $r['Type'] }
        $r.Close()
        ""
        "columns: $n   (expected 5)"
    }
    finally { $conn.Dispose() }
} | Out-Null

Invoke-Step "run all tests" { dotnet test DigiCard.sln -c Release } | Out-Null

Write-Log ""
Write-Log "DONE. Log saved to: $log"
Write-Log "Next: F5, Ctrl+F5 on /studio, arrange a card, sign in, and press ذخیره کارت."
