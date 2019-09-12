set
powershell.exe -ExecutionPolicy Bypass -Command ".\setup.ps1"
IF %ERRORLEVEL% NEQ 0 (
  EXIT /B 1
)
