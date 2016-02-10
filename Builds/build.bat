@echo off
cls

REM Clean previous version
rmdir /S /Q WaveEngine.Tools

"NuGet\nuget.exe" "install" "WaveEngine.Tools" "-ExcludeVersion" "-ConfigFile" "NuGet\MyGet.NuGet.config"

REM Copy WaveTools to accommodate to current Windows Wave Targets
xcopy /E /I /Y WaveEngine.Tools\tools WaveEngine.Tools\v2.0\Tools\VisualEditor\

FAKE.3.26.1\tools\FAKE.exe build.fsx %*