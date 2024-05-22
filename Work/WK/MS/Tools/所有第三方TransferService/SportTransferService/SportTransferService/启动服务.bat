@echo off 

cd /d %~dp0

set filename=%cd%\SportTransferService.exe
set servicename=SportTransferService
echo start service
net start %servicename% >> InstallService.log
echo *********************
echo ======================================================================= >>InstallService.log 
type InstallService.log
echo.
echo operating is over. You may check InstallService.log.
:LastEnd 
pause
rem exit