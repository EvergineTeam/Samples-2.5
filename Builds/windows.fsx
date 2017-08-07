#r @"FAKE.3.26.1/tools/FakeLib.dll"
#load "common.fsx"

open Fake
open Common

let WaveToolDirectory = "WaveEngine.WindowsTools"

Target "windows-restore-tools" (fun() ->
    traceImportant "Clear tools directory"
    DeleteDirs [WaveToolDirectory]

    traceImportant "Get WaveEngine.WindowsTools nuget packages"
    let nugetArgs = " install " + WaveToolDirectory + " -ExcludeVersion -PreRelease -ConfigFile NuGet\NuGet.config"
    trace nugetArgs
    Exec "NuGet/nuget.exe" nugetArgs

    traceImportant "Generate waveengine installation path"
    let target = WaveToolDirectory + "/v2.0/Tools/VisualEditor/"
    !! (WaveToolDirectory + "/tools/*.*")
        |> CopyFiles target
)

Target "windows-environment-var" (fun () ->
    let variablePath = System.IO.Path.GetFullPath("WaveEngine.WindowsTools/");
    trace variablePath
    let variableName = "WaveEngine"

    setEnvironVar variableName variablePath
    trace "Environment Variable created"
)

Target "windows-update-nightlypackages" (fun() ->
    traceImportant "Update to nightly nuget packages"
    Exec "WaveTools/UpdateToNightlyPackages.exe" rootFolder
)

Target "windows-samples" (fun () ->
    buildsamples("Windows")
)

"windows-restore-tools"
    ==> "windows-environment-var"
    ==> "windows-update-nightlypackages"
    ==> "windows-samples"


 