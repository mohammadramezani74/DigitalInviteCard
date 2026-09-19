# DigiCard - Phase 2 / Step 2: a card becomes a list of positioned elements.
# Run from the Package Manager Console:   .\run-migration6.ps1
# Log: migration6-log.txt
#
# Two migrations, because the layout lives in two places: the template's saved arrangement
# (templates schema) and the copy a draft keeps for itself (invitations schema).

$ErrorActionPreference = 'Continue'

if (-not $PSScriptRoot) {
    Write-Host "Run this as a file (.\run-migration6.ps1)." -ForegroundColor Yellow
    return
}
Set-Location -LiteralPath $PSScriptRoot

$log = Join-Path $PSScriptRoot 'migration6-log.txt'
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
$unit = Invoke-Step "card layout tests" {
    dotnet test DigiCard.sln -c Release --no-build --filter "FullyQualifiedName~CardLayoutTests"
}
if ($unit -ne 0) { Stop-Here "LAYOUT TESTS FAILED - stopping before touching the database."; return }

# ------------------------------------------------------------------ 2. templates migration
$m1 = Invoke-Step "create migration CardElements (templates)" {
    dotnet ef migrations add CardElements --context TemplatesDbContext --project ".\src\Modules\DigiCard.Modules.Templates" --startup-project ".\src\DigiCard.DbMigrator" --output-dir "Infrastructure\Migrations"
}
if ($m1 -ne 0) { Stop-Here "TEMPLATES MIGRATION NOT CREATED - stopping. The database was not touched."; return }

# ------------------------------------------------------------------ 3. invitations migration
$m2 = Invoke-Step "create migration DraftElements (invitations)" {
    dotnet ef migrations add DraftElements --context InvitationsDbContext --project ".\src\Modules\DigiCard.Modules.Invitations" --startup-project ".\src\DigiCard.DbMigrator" --output-dir "Infrastructure\Migrations"
}
if ($m2 -ne 0) { Stop-Here "INVITATIONS MIGRATION NOT CREATED - stopping. The database was not touched."; return }

# ------------------------------------------------------------------ 4. idempotent sql scripts
Invoke-Step "generate idempotent SQL scripts" {
    dotnet ef migrations script --context TemplatesDbContext --idempotent --project ".\src\Modules\DigiCard.Modules.Templates" --startup-project ".\src\DigiCard.DbMigrator" --output ".\docs\sql\templates.sql"
    dotnet ef migrations script --context InvitationsDbContext --idempotent --project ".\src\Modules\DigiCard.Modules.Invitations" --startup-project ".\src\DigiCard.DbMigrator" --output ".\docs\sql\invitations.sql"
} | Out-Null

# ------------------------------------------------------------------ 5. apply
$applied = Invoke-Step "apply migrations (DbMigrator)" { dotnet run --project .\src\DigiCard.DbMigrator -c Release }
if ($applied -ne 0) { Stop-Here "MIGRATOR FAILED - see the error above."; return }

# ------------------------------------------------------------------ 6. verify against the db
Invoke-Step "layouts now in the database" {
    $settings = Join-Path $PSScriptRoot 'src\DigiCard.Web\appsettings.json'
    $cs = (Get-Content $settings -Raw | ConvertFrom-Json).ConnectionStrings.DefaultConnection
    "connection: $cs"
    ""

    $conn = New-Object System.Data.SqlClient.SqlConnection $cs
    try {
        $conn.Open()
        $cmd = $conn.CreateCommand()
        $cmd.CommandText = @"
SELECT Slug, SafeTop, SafeBottom, LEN(Elements) AS Len,
       CASE WHEN Elements LIKE '%"role":"names"%' THEN 1 ELSE 0 END AS HasNames
FROM templates.Templates ORDER BY Slug
"@
        $r = $cmd.ExecuteReader()
        $n = 0; $withLayout = 0
        while ($r.Read()) {
            $n++
            if ($r['Len'] -gt 0 -and $r['HasNames'] -eq 1) { $withLayout++ }
            "{0,-18} safe {1,3}-{2,-3} json {3,5} chars  names={4}" -f `
                $r['Slug'], $r['SafeTop'], $r['SafeBottom'], $r['Len'], $r['HasNames']
        }
        $r.Close()
        ""
        "templates: $n   with a usable layout: $withLayout"
        "expected: 11 / 11"
        ""

        # The drafts column only has to exist; nothing writes it until the editor does.
        $cmd2 = $conn.CreateCommand()
        $cmd2.CommandText = "SELECT COUNT(*) FROM sys.columns WHERE object_id = OBJECT_ID('invitations.Drafts') AND name = 'Elements'"
        "invitations.Drafts.Elements present: " + $cmd2.ExecuteScalar()
    }
    finally { $conn.Dispose() }
} | Out-Null

# ------------------------------------------------------------------ 7. full test run
Invoke-Step "run all tests" { dotnet test DigiCard.sln -c Release } | Out-Null

Write-Log ""
Write-Log "DONE. Log saved to: $log"
Write-Log "Next: press F5, then open  /  and  /templates  - the cards should look unchanged."

if ($Host.Name -notmatch 'Package Manager') {
    Write-Host ""
    Read-Host "Press Enter to close"
}
