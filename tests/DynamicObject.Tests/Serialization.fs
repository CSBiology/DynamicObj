module Serialization.Tests

open System
open System.Collections.Generic
open Fable.Pyxpecto
open DynamicObj

#if !FABLE_COMPILER
open Newtonsoft.Json
#endif

open DynamicObj

let test_object = 
    let obj = new DynamicObj()
    obj.SetProperty("string", "yes")
    obj.SetProperty("number", 69)
    obj.SetProperty("boolean", true)
    obj.SetProperty("array", ["First"; "Second"])
    obj.SetProperty(
        "object", 
            let tmp = new DynamicObj()
            tmp.SetProperty("inner", "yup")
        )
    obj

let tests_newtonsoft = testList "Newtonsoft (.NET)" [
#if !FABLE_COMPILER
    ftestCase "Serialize" <| fun _ ->
        let actual = JsonConvert.SerializeObject(test_object)
        Expect.equal actual """{"string":"yes","number":69,"boolean":true,"array":["First","Second"],"object":{"inner":"yup"}}""" ""

    testCase "Deserialize" <| fun _ ->
        ()
#endif
]

let main = testList "Serialization" [
    tests_newtonsoft
]