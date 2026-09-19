# DigiCard - Phase 1: build the new code, create the template migration, apply it, test.
# Run from the Package Manager Console:   .\run-phase1.ps1
# Output is written to phase1-log.txt next to this script.
#
# The connection string is NOT set here: DbMigrator already falls back to the same
# server/database your app is configured for. Nothing is dropped and nothing is recreated;
# the migration only alters the templates schema and updates its seed rows.

$ErrorActionPreference = 'Continue'

if (-not $PSScriptRoot) {
    Write-Host "Run this as a file (.\run-phase1.ps1), not by pasting its contents." -ForegroundColor Yellow
    return
}
Set-Location -LiteralPath $PSScriptRoot

$log = Join-Path $PSScriptRoot 'phase1-log.txt'
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

Write-Log "DigiCard Phase 1 - template design tokens, gallery, SEO"
Write-Log ("date  : " + (Get-Date -Format 'yyyy-MM-dd HH:mm:ss'))

# ------------------------------------------------------------------ 1. compile the new code
# This is the gate. Everything after it is pointless if the code does not build.
$build = Invoke-Step "build the new code (Release)" {
    dotnet build DigiCard.sln -c Release
}

if ($build -ne 0) {
    Write-Log ""
    Write-Log "################################################################"
    Write-Log "BUILD FAILED - stopping here on purpose."
    Write-Log "No migration was created and the database was not touched."
    Write-Log "Send the errors above; they are also in phase1-log.txt."
    Write-Log "################################################################"
    if ($Host.Name -notmatch 'Package Manager') { Read-Host "Press Enter to close" }
    return
}

# ------------------------------------------------------------------ 2. ef tooling
Invoke-Step "restore dotnet tools (dotnet-ef)" { dotnet tool restore } | Out-Null

# ------------------------------------------------------------------ 3. the migration
# Templates keeps its migrations under Infrastructure\Migrations, and the design-time
# factories live in DbMigrator - hence the -o and -s arguments.
$migration = Invoke-Step "create migration TemplateDesignTokens" {
    dotnet ef migrations add TemplateDesignTokens `
        -p .\src\Modules\DigiCard.Modules.Templates `
        -s .\src\DigiCard.DbMigrator `
        -o Infrastructure\Migrations
}

if ($migration -ne 0) {
    Write-Log ""
    Write-Log "################################################################"
    Write-Log "MIGRATION NOT CREATED - stopping. The database was not touched."
    Write-Log "################################################################"
    if ($Host.Name -notmatch 'Package Manager') { Read-Host "Press Enter to close" }
    return
}

# ------------------------------------------------------------------ 4. idempotent sql script
# Kept beside the existing ones so the change can also be applied by hand in SSMS.
Invoke-Step "generate idempotent SQL script" {
    dotnet ef migrations script `
        -p .\src\Modules\DigiCard.Modules.Templates `
        -s .\src\DigiCard.DbMigrator `
        --idempotent `
        -o .\docs\sql\templates.sql
} | Out-Null

# ------------------------------------------------------------------ 5. apply
Invoke-Step "apply migrations (DbMigrator)" {
    dotnet run --project .\src\DigiCard.DbMigrator -c Release
} | Out-Null

# ------------------------------------------------------------------ 6. tests
Invoke-Step "run tests" { dotnet test DigiCard.sln -c Release --no-build } | Out-Null

# ------------------------------------------------------------------ 7. what is in the catalog
Invoke-Step "templates now in the catalog" {
    $cs = 'Server=.;Database=DigitalCard;User Id=sa;Password=39143914;TrustServerCertificate=True;Encrypt=false;Connect Timeout=10'
    $conn = New-Object System.Data.SqlClient.SqlConnection $cs
    try {
        $conn.Open()
        $cmd = $conn.CreateCommand()
        $cmd.CommandText = "SELECT Slug, Name, Category, Family, Layout FROM templates.Templates ORDER BY Family, Name"
        $r = $cmd.ExecuteReader()
        $n = 0
        while ($r.Read()) {
            $n++
            "{0,-18} {1,-16} {2,-12} {3,-12} {4}" -f $r['Slug'], $r['Name'], $r['Category'], $r['Family'], $r['Layout']
        }
        $r.Close()
        ""
        "total: $n templates"
    }
    finally { $conn.Dispose() }
} | Out-Null

Write-Log ""
Write-Log "================================================================"
Write-Log "DONE. Log saved to: $log"
Write-Log "================================================================"
Write-Log ""
Write-Log "Next: press F5 in Visual Studio and look at  /  and  /templates  ."

if ($Host.Name -notmatch 'Package Manager') {
    Write-Host ""
    Read-Host "Press Enter to close"
}
