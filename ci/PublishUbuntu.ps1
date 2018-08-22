Write-Host "Build" -ForegroundColor Red;
# Pause;

$ProjectPath = "./../TRNMNT/";
$PublishPath = $ProjectPath + "/publish";
If(!(test-path $PublishPath))
{
      New-Item -ItemType Directory -Force -Path $PublishPath
}
$FullPublishPath = [System.IO.Path]::GetFullPath($PublishPath);
try {
    Write-Host "Build and publish" -ForegroundColor Green;
    Write-Host "Clearing publish folder" -ForegroundColor Green;
    Remove-Item "$PublishPath\*" -Recurse;
    Write-Host "Building project" -ForegroundColor DarkGreen;
    Push-Location -Path $ProjectPath;
    npm install;
    dotnet publish --configuration Release "TRNMNT.Web.csproj" -o $FullPublishPath -r "ubuntu.16.04-x64";
    Pop-Location;
    # Pause;
}
catch {
    Write-Host $_.Exception.Message
}
 