@echo off 

cd /d %~dp0

set filename=%cd%\IMBGTransferService.exe
set servicename=IMBGTransferService
echo ============================操作日志==================================== >UnInstallService.log  
if exist "%SystemRoot%\Microsoft.NET\Framework\v4.0.30319" goto netOld 
:DispError 
echo 您的机器上没有安装.net FrameWork 4.0,卸载即将终止 
echo 您的机器上没有安装.net FrameWork 4.0,卸载即将终止 >>UnInstallService.log  
pause 
goto LastEnd 
:netOld 
echo 即将卸载本服务…
echo off 
pause 
echo *********************
echo 停止服务
net stop %servicename% >>UnInstallService.log 
cd %SystemRoot%\Microsoft.NET\Framework\v4.0.30319 
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil /uninstall %filename% >>UnInstallService.log 
echo 卸载服务
echo ======================================================================= >>UnInstallService.log 
echo *********************
type UnInstallService.log 
echo.
echo 操作结束，可以查看日志文件UnInstallService.log中具体的操作结果。
:LastEnd 
pause 
rem exit

