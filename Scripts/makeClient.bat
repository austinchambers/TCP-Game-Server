@echo off

set GameClient=..\GameClient
set GameClientCompiled=CompiledGameClient
set GameClientProjectName=GameNetworkClient.csproj

rd %GameClientCompiled% /S /Q

IF EXIST "%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe" SET MSBuildDir=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319
IF EXIST "%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" SET MSBuildDir=%WINDIR%\Microsoft.NET\Framework\v4.0.30319

"%MSBuildDir%\MSBuild.exe" "%GameClient%\%GameClientProjectName%" /p:Configuration=Release;platform=x86
if %ERRORLEVEL% GTR 0 goto Error

md %GameClientCompiled%
if %ERRORLEVEL% GTR 0 goto Error

copy /Y "%GameClient%\bin\x86\Release\*" "%GameClientCompiled%\"
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