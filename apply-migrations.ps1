# DigiCard - build, apply whatever migrations exist, verify, test.
#
#   .\apply-migrations.ps1
#
# This one creates nothing. Use it whenever the migration files are already in the project and
# only need applying - which is the normal case after pulling changes, and the case you land in
# when a run-migration script got as far as scaffolding and then stopped.

$ErrorActionPreference = 'Continue'

if (-not $PSScriptRoot) {
    Write-Host "Run this as a file (.\apply-migrations.ps1)." -ForegroundColor Yellow
    return
}
Set-Location -LiteralPath $PSScriptRoot

$log = Join-Path $PSScriptRoot 'apply-log.txt'
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

Write-Log ("DigiCard apply - " + (Get-Date -Format 'yyyy-MM-dd HH:mm:ss'))

$build = Invoke-Step "build solution" { dotnet build DigiCard.sln -c Release }
if ($build -ne 0) {
    Write-Log ""
    Write-Log "BUILD FAILED - stopping. The database was not touched."
    return
}

$applied = Invoke-Step "apply migrations (DbMigrator)" { dotnet run --project .\src\DigiCard.DbMigrator -c Release }
if ($applied -ne 0) {
    Write-Log ""
    Write-Log "MIGRATOR FAILED - see the error above."
    return
}

Invoke-Step "what the database has now" {
    $settings = Join-Path $PSScriptRoot 'src\DigiCard.Web\appsettings.json'
    $cs = (Get-Content $settings -Raw | ConvertFrom-Json).ConnectionStrings.DefaultConnection

    $conn = New-Object System.Data.SqlClient.SqlConnection $cs
    try {
        $conn.Open()

        $cmd = $conn.CreateCommand()
        $cmd.CommandText = @"
SELECT name, TYPE_NAME(user_type_id) AS Type FROM sys.columns
WHERE object_id = OBJECT_ID('invitations.Drafts') AND name IN ('Elements','EventDate','EventTime','EventEndTime')
ORDER BY name
"@
        $r = $cmd.ExecuteReader()
        $n = 0
        while ($r.Read()) { $n++; "invitations.Drafts.{0,-13} {1}" -f $r['name'], $r['Type'] }
        $r.Close()
        ""
        "draft columns: $n   (expected 4)"
        ""

        $cmd2 = $conn.CreateCommand()
        $cmd2.CommandText = "SELECT COUNT(*) FROM templates.Templates"
        $cmd3 = $conn.CreateCommand()
        $cmd3.CommandText = "SELECT COUNT(*) FROM templates.Occasions"
        $cmd4 = $conn.CreateCommand()
        $cmd4.CommandText = "SELECT COUNT(*) FROM templates.Poems"

        "templates: {0}   occasions: {1}   poems: {2}" -f `
            $cmd2.ExecuteScalar(), $cmd3.ExecuteScalar(), $cmd4.ExecuteScalar()
        "expected:  11              11               7"
    }
    finally { $conn.Dispose() }
} | Out-Null

Invoke-Step "run all tests" { dotnet test DigiCard.sln -c Release } | Out-Null

Write-Log ""
Write-Log "DONE. Log saved to: $log"
