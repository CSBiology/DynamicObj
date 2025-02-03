module TryGetPropertyHelper

open Fable.Pyxpecto
open DynamicObj
open TestUtils

let tests_TryGetPropertyHelper = testList "TryGetPropertyHelper" [
    testCase "NonExisting" <| fun _ -> 
        let a = DynamicObj()
        let b = a.TryGetPropertyHelper("a")
        Expect.isNone b "Value should not exist"

    testCase "Existing dynamic property" <| fun _ -> 
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b =  Expect.wantSome (a.TryGetPropertyHelper("a")) "Value should exist"
        Expect.isFalse b.IsStatic "Properties should be static"
        Expect.isTrue b.IsDynamic "Properties should not be dynamic"
        Expect.isTrue b.IsMutable "Properties should be mutable"
        Expect.isFalse b.IsImmutable "Properties should not be immutable"

    #if !FABLE_COMPILER // Properties field is dotnet only as js and py use native properties
    testCase "Existing static property" <| fun _ -> 
        let a = DynamicObj()
        let b =  Expect.wantSome (a.TryGetPropertyHelper("Properties")) "Value should exist"
        Expect.isTrue b.IsStatic "Properties should be static"
        Expect.isFalse b.IsDynamic "Properties should not be dynamic"
        Expect.isTrue b.IsMutable "Properties should be mutable"
        Expect.isFalse b.IsImmutable "Properties should not be immutable"
    #endif
]