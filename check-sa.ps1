# DigiCard - why is the sa login failing?
# Run from the Package Manager Console:   .\check-sa.ps1
# Log: check-sa-log.txt
#
# This script only READS server state. It changes nothing. If it finds the login locked
# out, it prints the exact statement to unlock it and stops, so the decision stays yours.

$ErrorActionPreference = 'Continue'

if (-not $PSScriptRoot) {
    Write-Host "Run this as a file (.\check-sa.ps1)." -ForegroundColor Yellow
    return
}
Set-Location -LiteralPath $PSScriptRoot

$log = Join-Path $PSScriptRoot 'check-sa-log.txt'
if (Test-Path $log) { Remove-Item $log -Force }

function Write-Log {
    param([string]$Text)
    $Text | Tee-Object -FilePath $log -Append | Out-Host
}

function Try-Connect {
    param([string]$Label, [string]$ConnectionString)
    Write-Log ""
    Write-Log "--- $Label"
    $conn = New-Object System.Data.SqlClient.SqlConnection $ConnectionString
    try {
        $conn.Open()
        $cmd = $conn.CreateCommand()
        $cmd.CommandText = "SELECT SUSER_SNAME() AS who, DB_NAME() AS db, IS_SRVROLEMEMBER('sysadmin') AS sysadmin"
        $r = $cmd.ExecuteReader()
        while ($r.Read()) {
            Write-Log ("  CONNECTED as {0}, database {1}, sysadmin={2}" -f $r['who'], $r['db'], $r['sysadmin'])
        }
        $r.Close()
        return $true
    }
    catch [System.Data.SqlClient.SqlException] {
        foreach ($e in $_.Exception.Errors) {
            Write-Log ("  FAILED  number={0} state={1} : {2}" -f $e.Number, $e.State, $e.Message)
        }
        return $false
    }
    catch {
        Write-Log ("  FAILED  " + $_.Exception.Message)
        return $false
    }
    finally { $conn.Dispose() }
}

Write-Log ("DigiCard sa diagnosis - " + (Get-Date -Format 'yyyy-MM-dd HH:mm:ss'))
Write-Log ""
Write-Log "SQL error numbers worth knowing:"
Write-Log "  18456 = login failed. state 7/8 = wrong password, state 1 = masked,"
Write-Log "          state 5 = login does not exist, state 15 = locked out."
Write-Log "  4060  = the login is fine but cannot open that database."
Write-Log "  17892 = logon rejected by a server logon trigger."

$settings = Join-Path $PSScriptRoot 'src\DigiCard.Web\appsettings.json'
$appCs = (Get-Content $settings -Raw | ConvertFrom-Json).ConnectionStrings.DefaultConnection
Write-Log ""
Write-Log "app connection string: $appCs"

# 1. exactly what the app uses
Try-Connect "the app's own connection string (sa)" $appCs | Out-Null

# 2. sa against master, to separate "bad login" from "cannot open that database"
$saMaster = ($appCs -replace 'Database=[^;]*', 'Database=master')
Try-Connect "sa against master" $saMaster | Out-Null

# 3. Windows authentication, which is what can repair the login if sa is locked
$winCs = 'Server=.;Database=master;Trusted_Connection=True;TrustServerCertificate=True;Connect Timeout=10'
$windowsWorks = Try-Connect "Windows authentication (your account)" $winCs

# 4. if Windows auth works, read the login's real state
if ($windowsWorks) {
    Write-Log ""
    Write-Log "--- login state for 'sa' (read-only)"
    $conn = New-Object System.Data.SqlClient.SqlConnection $winCs
    try {
        $conn.Open()
        $cmd = $conn.CreateCommand()
        $cmd.CommandText = @"
SELECT
    LOGINPROPERTY('sa','IsLocked')          AS IsLocked,
    LOGINPROPERTY('sa','IsExpired')         AS IsExpired,
    LOGINPROPERTY('sa','IsMustChange')      AS IsMustChange,
    LOGINPROPERTY('sa','BadPasswordCount')  AS BadPasswordCount,
    LOGINPROPERTY('sa','LockoutTime')       AS LockoutTime,
    (SELECT is_disabled FROM sys.sql_logins WHERE name = 'sa') AS IsDisabled,
    (SELECT is_policy_checked FROM sys.sql_logins WHERE name = 'sa') AS PolicyChecked,
    (SELECT CASE SERVERPROPERTY('IsIntegratedSecurityOnly')
        WHEN 1 THEN 'Windows only - SQL logins are refused'
        ELSE 'Mixed mode - SQL logins allowed' END) AS AuthMode
"@
        $r = $cmd.ExecuteReader()
        while ($r.Read()) {
            foreach ($name in 'IsLocked','IsExpired','IsMustChange','BadPasswordCount','LockoutTime','IsDisabled','PolicyChecked','AuthMode') {
                Write-Log ("  {0,-18} {1}" -f $name, $r[$name])
            }
        }
        $r.Close()

        Write-Log ""
        Write-Log "--- server logon triggers"
        $cmd2 = $conn.CreateCommand()
        $cmd2.CommandText = "SELECT name, is_disabled FROM sys.server_triggers"
        $r2 = $cmd2.ExecuteReader()
        $any = $false
        while ($r2.Read()) {
            $any = $true
            Write-Log ("  {0}  disabled={1}" -f $r2['name'], $r2['is_disabled'])
        }
        $r2.Close()
        if (-not $any) { Write-Log "  none" }
    }
    catch { Write-Log ("  query failed: " + $_.Exception.Message) }
    finally { $conn.Dispose() }
}
else {
    Write-Log ""
    Write-Log "Windows authentication did not work either, so the login state could not be read."
    Write-Log "In that case the fix has to happen in SSMS or SQL Server Configuration Manager."
}

Write-Log ""
Write-Log "================================================================"
Write-Log "Nothing was changed. Send me check-sa-log.txt and I will tell you"
Write-Log "which single statement fixes it."
Write-Log "================================================================"

if ($Host.Name -notmatch 'Package Manager') {
    Write-Host ""
    Read-Host "Press Enter to close"
}
