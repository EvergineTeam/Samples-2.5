#r @"FAKE.3.26.1/tools/FakeLib.dll"

open Fake
open Fake
open Fake.XamarinHelper
open System
open System.Collections.Generic
open System.IO
open System.Linq
open Fake.AssemblyInfoFile

let configuration = "Release"
let Platform = "x86"

let rootFolder = "../"

let getFolder solutionFile= Path.GetDirectoryName(solutionFile)

let RestorePackages solutionFile =
    RestoreMSSolutionPackages (fun p -> 
        { p with
            Retries = 5 
            OutputPath = Path.Combine(getFolder solutionFile, "packages") }) solutionFile

type status =
    | Success
    | Failed

type sampleReport = 
    {
        Result : status
        Path : string
        ErrorMessage : string
    }

let items = new List<sampleReport>()

let processResults (path : string) (message : string) (flag : bool) =
    let report : sampleReport = 
        {
            Result = if flag then status.Success else status.Failed
            Path = path
            ErrorMessage = message
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

let buildsamples(platform: string) =
    for sample in Directory.GetFiles(rootFolder, ("*" + platform + ".sln"), SearchOption.AllDirectories) do
        trace ("Project " + sample)

        trace ("restoring..")
        RestorePackages sample

        trace ("Building...")
        let mutable flag = true
        let mutable error = String.Empty
        try
            MSBuild null "Build" [("Configuration", configuration); ("Platform", Platform)] [sample] |> ignore
        with
            | BuildException(errorMessage, errors) -> 
                (
                    flag <- false
                    error <- ErrorMessage.ToString()
                )
        
        processResults sample error flag

    printReport items

Target "windows-samples" (fun () ->
    buildsamples("Windows")
)

Target "macos-samples" (fun () ->
    buildsamples("MacOS")
)

Target "linux-samples" (fun () ->
    buildsamples("Linux")
)