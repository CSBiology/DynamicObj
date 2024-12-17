module DynamicObj.Tests.ShallowCopyDynamicProperties

open Fable.Pyxpecto
open DynamicObj
open TestUtils

let tests_ShallowCopyDynamicProperties = testList "ShallowCopyDynamicProperties" [
    testCase "NewObject" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", 1)
        a.SetProperty("b", 2)
        let b = a.ShallowCopyDynamicProperties()
        Expect.equal a b "Values should be equal"

    testCase "copies are only references" <| fun _ ->
        let a = DynamicObj()
        let inner = DynamicObj()
        inner.SetProperty("inner", 1)
        a.SetProperty("nested", inner)
        let b = a.ShallowCopyDynamicProperties()
        Expect.equal a b "Value should be copied"
        inner.SetProperty("another", 2)
        Expect.equal a b "copied value was not mutated via reference"
]