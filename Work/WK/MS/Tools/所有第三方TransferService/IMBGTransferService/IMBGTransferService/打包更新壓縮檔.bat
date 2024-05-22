rd /s /q IMBGTransferService
md IMBGTransferService
xcopy "bin/debug" "IMBGTransferService" /s
rd /s /q "IMBGTransferService\Logs"
rd /s /q "IMBGTransferService\x64"
rd /s /q "IMBGTransferService\x86"
del IMBGTransferService\*.config
del IMBGTransferService\*.xml
del IMBGTransferService\*.db
del IMBGTransferService\*.bat
"C:\Program Files\7-Zip\7z" a "UPDATE_IMBGTransferService.zip" "IMBGTransferService"
rd /s /q IMBGTransferService