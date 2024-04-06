@ECHO OFF
IF "%~1"==":historySafe" GOTO :historySafe
cmd /c "%~f0" :historySafe
EXIT /b

:historySafe
IF NOT DEFINED USER SET /p "USER=User: "
IF NOT DEFINED MYSQL_PWD SET /p "MYSQL_PWD=Password: "
IF NOT DEFINED MK8_DB SET /p "MK8_DB=MK8 DB: "

SET scripts="%~dp0/RegionTable.sql"
SET scripts=%scripts%;"%~dp0/RegionIdentify.sql"
SET scripts=%scripts%;"%~dp0/RegionList.sql"

ECHO Deploying Regions...
FOR %%S IN (%scripts%) DO mysql %MK8_DB% < %%S