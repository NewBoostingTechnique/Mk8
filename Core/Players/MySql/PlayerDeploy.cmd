@ECHO OFF
IF "%~1"==":historySafe" GOTO :historySafe
cmd /c "%~f0" :historySafe
EXIT /b

:historySafe
IF NOT DEFINED USER SET /p "USER=User: "
IF NOT DEFINED MYSQL_PWD SET /p "MYSQL_PWD=Password: "
IF NOT DEFINED MK8_DB SET /p "MK8_DB=MK8 DB: "

SET scripts="%~dp0/PlayerTable.sql"
SET scripts=%scripts%;"%~dp0/PlayerDelete.sql"
SET scripts=%scripts%;"%~dp0/PlayerDetail.sql"
SET scripts=%scripts%;"%~dp0/PlayerExists.sql"
SET scripts=%scripts%;"%~dp0/PlayerIdentify.sql"
SET scripts=%scripts%;"%~dp0/PlayerInsert.sql"
SET scripts=%scripts%;"%~dp0/PlayerList.sql"

ECHO Deploying Players...
FOR %%S IN (%scripts%) DO mysql %MK8_DB% < %%S
