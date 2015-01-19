@echo off

set GameServer=..\GameServer
set GameServerCompiled=CompiledGameServer
set GameServerProjectName=GameNetworkServer.csproj

rd %GameServerCompiled% /S /Q

IF EXIST "%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe" SET MSBuildDir=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319
IF EXIST "%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" SET MSBuildDir=%WINDIR%\Microsoft.NET\Framework\v4.0.30319

"%MSBuildDir%\MSBuild.exe" "%GameServer%\%GameServerProjectName%" /p:Configuration=Release;platform=x86
if %ERRORLEVEL% GTR 0 goto Error

md %GameServerCompiled%
if %ERRORLEVEL% GTR 0 goto Error

copy /Y "%GameServer%\bin\x86\Release\*" "%GameServerCompiled%\"
if %ERRORLEVEL% GTR 0 goto Error

@echo Yet another successful build.

if "%1" == "automated" goto End
pause

:End
exit %ERRORLEVEL%

:Error
@echo Build was broken.
if "%1" == "automated" goto End
pause