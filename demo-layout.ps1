# DigiCard - proof that a card's arrangement is now data, not code.
#
#   .\demo-layout.ps1           move things around on one template
#   .\demo-layout.ps1 -Restore  put it back exactly as it was
#
# Nothing is rebuilt and no code is touched: this writes a different Elements JSON for the
# "golab-abrang" template and the site renders it on the next refresh. That is the whole point
# of this step - the editor will write exactly this column, nothing more.
#
# The original value is copied into templates.LayoutBackup first, so -Restore puts back the real
# previous JSON rather than a copy kept in this script that could drift from the migration.

param([switch]$Restore)

$ErrorActionPreference = 'Stop'
Set-Location -LiteralPath $PSScriptRoot

$cs = (Get-Content (Join-Path $PSScriptRoot 'src\DigiCard.Web\appsettings.json') -Raw |
       ConvertFrom-Json).ConnectionStrings.DefaultConnection

$slug = 'golab-abrang'

# Names moved down, tilted a few degrees and set in nastaliq; a line of poetry added under the
# rule. Coordinates are percentages of the card, so this holds at any size.
$demo = @'
[{"id":"kicker","role":"kicker","x":10,"y":34,"w":80,"z":10,"style":{"fontSize":3,"font":"body","weight":"normal","align":"center","color":"muted","lineHeight":1.6,"opacity":1}},
 {"id":"names","role":"names","x":6,"y":40,"w":88,"rotation":-3,"z":20,"style":{"fontSize":9.5,"font":"nastaliq","weight":"normal","align":"center","color":"ink","lineHeight":1.5,"opacity":1}},
 {"id":"divider","role":"divider","x":32,"y":54,"w":36,"h":3,"z":15,"style":{"align":"center","color":"accent","opacity":0.8}},
 {"id":"poem","role":"poem","x":10,"y":58.5,"w":80,"z":10,"text":"دو دل یک شد و یک دل، دو جهان روشن شد","style":{"fontSize":3.4,"font":"nastaliq","weight":"normal","align":"center","color":"accent","lineHeight":2.1,"opacity":1}},
 {"id":"message","role":"message","x":12,"y":67,"w":76,"z":10,"style":{"fontSize":3.4,"font":"body","weight":"normal","align":"center","color":"ink","lineHeight":1.9,"opacity":1}}]
'@ -replace "`r?`n\s*", ""

function Invoke-Sql {
    param([System.Data.SqlClient.SqlConnection]$Conn, [string]$Sql, [hashtable]$Args = @{})
    $cmd = $Conn.CreateCommand()
    $cmd.CommandText = $Sql
    foreach ($k in $Args.Keys) { [void]$cmd.Parameters.AddWithValue($k, $Args[$k]) }
    return $cmd.ExecuteNonQuery()
}

$conn = New-Object System.Data.SqlClient.SqlConnection $cs
try {
    $conn.Open()

    Invoke-Sql $conn @"
IF OBJECT_ID('templates.LayoutBackup') IS NULL
    CREATE TABLE templates.LayoutBackup (Slug nvarchar(60) NOT NULL PRIMARY KEY, Elements nvarchar(max) NOT NULL);
"@ | Out-Null

    if ($Restore) {
        $rows = Invoke-Sql $conn @"
UPDATE t SET t.Elements = b.Elements
FROM templates.Templates t JOIN templates.LayoutBackup b ON b.Slug = t.Slug
WHERE t.Slug = @slug;
DELETE FROM templates.LayoutBackup WHERE Slug = @slug;
"@ @{ '@slug' = $slug }

        if ($rows -eq 0) {
            Write-Host "Nothing to restore - no backup was saved for $slug." -ForegroundColor Yellow
            return
        }
        Write-Host "Restored $slug to its previous layout." -ForegroundColor Cyan
    }
    else {
        # Saved once. Running the demo twice must not overwrite the backup with the demo itself.
        Invoke-Sql $conn @"
INSERT INTO templates.LayoutBackup (Slug, Elements)
SELECT Slug, Elements FROM templates.Templates
WHERE Slug = @slug AND NOT EXISTS (SELECT 1 FROM templates.LayoutBackup WHERE Slug = @slug);
"@ @{ '@slug' = $slug } | Out-Null

        Invoke-Sql $conn "UPDATE templates.Templates SET Elements = @e WHERE Slug = @slug" `
            @{ '@e' = $demo; '@slug' = $slug } | Out-Null

        Write-Host "Applied the demo layout to $slug." -ForegroundColor Cyan
        Write-Host "The previous JSON is kept in templates.LayoutBackup."
    }

    Write-Host ""
    Write-Host "Now open  /templates/$slug  and refresh with Ctrl+F5." -ForegroundColor Green
    Write-Host "No rebuild, no code change - only one column in the database moved."
}
finally { $conn.Dispose() }
