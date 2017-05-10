@echo off

if [%1]==[] goto :blank

set testFolder=%1

NuGet\nuget.exe install WaveEngine.WindowsTools -ExcludeVersion -ConfigFile NuGet\NuGet.config

xcopy  WaveEngine.WindowsTools\tools  WaveEngine.WindowsTools\v2.0\Tools\VisualEditor /Y /I

SET WaveEngine=%~dp0\WaveEngine.WindowsTools\


NuGet\nuget.exe install WaveEngine.VisualTestManager -ExcludeVersion -ConfigFile NuGet\NuGet.config
WaveEngine.VisualTestManager\tools\TestManager.exe batch --path %testFolder% --platform Windows

echo ##teamcity[publishArtifacts report.txt]
echo ##teamcity[publishArtifacts report.zip]

:blank
exit /b
