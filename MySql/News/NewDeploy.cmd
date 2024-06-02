@ECHO OFF
IF "%~1"==":historySafe" GOTO :historySafe
cmd /c "%~f0" :historySafe
EXIT /b

:historySafe
IF NOT DEFINED USER SET /p "USER=User: "
IF NOT DEFINED MYSQL_PWD SET /p "MYSQL_PWD=Password: "
IF NOT DEFINED MK8_DB SET /p "MK8_DB=MK8 DB: "

SET scripts="%~dp0/NewTable.sql"
SET scripts=%scripts%;"%~dp0/NewClear.sql"
SET scripts=%scripts%;"%~dp0/NewInsert.sql"
SET scripts=%scripts%;"%~dp0/NewList.sql"

ECHO Deploying News...
FOR %%S IN (%scripts%) DO mysql %MK8_DB% < %%S
