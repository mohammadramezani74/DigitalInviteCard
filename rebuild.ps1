# DigiCard - build and test only. No migration, no database change.
#
#   .\rebuild.ps1
#
# Use this after a code-only fix, to be sure the solution compiles and every test still passes
# before running the site again.

$ErrorActionPreference = 'Continue'

if (-not $PSScriptRoot) {
    Write-Host "Run this as a file (.\rebuild.ps1)." -ForegroundColor Yellow
    return
}
Set-Location -LiteralPath $PSScriptRoot

$log = Join-Path $PSScriptRoot 'rebuild-log.txt'
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

Write-Log ("DigiCard rebuild - " + (Get-Date -Format 'yyyy-MM-dd HH:mm:ss'))

$build = Invoke-Step "build solution" { dotnet build DigiCard.sln -c Release }
if ($build -ne 0) {
    Write-Log ""
    Write-Log "BUILD FAILED - stopping."
    return
}

Invoke-Step "run all tests" { dotnet test DigiCard.sln -c Release --no-build } | Out-Null

Write-Log ""
Write-Log "DONE. Log saved to: $log"
