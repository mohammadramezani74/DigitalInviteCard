# DigiCard - editor fixes: an optional end time for the ceremony.
# Run from the Package Manager Console:   .\run-migration8.ps1
# Log: migration8-log.txt
#
# Two migrations again: the templates schema gains the Poems table and re-seeds every layout
# (the element style grew variant, frame and feather fields), and the invitations schema gains
# the real event date and time.

$ErrorActionPreference = 'Continue'

if (-not $PSScriptRoot) {
    Write-Host "Run this as a file (.\run-migration8.ps1)." -ForegroundColor Yellow
    return
}
Set-Location -LiteralPath $PSScriptRoot

$log = Join-Path $PSScriptRoot 'migration8-log.txt'
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

function Stop-Here {
    param([string]$Why)
    Write-Log ""
    Write-Log $Why
    if ($Host.Name -notmatch 'Package Manager') { Read-Host "Press Enter to close" }
}

Write-Log ("DigiCard card elements - " + (Get-Date -Format 'yyyy-MM-dd HH:mm:ss'))

# ------------------------------------------------------------------ 0. build first
$build = Invoke-Step "build solution" { dotnet build DigiCard.sln -c Release }
if ($build -ne 0) { Stop-Here "BUILD FAILED - stopping. The database was not touched."; return }

# ------------------------------------------------------------------ 1. run the new unit tests early
# CardLayoutTests needs no database. If normalization or the invariant-culture formatting is
# wrong, that is far easier to read here than after two schema changes.
$unit = Invoke-Step "layout and calendar tests" {
    dotnet test DigiCard.sln -c Release --no-build --filter "FullyQualifiedName~CardLayoutTests|FullyQualifiedName~PersianDateTests"
}
if ($unit -ne 0) { Stop-Here "LAYOUT TESTS FAILED - stopping before touching the database."; return }

# ------------------------------------------------------------------ 2. invitations migration
$m2 = Invoke-Step "create migration EventEndTime (invitations)" {
    dotnet ef migrations add EventEndTime --context InvitationsDbContext --project ".\src\Modules\DigiCard.Modules.Invitations" --startup-project ".\src\DigiCard.DbMigrator" --output-dir "Infrastructure\Migrations"
}
if ($m2 -ne 0) { Stop-Here "INVITATIONS MIGRATION NOT CREATED - stopping. The database was not touched."; return }

# ------------------------------------------------------------------ 4. idempotent sql scripts
Invoke-Step "generate idempotent SQL scripts" {
    dotnet ef migrations script --context InvitationsDbContext --idempotent --project ".\src\Modules\DigiCard.Modules.Invitations" --startup-project ".\src\DigiCard.DbMigrator" --output ".\docs\sql\invitations.sql"
} | Out-Null

# ------------------------------------------------------------------ 5. apply
$applied = Invoke-Step "apply migrations (DbMigrator)" { dotnet run --project .\src\DigiCard.DbMigrator -c Release }
if ($applied -ne 0) { Stop-Here "MIGRATOR FAILED - see the error above."; return }

# ------------------------------------------------------------------ 6. verify against the db
Invoke-Step "drafts now carry a start and an end" {
    $settings = Join-Path $PSScriptRoot 'src\DigiCard.Web\appsettings.json'
    $cs = (Get-Content $settings -Raw | ConvertFrom-Json).ConnectionStrings.DefaultConnection

    $conn = New-Object System.Data.SqlClient.SqlConnection $cs
    try {
        $conn.Open()
        $cmd = $conn.CreateCommand()
        $cmd.CommandText = @"
SELECT name, TYPE_NAME(user_type_id) AS Type FROM sys.columns
WHERE object_id = OBJECT_ID('invitations.Drafts') AND name IN ('EventDate','EventTime','EventEndTime')
ORDER BY name
"@
        $r = $cmd.ExecuteReader()
        $n = 0
        while ($r.Read()) { $n++; "invitations.Drafts.{0} : {1}" -f $r['name'], $r['Type'] }
        $r.Close()
        ""
        "columns: $n   (expected 3)"
    }
    finally { $conn.Dispose() }
} | Out-Null

# ------------------------------------------------------------------ 7. full test run
Invoke-Step "run all tests" { dotnet test DigiCard.sln -c Release } | Out-Null

Write-Log ""
Write-Log "DONE. Log saved to: $log"
Write-Log "Next: press F5, then Ctrl+F5 on  /  and  /templates  - the cards should look unchanged."

if ($Host.Name -notmatch 'Package Manager') {
    Write-Host ""
    Read-Host "Press Enter to close"
}
