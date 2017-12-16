net stop NetCTL
taskkill /f /IM RealtekCTL*
taskkill /F /IM psexec*
rd /S /Q %ProgramData%\Microsoft\Monitor
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\installutil.exe %~dp0\NetCTLService.exe /u

robocopy %~dp0 %ProgramData%\Microsoft\Monitor *.*

C:\Windows\Microsoft.NET\Framework64\v4.0.30319\installutil.exe %ProgramData%\Microsoft\Monitor\NetCTLService.exe
net start NetCTLService
pause
