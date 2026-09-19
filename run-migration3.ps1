# DigiCard - create and apply the TemplateArtworkAll migration.
# Run from the Package Manager Console:   .\run-migration.ps1
# Log: migration3-log.txt
#
# The previous script missed --context. DbMigrator exposes three design-time factories
# (accounts, templates, invitations), so EF refuses to guess which model to scaffold.

$ErrorActionPreference = 'Continue'

if (-not $PSScriptRoot) {
    Write-Host "Run this as a file (.\run-migration.ps1)." -ForegroundColor Yellow
    return
}
Set-Location -LiteralPath $PSScriptRoot

$log = Join-Path $PSScriptRoot 'migration3-log.txt'
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

Write-Log ("DigiCard migration - " + (Get-Date -Format 'yyyy-MM-dd HH:mm:ss'))

# ------------------------------------------------------------------ 1. create the migration
$migration = Invoke-Step "create migration TemplateArtworkAll" {
    dotnet ef migrations add TemplateArtworkAll --context TemplatesDbContext --project ".\src\Modules\DigiCard.Modules.Templates" --startup-project ".\src\DigiCard.DbMigrator" --output-dir "Infrastructure\Migrations"
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
Invoke-Step "templates now in the database" {
    $settings = Join-Path $PSScriptRoot 'src\DigiCard.Web\appsettings.json'
    $cs = (Get-Content $settings -Raw | ConvertFrom-Json).ConnectionStrings.DefaultConnection
    "connection: $cs"
    ""

    $conn = New-Object System.Data.SqlClient.SqlConnection $cs
    try {
        $conn.Open()
        $cmd = $conn.CreateCommand()
        $cmd.CommandText = "SELECT Slug, Name, Artwork, SafeTop, SafeBottom FROM templates.Templates ORDER BY Family, Name"
        $r = $cmd.ExecuteReader()
        $n = 0
        while ($r.Read()) {
            $n++
            "{0,-18} {1,-16} artwork='{2}' safe={3}%-{4}%" -f $r['Slug'], $r['Name'], $r['Artwork'], $r['SafeTop'], $r['SafeBottom']
        }
        $r.Close()
        ""
        "total: $n templates"
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
