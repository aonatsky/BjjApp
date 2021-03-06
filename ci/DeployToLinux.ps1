Write-Host "Build" -ForegroundColor Red;
# Pause;

$SSHKeyPath = "$env:USERPROFILE\Dropbox\trnmnt\private_win_work.ppk";
$HostName = "188.166.165.48";
$UserName = "root";
$FullMigrationsFolderPath = [System.IO.Path]::GetFullPath("./migrations/");
$LatestMigration = Get-ChildItem -Path $FullMigrationsFolderPath | Sort-Object LastAccessTime -Descending | Select-Object -First 1
$DOMigrationsPath = "/var/aspnet/db/migrations/";
$FullPublishPath = [System.IO.Path]::GetFullPath("./../TRNMNT/publish/");
$DbName = "trnmnt_dev";
$DOAspNetPath = "/var/aspnet";
$DOAppPath = "$DOAspNetPath/trnmnt/";


Function Execute-DO {
    param([string]$Command)
    plink -ssh -i $SSHKeyPath $UserName@$HostName ($Command);
}


Function Copy-To-DO {
    param([string]$Source, [string]$Target)
    $p = "${UserName}@${HostName}:${Target}"
    pscp -r -i $SSHKeyPath $Source "${UserName}@${HostName}:${Target}" 
}


try {
    Write-Host "Executing linux commands" -ForegroundColor Green;
    Write-Host "Stop trnmnt_dev service" -ForegroundColor Green;
    Execute-DO -Command "sudo systemctl stop trnmnt-dev.service;"
    
    Write-Host "Copy and apply migration" -ForegroundColor Green;
    Copy-To-DO -Source $LatestMigration.FullName -Target $DOMigrationsPath;
    $command = 'cd ' + ${DOMigrationsPath} + '; FILENAME=$(ls -Art | tail -n 1 | xargs readlink -f); sudo -u postgres psql -d ' +$DbName+' -f $FILENAME'
    Execute-DO -Command $command;
    
    Write-Host "Clear app folder" -ForegroundColor Green;
    $command = "cd $DOAspNetPath && find -not -path '*/wwwroot/EventData*' -path '*/trnmnt/*' -delete"
    Execute-DO -Command $command

    Write-Host "Upload app folder" -ForegroundColor Green;
    Copy-To-DO -Source "$FullPublishPath\*" -Target $DOAppPath;
    
    Write-Host "Start trnmnt_dev service" -ForegroundColor Green;
    Execute-DO -Command "sudo systemctl start trnmnt-dev.service;"
    Pause;
}
catch {
    Write-Host $_.Exception.Message
}

