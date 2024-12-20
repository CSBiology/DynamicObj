module CopyUtils.Tests

open System
open Fable.Pyxpecto
open DynamicObj
open Fable.Core

let main = testList "CopyUtils.tryDeepCopyObj" [
    DeepCopyPrimitives.tests_DeepCopyPrimitives
]

