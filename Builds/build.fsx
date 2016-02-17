#r @"FAKE.3.26.1/tools/FakeLib.dll"

#load "common.fsx"
#load "windows.fsx"
#load "mac.fsx"
#load "linux.fsx"
open Fake

RunTarget()
