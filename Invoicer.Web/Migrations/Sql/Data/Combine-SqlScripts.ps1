# Define the folder containing the SQL files
$sourceFolder = "..\"
$outputFile = "CombinedScript.sql"

# Get all .sql files sorted alphabetically by name
$sqlFiles = Get-ChildItem -Path $sourceFolder -Filter "*.sql" | Sort-Object Name

# Clear the output file if it exists
if (Test-Path $outputFile) {
    Remove-Item $outputFile
}

# Combine all SQL scripts
foreach ($file in $sqlFiles) {
    Add-Content -Path $outputFile -Value "-- Begin $($file.Name)"
    Add-Content -Path $outputFile -Value (Get-Content -Path $file.FullName -Raw)
    Add-Content -Path $outputFile -Value "`n-- End $($file.Name)`n"
}

Write-Host "Combined $($sqlFiles.Count) files into $outputFile"

