#r @"FAKE.3.26.1/tools/FakeLib.dll"

#load "common.fsx"

open Fake
open Common

let WaveToolDirectory = "WaveEngine.LinuxTools"

Target "linux-restore-tools" (fun() ->
    traceImportant "Clear tools directory"
    DeleteDirs [WaveToolDirectory]

    traceImportant "Get WaveEngine.LinuxTools nuget packages"
    let nugetArgs = " install " + WaveToolDirectory + " -ExcludeVersion -PreRelease -ConfigFile NuGet/NuGet.config"
    trace nugetArgs
    Exec "NuGet/nuget.exe" nugetArgs

    traceImportant "Generate waveengine installation path"
    let target = "/usr/lib/WaveEngine/2.0/Tools/VisualEditor/"
    !! (WaveToolDirectory + "/tools/*.*")
        |> CopyFiles target
)

Target "linux-update-nightlypackages" (fun() ->
    traceImportant "Update to nightly nuget packages"
    let args = "WaveTools/UpdateToNightlyPackages.exe " + rootFolder
    Exec "mono" args
)

Target "linux-samples" (fun () ->
    buildsamples("Linux")
)

"linux-restore-tools"
    ==> "linux-update-nightlypackages"
    ==> "linux-samples"