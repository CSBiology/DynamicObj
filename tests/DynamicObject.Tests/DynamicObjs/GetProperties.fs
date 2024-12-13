module DynamicObj.Tests.GetProperties

open Fable.Pyxpecto
open DynamicObj
open TestUtils

let tests_GetProperties = testList "GetProperties" [
    testCase "GetProperties" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", 1)
        a.SetProperty("b", 2)
        let properties = a.GetProperties(true) |> List.ofSeq
        let expected = [
            System.Collections.Generic.KeyValuePair("a", box 1)
            System.Collections.Generic.KeyValuePair("b", box 2)
        ]
        Expect.sequenceEqual properties expected "Should have all properties"
    testCase "returns static instance members when wanted" <| fun _ ->
        let a = DerivedClass(stat = "stat", dyn = "dyn")
        let properties = a.GetProperties(true) |> List.ofSeq |> List.sortBy (fun kv -> kv.Key)
        let expected = 
            [
                System.Collections.Generic.KeyValuePair("dyn", box "dyn")
                System.Collections.Generic.KeyValuePair("stat", box "stat")
            ] 
            |> Seq.sortBy (fun kv -> kv.Key)
        Expect.sequenceEqual properties expected "Should have all properties"
]