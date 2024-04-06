@ECHO OFF
IF "%~1"==":historySafe" GOTO :historySafe
cmd /c "%~f0" :historySafe
EXIT /b

:historySafe
IF NOT DEFINED USER SET /p "USER=User: "
IF NOT DEFINED MYSQL_PWD SET /p "MYSQL_PWD=Password: "
IF NOT DEFINED MK8_DB SET /p "MK8_DB=MK8 DB: "

ECHO Deploying Locations...
CALL "%~dp0/../Countries/MySql/CountryDeploy.cmd"
CALL "%~dp0/../Regions/MySql/RegionDeploy.cmd"