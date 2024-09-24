module Serialization.Tests

open System
open System.Collections.Generic
open Fable.Pyxpecto
open DynamicObj

#if !FABLE_COMPILER
open Newtonsoft.Json
#endif

open DynamicObj

let test_dynobj = 
    let obj = new DynamicObj()
    obj.SetProperty("dynamic_string", "yes")
    obj.SetProperty("dynamic_number", 69)
    obj.SetProperty("dynamic_boolean", true)
    obj.SetProperty("dynamic_array", ["First"; "Second"])
    obj.SetProperty(
        "dynamic_object", 
            let tmp = new DynamicObj()
            tmp.SetProperty("inner", "yup")
            tmp
        )
    obj

type DerivedClass1(staticProp: string) =
    inherit DynamicObj()
    member this.StaticProp = staticProp

type DerivedClass2(
    staticString: string,
    staticNumber: float,
    staticBoolean: bool,
    staticArray: string list,
    staticObject: DynamicObj
) =
    inherit DynamicObj()
    member this.StaticString = staticString
    member this.StaticNumber = staticNumber
    member this.StaticBoolean = staticBoolean
    member this.StaticArray = staticArray
    member this.StaticObject = staticObject

let test_derived_1 = 
    let obj = DerivedClass1("lol")
    obj.SetProperty("dynamicProp", 42)
    obj

let test_derived_2 = 
    let obj = DerivedClass2(
        "lol",
        42.0,
        true,
        ["First"; "Second"],
        test_derived_1
    )
    obj.SetProperty("dynamic_string", "yes")
    obj.SetProperty("dynamic_number", 69)
    obj.SetProperty("dynamic_boolean", true)
    obj.SetProperty("dynamic_array", ["First"; "Second"])
    obj.SetProperty(
        "dynamic_object", 
            let tmp = new DynamicObj()
            tmp.SetProperty("inner", "yup")
            tmp
        )
    obj

#if !FABLE_COMPILER
let tests_newtonsoft = testList "Newtonsoft (.NET)" [
    testCase "Serialize DynamicObj" <| fun _ ->
        let actual = JsonConvert.SerializeObject(test_dynobj)
        Expect.equal actual """{"dynamic_string":"yes","dynamic_number":69,"dynamic_boolean":true,"dynamic_array":["First","Second"],"dynamic_object":{"inner":"yup"}}""" ""

    testCase "Serialize simplederived class from DynamicObj" <| fun _ ->
        let actual = JsonConvert.SerializeObject(test_derived_1)
        Expect.equal actual """{"StaticProp":"lol","dynamicProp":42}""" ""

    testCase "Serialize complex derived class from DynamicObj" <| fun _ ->
        let actual = JsonConvert.SerializeObject(test_derived_2)
        Expect.equal actual """{"StaticString":"lol","StaticNumber":42.0,"StaticBoolean":true,"StaticArray":["First","Second"],"StaticObject":{"StaticProp":"lol","dynamicProp":42},"dynamic_string":"yes","dynamic_number":69,"dynamic_boolean":true,"dynamic_array":["First","Second"],"dynamic_object":{"inner":"yup"}}""" ""
]
#endif

// eventually, we want a transpilable, custom serialization
let tests_custom = testList "Custom" []

let main = testList "Serialization" [
    tests_custom
    #if !FABLE_COMPILER
    tests_newtonsoft
    #endif
]