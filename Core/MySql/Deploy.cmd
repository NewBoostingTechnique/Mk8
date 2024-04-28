@ECHO OFF
IF "%~1"==":historySafe" GOTO :historySafe
cmd /c "%~f0" :historySafe
EXIT /b

:historySafe
IF NOT DEFINED USER SET /p "USER=Login: "
IF NOT DEFINED MYSQL_PWD SET /p "MYSQL_PWD=Password: "
IF NOT DEFINED MK8_DB SET /p "MK8_DB=MK8 DB: "
IF NOT DEFINED MK8_PWD SET /p "MK8_PWD=MK8 Password: "

ECHO Deploying Database...
mysql -e "CREATE DATABASE %MK8_DB%"
mysql -e "CREATE USER IF NOT EXISTS 'mk8'@'%%' IDENTIFIED BY '%MK8_PWD%';"
mysql -e "USE %MK8_DB%; GRANT EXECUTE ON * TO 'mk8'@'%%';"
CALL "%~dp0/../Courses/MySql/CourseDeploy.cmd"
CALL "%~dp0/../Locations/MySql/LocationDeploy.cmd"
CALL "%~dp0/../Persons/MySql/PersonDeploy.cmd"
CALL "%~dp0/../ProofTypes/MySql/ProofTypeDeploy.cmd"
CALL "%~dp0/../Players/MySql/PlayerDeploy.cmd"
CALL "%~dp0/../Times/MySql/TimeDeploy.cmd"
CALL "%~dp0/../Logins/MySql/LoginDeploy.cmd"
CALL "%~dp0/../News/MySql/NewsDeploy.cmd"
