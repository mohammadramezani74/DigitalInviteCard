# DigiCard - Phase 0, part 2: the database steps.
# Run from the Package Manager Console:   .\run-phase0-sql.ps1
# Output is written to phase0-sql-log.txt next to this script.
#
# Your machine runs the DEFAULT SQL Server instance (MSSQLSERVER), so the server name
# is "." and not ".\SQLEXPRESS". Windows authentication is used.

$ErrorActionPreference = 'Continue'

if (-not $PSScriptRoot) {
    Write-Host "Run this as a file (.\run-phase0-sql.ps1), not by pasting its contents." -ForegroundColor Yellow
    return
}
Set-Location -LiteralPath $PSScriptRoot

$log = Join-Path $PSScriptRoot 'phase0-sql-log.txt'
if (Test-Path $log) { Remove-Item $log -Force }

$appDb  = 'Server=.;Database=DigiCard;Trusted_Connection=True;TrustServerCertificate=True'
$testDb = 'Server=.;Database=DigiCard_Test;Trusted_Connection=True;TrustServerCertificate=True'

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
    $sw = [System.Diagnostics.Stopwatch]::StartNew()
    try {
        $output = & $Action 2>&1 | Out-String
        $code = $LASTEXITCODE
        Write-Log $output.TrimEnd()
        $sw.Stop()
        if ($null -eq $code) { $code = 0 }
        Write-Log ("-> exit code: {0}   ({1:n1}s)" -f $code, $sw.Elapsed.TotalSeconds)
        return $code
    }
    catch {
        $sw.Stop()
        Write-Log ("EXCEPTION: " + $_.Exception.Message)
        Write-Log ("-> failed   ({0:n1}s)" -f $sw.Elapsed.TotalSeconds)
        return 1
    }
}

Write-Log "DigiCard Phase 0 - database steps"
Write-Log ("date   : " + (Get-Date -Format 'yyyy-MM-dd HH:mm:ss'))
Write-Log ("app db : " + $appDb)
Write-Log ("test db: " + $testDb)

# ------------------------------------------------------------------ connectivity
# A plain connection attempt, so a login problem is reported on its own rather than
# surfacing later as a confusing EF or test failure.
Invoke-Step "connect to SQL Server (master)" {
    $csb = 'Server=.;Database=master;Trusted_Connection=True;TrustServerCertificate=True;Connect Timeout=10'
    $conn = New-Object System.Data.SqlClient.SqlConnection $csb
    try {
        $conn.Open()
        $cmd = $conn.CreateCommand()
        $cmd.CommandText = "SELECT @@VERSION AS v, SUSER_SNAME() AS login_name, @@SERVERNAME AS server_name"
        $r = $cmd.ExecuteReader()
        while ($r.Read()) {
            "server : " + $r['server_name']
            "login  : " + $r['login_name']
            "version: " + ($r['v'] -split "`n")[0]
        }
        $r.Close()
    }
    finally { $conn.Dispose() }
} | Out-Null

# ------------------------------------------------------------------ logon triggers
# The handoff records a "Logon failed ... due to trigger execution" error on this machine.
# List server-level triggers so the cause is visible instead of guessed. Read-only.
Invoke-Step "server logon triggers (read-only check)" {
    $csb = 'Server=.;Database=master;Trusted_Connection=True;TrustServerCertificate=True;Connect Timeout=10'
    $conn = New-Object System.Data.SqlClient.SqlConnection $csb
    try {
        $conn.Open()
        $cmd = $conn.CreateCommand()
        $cmd.CommandText = "SELECT name, is_disabled, create_date FROM sys.server_triggers"
        $r = $cmd.ExecuteReader()
        $found = $false
        while ($r.Read()) {
            $found = $true
            "trigger: {0}   disabled={1}   created={2}" -f $r['name'], $r['is_disabled'], $r['create_date']
        }
        $r.Close()
        if (-not $found) { "No server-level triggers found. The earlier logon error is not caused by one now." }
    }
    finally { $conn.Dispose() }
} | Out-Null

# ------------------------------------------------------------------ migrator
$env:ConnectionStrings__DefaultConnection = $appDb
$migrator = Invoke-Step "DbMigrator against DigiCard" {
    dotnet run --project .\src\DigiCard.DbMigrator -c Release
}

# ------------------------------------------------------------------ sql-backed tests
$env:DIGICARD_TEST_SQL = $testDb
Invoke-Step "dotnet test with SQL Server enabled" {
    dotnet test DigiCard.sln -c Release
} | Out-Null

# ------------------------------------------------------------------ what landed
Invoke-Step "tables created in DigiCard" {
    $conn = New-Object System.Data.SqlClient.SqlConnection $appDb
    try {
        $conn.Open()
        $cmd = $conn.CreateCommand()
        $cmd.CommandText = @"
SELECT TABLE_SCHEMA + '.' + TABLE_NAME AS t
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_SCHEMA, TABLE_NAME
"@
        $r = $cmd.ExecuteReader()
        while ($r.Read()) { $r['t'] }
        $r.Close()

        $cmd2 = $conn.CreateCommand()
        $cmd2.CommandText = "SELECT COUNT(*) FROM templates.Templates"
        "template rows: " + $cmd2.ExecuteScalar()
    }
    finally { $conn.Dispose() }
} | Out-Null

Remove-Item Env:\ConnectionStrings__DefaultConnection -ErrorAction SilentlyContinue
Remove-Item Env:\DIGICARD_TEST_SQL -ErrorAction SilentlyContinue

Write-Log ""
Write-Log "================================================================"
Write-Log "DONE. Log saved to: $log"
Write-Log "================================================================"

if ($Host.Name -notmatch 'Package Manager') {
    Write-Host ""
    Read-Host "Press Enter to close"
}
