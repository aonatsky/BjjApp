$env:PGPASSWORD = 'ZRTeam2017';
psql --username=postgres -w -c "CREATE DATABASE trnmnt_app_dev"
psql --username=postgres -w -c "CREATE USER TRNMNTAPPDEV with PASSWORD '1'"
psql --username=postgres -w -c "GRANT ALL ON DATABASE trnmnt_app_dev TO TRNMNTAPPDEV;"
