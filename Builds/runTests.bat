@echo off

set testFolder=%1

rmdir /s /q WaveEngine.WindowsTools
NuGet\nuget.exe install WaveEngine.WindowsTools -ExcludeVersion -ConfigFile NuGet\NuGet.config -PreRelease
xcopy  WaveEngine.WindowsTools\tools  WaveEngine.WindowsTools\v2.0\Tools\VisualEditor /Y /I
set WaveEngine=%~dp0\WaveEngine.WindowsTools\

rmdir /s /q WaveEngine.VisualTestManager
NuGet\nuget.exe install WaveEngine.VisualTestManager -ExcludeVersion -ConfigFile NuGet\NuGet.config
WaveEngine.VisualTestManager\tools\TestManager.exe batch --path %testFolder% --platform Windows -t 8

if %ERRORLEVEL% NEQ 0 echo ##teamcity[buildStatus status='FAILURE' text='Visual tests failed'] 

set reportTXT=%~dp0%testFolder%\summary.txt
set reportZIP=%~dp0%testFolder%\summary.zip

echo ##teamcity[publishArtifacts '%reportTXT%']
echo ##teamcity[publishArtifacts '%reportZIP%']
