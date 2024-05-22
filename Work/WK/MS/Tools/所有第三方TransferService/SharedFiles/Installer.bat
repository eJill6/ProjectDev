rem @echo off 

rem cd /d %~dp0

rem set filename=%cd%\ProductTransferService.exe

echo ============================OperationLog==================================== >InstallService.log  
if exist "%SystemRoot%\Microsoft.NET\Framework\v4.0.30319" goto netOld 
:DispError 
echo .net Framework 4.0 is not installed on your machine, installing will be stop.
echo .net Framework 4.0 is not installed on your machine, installation will be stop. >>InstallService.log  
goto LastEnd 
:netOld 
echo installation will begin…
echo off
pause
cd %SystemRoot%\Microsoft.NET\Framework\v4.0.30319 
echo .net Framework 4.0 is installed on your machine.
echo .net Framework 4.0 is installed on your machine. >>InstallService.log  
echo uninstalling previous version service...
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil /U %filename% >> InstallService.log
echo uninstalling is completed
echo.
echo *********************
echo installation starts
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil %filename% >> InstallService.log
echo ======================================================================= >>InstallService.log 
type InstallService.log
echo.
echo operating is over. You may check InstallService.log.
:LastEnd 
pause