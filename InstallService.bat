@echo off
chcp 65001
REM Install
%SystemRoot%/Microsoft.NET/Framework/v4.0.30319/InstallUtil.exe %~DP0/MarketService/bin/Debug/MarketService.exe
pause