@echo off 

cd /d %~dp0

set filename=%cd%\AgTransferService.exe
set servicename=AgTransferService
echo 启动服务
net start %servicename% >> InstallService.log
echo *********************
echo ======================================================================= >>InstallService.log 
type InstallService.log
echo.
echo 操作结束，可以查看日志文件InstallService.log 中具体的操作结果。
:LastEnd 
pause
rem exit