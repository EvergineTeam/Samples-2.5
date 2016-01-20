#r @"FAKE.3.26.1/tools/FakeLib.dll"

#load "windows.fsx"
open Fake

let BinariesDir = "../Binaries/"

Target "clean" (fun _ ->
    DeleteDirs [BinariesDir]
)

RunTarget()
