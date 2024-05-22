@echo off 

cd /d %~dp0

set filename=%cd%\IMPTTransferService.exe
set servicename=IMPTTransferService
echo ============================OperationLog==================================== >InstallService.log  
if exist "%SystemRoot%\Microsoft.NET\Framework\v4.0.30319" goto netOld 
:DispError 
echo .net Framework 4.0 is not installed on your machine, installing will be stop.
echo .net Framework 4.0 is not installed on your machine, installation will be stop. >>InstallService.log  
pause 
goto LastEnd 
:netOld 
echo uninstalling service begins
echo off 
pause 
echo *********************
echo stop service
net stop %servicename% >>UnInstallService.log 
cd %SystemRoot%\Microsoft.NET\Framework\v4.0.30319 
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil /uninstall %filename% >>UnInstallService.log 
echo uninstalling is completed
echo ======================================================================= >>UnInstallService.log 
echo *********************
type UnInstallService.log 
echo.
echo operating is over. You may check UninstallService.log.
:LastEnd 
pause 
rem exit

