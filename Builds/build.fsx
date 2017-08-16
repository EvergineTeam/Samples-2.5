#r @"FAKE.4.63/tools/FakeLib.dll"

#load "common.fsx"
#load "windows.fsx"
#load "mac.fsx"
#load "linux.fsx"
open Fake

RunTarget()
