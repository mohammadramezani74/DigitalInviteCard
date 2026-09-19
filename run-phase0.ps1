# DigiCard - Phase 0 baseline check
# Usage: right-click this file in File Explorer -> "Run with PowerShell"
# Everything it prints is also written to phase0-log.txt next to this script.

$ErrorActionPreference = 'Continue'

# $PSScriptRoot is empty when the contents are pasted rather than run as a file.
if (-not $PSScriptRoot) {
    Write-Host "Run this as a file (.\run-phase0.ps1), not by pasting its contents." -ForegroundColor Yellow
    return
}
Set-Location -LiteralPath $PSScriptRoot

$log = Join-Path $PSScriptRoot 'phase0-log.txt'
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

Write-Log "DigiCard Phase 0 baseline"
Write-Log ("date      : " + (Get-Date -Format 'yyyy-MM-dd HH:mm:ss'))
Write-Log ("folder    : " + $PSScriptRoot)
Write-Log ("machine   : " + $env:COMPUTERNAME + "  /  windows " + [System.Environment]::OSVersion.Version)
Write-Log ("powershell: " + $PSVersionTable.PSVersion)

# ---------------------------------------------------------------- environment
Invoke-Step "dotnet --info" { dotnet --info } | Out-Null
Invoke-Step "installed SDKs" { dotnet --list-sdks } | Out-Null

# ---------------------------------------------------------------- restore
$restore = Invoke-Step "dotnet restore" { dotnet restore DigiCard.sln }

# ---------------------------------------------------------------- build
$build = 1
if ($restore -eq 0) {
    $build = Invoke-Step "dotnet build (Release)" { dotnet build DigiCard.sln -c Release --no-restore }
} else {
    Write-Log ""
    Write-Log "SKIPPED build: restore failed."
}

# ---------------------------------------------------------------- publish
if ($build -eq 0) {
    Invoke-Step "dotnet publish DigiCard.Web (Release)" {
        dotnet publish .\src\DigiCard.Web\DigiCard.Web.csproj -c Release -o .\artifacts\publish
    } | Out-Null
} else {
    Write-Log ""
    Write-Log "SKIPPED publish: build did not succeed."
}

# ---------------------------------------------------------------- tests
if ($build -eq 0) {
    Invoke-Step "dotnet test (no SQL Server)" {
        dotnet test DigiCard.sln -c Release --no-build
    } | Out-Null
} else {
    Write-Log ""
    Write-Log "SKIPPED tests: build did not succeed."
}

# ---------------------------------------------------------------- sql server discovery
Invoke-Step "local SQL Server services" {
    Get-Service -Name 'MSSQL*' -ErrorAction SilentlyContinue |
        Select-Object Name, DisplayName, Status | Format-Table -AutoSize | Out-String
} | Out-Null

Write-Log ""
Write-Log "================================================================"
Write-Log "DONE. Full log saved to: $log"
Write-Log "================================================================"
Write-Log ""
Write-Log "The SQL Server test and the DbMigrator were NOT run by this script,"
Write-Log "because they need your real connection string. We do those next."

# In the Visual Studio Package Manager Console there is no window to keep open,
# and Read-Host would just block the console. Only pause for a real console window.
if ($Host.Name -notmatch 'Package Manager') {
    Write-Host ""
    Read-Host "Press Enter to close"
}
