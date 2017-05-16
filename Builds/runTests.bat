@echo off

if [%1]==[] goto :blank

set testFolder=%1

NuGet\nuget.exe install WaveEngine.WindowsTools -ExcludeVersion -ConfigFile NuGet\NuGet.config

xcopy  WaveEngine.WindowsTools\tools  WaveEngine.WindowsTools\v2.0\Tools\VisualEditor /Y /I

SET WaveEngine=%~dp0\WaveEngine.WindowsTools\


NuGet\nuget.exe install WaveEngine.VisualTestManager -ExcludeVersion -ConfigFile NuGet\NuGet.config
WaveEngine.VisualTestManager\tools\TestManager.exe batch --path %testFolder% --platform Windows

set reportTXT=%~dp0%testFolder%\report.txt
set reportZIP=%~dp0%testFolder%\report.zip

echo ##teamcity[publishArtifacts '%reportTXT%']
echo ##teamcity[publishArtifacts '%reportZIP%']

:blank
exit /b
