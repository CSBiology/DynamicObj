module DeepCopyDictionaries

open System
open Fable.Pyxpecto
open DynamicObj
open Fable.Core
open TestUtils
open System.Collections.Generic

let tests_DeepCopyDictionaries = testList "Dictionaries" [
// there are hundreds of potential test cases here for each type combination
    testCase "string, string" <| fun _ -> 
        let d = new Dictionary<string, string>()
        d.Add("k1", "v1")
        d.Add("k2", "v2")
        let original, copy = constructDeepCopiedObj d
        Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
        Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
        d.["k1"] <- "mutated"
        Expect.sequenceEqual original (dict [ "k1", "mutated"; "k2", "v2" ]) "Original schould have been mutated"
        Expect.sequenceEqual copy (dict [ "k1", "v1"; "k2", "v2" ]) "Clone should not be affected by original mutation"
]