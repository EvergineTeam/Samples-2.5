@echo off
cls

"NuGet\nuget.exe" "install" "WaveEngine.Tools" "-ExcludeVersion" "-ConfigFile" "NuGet\MyGet.NuGet.config"

FAKE.3.26.1\tools\FAKE.exe build.fsx %*