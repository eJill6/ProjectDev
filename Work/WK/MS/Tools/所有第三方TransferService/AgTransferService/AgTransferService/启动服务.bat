@echo off 

cd /d %~dp0

set filename=%cd%\AgTransferService.exe
set servicename=AgTransferService
echo ��������
net start %servicename% >> InstallService.log
echo *********************
echo ======================================================================= >>InstallService.log 
type InstallService.log
echo.
echo �������������Բ鿴��־�ļ�InstallService.log �о���Ĳ��������
:LastEnd 
pause
rem exit