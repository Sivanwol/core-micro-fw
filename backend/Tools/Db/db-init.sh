#!/bin/bash
#wait for the SQL Server to come up
echo "padding init set up script"
sleep 30s

echo "running set up script"
#run the setup script to create the DB and the schema in the DB
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P developer123! -d master -i ./Tools/Db/db-init.sql