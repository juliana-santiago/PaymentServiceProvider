/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P Mssql!Passw0rd -d master -i /tmp/01-create-psp-database.sql
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P Mssql!Passw0rd -d master -i /tmp/02-insert-psp-data.sql