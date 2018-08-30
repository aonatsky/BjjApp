Write-Host "Creating migration" -ForegroundColor Red;
# Pause;


$DataProjectPath = "./../TRNMNT.Data/";
$StartupProjectPath = "./../TRNMNT/";
$datestring =  "{0:yyyyMMdd}" -f (get-date);
$MigrationsPublishPath = "./migrations/migration_$datestring.sql";
$FullPublishPath = [System.IO.Path]::GetFullPath($MigrationsPublishPath);
try {
    Write-Host "Build and publish" -ForegroundColor Green;

    Write-Host "Creating migration script" -ForegroundColor DarkGreen;
    Push-Location -Path $DataProjectPath;
    dotnet ef --startup-project $StartupProjectPath migrations script -i -o $FullPublishPath;
    Pop-Location;
    Write-Host "Done" -ForegroundColor DarkGreen;
    # Pause;
}
catch {
    Write-Host $_.Exception.Message
}
 