@echo off

if [%1]==[] goto :blank

set testFolder=%1

NuGet\nuget.exe install WaveEngine.VisualTestManager -ExcludeVersion -ConfigFile NuGet\NuGet.config

WaveEngine.VisualTestManager\tools\TestManager.exe batch --path %testFolder% --platform Windows

echo ##teamcity[publishArtifacts %~dp0%testFolder%\report.txt]
echo ##teamcity[publishArtifacts %~dp0%testFolder%\report.zip]

:blank
exit /b


