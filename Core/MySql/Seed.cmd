@ECHO OFF
IF "%~1"==":historySafe" GOTO :historySafe
cmd /c "%~f0" :historySafe
EXIT /b

:historySafe
IF NOT DEFINED USER SET /p "USER=User: "
IF NOT DEFINED MYSQL_PWD SET /p "MYSQL_PWD=Password: "
IF NOT DEFINED MK8_DB SET /p "MK8_DB=MK8 DB: "

ECHO Seeding Database...
CALL "%~dp0/../Courses/MySql/CourseSeed.cmd"
CALL "%~dp0/../Locations/MySql/LocationSeed.cmd"
CALL "%~dp0/../ProofTypes/MySql/ProofTypeSeed.cmd"
CALL "%~dp0/../Users/MySql/UserSeed.cmd"