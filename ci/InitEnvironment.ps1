$pwd = Read-Host -Prompt 'Input your pgsql password for user POSTGRES'
$env:PGPASSWORD = $pwd;
$dbAppUserName = "trnmnt_app_dev";
$dbAdminUserName = "postgres";
$dbName = "trnmnt_dev";
$dbpAppUserPass = "1";
psql --username=$dbAdminUserName -w -c "CREATE DATABASE $dbName"
psql --username=$dbAdminUserName -w -c "CREATE USER $dbAppUserName with PASSWORD '$dbpAppUserPass'"
psql --username=$dbAdminUserName -w -c "GRANT ALL ON DATABASE $dbName TO $dbAppUserName;"
Pop-Location;
Set-Location ../TRNMNT/;
dotnet ef database update;
psql --username=$dbAdminUserName -w -f "../sql/AddFederationPG.sql" -d $dbName
psql --username=$dbAdminUserName -w -f "../sql/AddEventPG.sql" -d $dbName
psql --username=$dbAdminUserName -w -f "../sql/AddTeamPG.sql" -d $dbName

