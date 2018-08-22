Write-Host "Build" -ForegroundColor Red;
# Pause;

$SSHKeyPath = "C:\Users\User\Dropbox\trnmnt\private_win_work.ppk";
$HostName = "188.166.165.48";
$UserName = "root";
$FullMigrationsFolderPath = [System.IO.Path]::GetFullPath("./migrations/");
$LatestMigration = Get-ChildItem -Path $FullMigrationsFolderPath | Sort-Object LastAccessTime -Descending | Select-Object -First 1
$DOMigrationApth

Function Execute-DO {
    param([string]$Command)
    plink -ssh -i $SSHKeyPath $UserName@$HostName ($Command);
}


Function Copy-To-DO {
    param([string]$Source, [string]$Target)
    pscp -i key $SSHKeyPath $Source ${UserName}@${HostName}:${$Target} 
}

try {
    Write-Host "Executing linux commands" -ForegroundColor Green;
    # plink -ssh -i C:\Users\User\Dropbox\trnmnt\private_win_work.ppk $UserName@$HostName ("cd /var/aspnet/db; mkdir test;");
    Write-Host "Stop trnmnt_dev service" -ForegroundColor Green;
    Execute-DO -Command "sudo systemctl stop trnmnt-dev.service;"
    Write-Host "Copy and apply migration" -ForegroundColor Green;
    
    Write-Host "Start trnmnt_dev service" -ForegroundColor Green;
    Execute-DO -Command "sudo systemctl start trnmnt-dev.service;"
    Write-Host "Clearing app folder" -ForegroundColor Green;
    Execute-DO -Command "cd /var/aspnet/db; mkdir test;"
    Pop-Location;
    # Pause;
}
catch {
    Write-Host $_.Exception.Message
}
 
Function Execute-DigitalOcean {
    param([string]$Command)
    plink -ssh -i C:\Users\User\Dropbox\trnmnt\private_win_work.ppk $UserName@$HostName ($Command);
}
