module DynamicObj.Tests.TryGetDynamicPropertyHelper

open Fable.Pyxpecto
open DynamicObj
open TestUtils

let tests_TryGetDynamicPropertyHelper = testList "TryGetDynamicPropertyHelper" [
    testCase "NonExisting" <| fun _ -> 
        let a = DynamicObj()
        let b = a.TryGetDynamicPropertyHelper("a")
        Expect.isNone b "Value should not exist"

    testCase "Existing dynamic property" <| fun _ -> 
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b =  Expect.wantSome (a.TryGetDynamicPropertyHelper("a")) "Value should exist"
        Expect.isFalse b.IsStatic "Properties should be static"
        Expect.isTrue b.IsDynamic "Properties should not be dynamic"
        Expect.isTrue b.IsMutable "Properties should be mutable"
        Expect.isFalse b.IsImmutable "Properties should not be immutable"

    testCase "static property not retrieved as dynamic" <| fun _ -> 
        let a = DynamicObj()
        Expect.isNone (a.TryGetDynamicPropertyHelper("Properties")) "static property should not be retrieved via TryGetDynamicPropertyInfo"
]