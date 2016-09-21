module Common
#r @"FAKE.3.26.1/tools/FakeLib.dll"

open Fake
open Fake.XamarinHelper
open System
open System.Collections.Generic
open System.IO
open System.Linq
open Fake.AssemblyInfoFile

exception BuildException of string

let configuration = "Debug"
let architecture = "Any CPU"
let rootFolder = "../"
let getFolder solutionFile= Path.GetDirectoryName(solutionFile)

let Exec command args =
    let result = Shell.Exec(command, args)

    if result <> 0 then failwithf "%s exited with error %d" command result

let RestorePackages solutionFile =
    RestoreMSSolutionPackages (fun p -> 
        { p with
            Sources = "https://www.myget.org/F/waveengine-nightly/api/v2" :: p.Sources
            Retries = 5 
            OutputPath = Path.Combine(getFolder solutionFile, "packages") }) solutionFile

type status =
    | Success
    | Failed

type sampleReport = 
    {
        Result : status
        Path : string
    }

let items = new List<sampleReport>()

let processResults (path : string) (flag : bool) =
    let report : sampleReport = 
        {
            Result = if flag then status.Success else status.Failed
            Path = path
        }

    items.Add(report)

let mutable OkProjects = 0;

let printReport (l : List<sampleReport>) =
    printfn ""
    traceImportant "---------------------------------------------------------------------"
    traceImportant "Samples Report:"
    traceImportant "---------------------------------------------------------------------"
    l |> Seq.iteri (fun index item ->
        if l.[index].Result = status.Success then
            trace (index.ToString() + "-    Success " + l.[index].Path)
            OkProjects <- OkProjects + 1
        else
            traceError (index.ToString() + "-    Failed " + l.[index].Path))

    printfn ""
    printfn "   Projects success: %i / %i" OkProjects l.Count
    traceImportant "---------------------------------------------------------------------"
    printfn ""
    if (OkProjects < l.Count) then raise (BuildException("All samples not passed")) 

let buildSample (platform: string, configuration : string, architecture : string, sample : string) = 
    match platform with
    | "Windows" -> MSBuild null "Build" [("Configuration", configuration); ("Platform", architecture)] [sample] |> ignore
    | "Linux" -> Exec "xbuild" ("/p:Configuration=" + configuration + " " + sample)
    | "MacOS" -> Exec "/Applications/Xamarin Studio.app/Contents/MacOS/mdtool" ("-v build -t:Build -c:" + configuration + " " + sample)
    | _-> ()

let buildsamples(platform: string) =
    for sample in Directory.GetFiles(rootFolder, ("*" + platform + "*.sln"), SearchOption.AllDirectories) do
        traceImportant ("Project " + sample)

        let mutable flag = true

        try
            traceImportant ("restoring..")
            RestorePackages sample

            traceImportant ("Building...")
            

            buildSample (platform, configuration, architecture, sample)
        with
            | _ -> flag <- false
        
        processResults sample flag

    printReport items