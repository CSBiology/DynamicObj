module ShallowCopyDynamicPropertiesTo

open Fable.Pyxpecto
open DynamicObj
open TestUtils

let tests_ShallowCopyDynamicPropertiesTo = testList "ShallowCopyDynamicPropertiesTo" [
    testCase "ExistingObject" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", 1)
        a.SetProperty("b", 2)
        let b = DynamicObj()
        b.SetProperty("c", 3)
        a.ShallowCopyDynamicPropertiesTo(b)
        Expect.equal (b.GetPropertyValue("a")) 1 "Value a should be copied"
        Expect.equal (b.GetPropertyValue("b")) 2 "Value b should be copied"
        Expect.equal (b.GetPropertyValue("c")) 3 "Value c should be unaffected"

    testCase "Overwrite" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b = DynamicObj()
        b.SetProperty("a", 3)
        Expect.notEqual a b "Values should not be equal before copying"
        a.ShallowCopyDynamicPropertiesTo(b, true)
        Expect.equal a b "Values should be equal"

    testCase "copies are only references" <| fun _ ->
        let a = DynamicObj()
        let inner = DynamicObj()
        inner.SetProperty("inner", 1)
        a.SetProperty("nested", inner)
        let b = DynamicObj()
        a.ShallowCopyDynamicPropertiesTo(b)
        Expect.equal a b "Value should be copied"
        inner.SetProperty("another", 2)
        Expect.equal a b "copied value was not mutated via reference"
]
