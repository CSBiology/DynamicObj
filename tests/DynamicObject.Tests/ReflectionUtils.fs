module ReflectionUtils.Tests

open System
open Fable.Pyxpecto
open DynamicObj
open Fable.Core

[<AttachMembers>]
type TestObject(id : string, name : string) =

    let id = id
    let mutable name = name

    member this.Id
        with get() = id

    member this.Name
        with get() = name
        and set(value) = name <- value


let tests_PropertyHelper = testList "PropertyHelper" [
    testCase "getStaticProperties" <| fun _ ->
        let p = TestObject("1", "test")
        let helpers = ReflectionUtils.getStaticProperties p
        Expect.hasLength helpers 2 "Should have 2 properties"
        let idOption = Array.tryFind (fun h -> h.Name = "Id") helpers
        let id = Expect.wantSome idOption "Should have Id property"
        Expect.equal id.IsStatic true "Id should be static"
        Expect.equal id.IsDynamic false "Id should not be dynamic"
        Expect.equal id.IsMutable false "Id should not be mutable"
        Expect.equal id.IsImmutable true "Id should be immutable"
        let nameOption = Array.tryFind (fun h -> h.Name = "Name") helpers
        let name = Expect.wantSome nameOption "Should have Name property"
        Expect.equal name.IsStatic true "Name should be static"
        Expect.equal name.IsDynamic false "Name should not be dynamic"
        Expect.equal name.IsMutable true "Name should be mutable"
        Expect.equal name.IsImmutable false "Name should not be immutable"
]

let tests_TryGetPropertyValue = testList "TryGetPropertyValue" [
    testCase "existing" <| fun _ ->
        let p = TestObject("1", "test")
        let nameOption = ReflectionUtils.tryGetPropertyValue p "Name"
        let name = Expect.wantSome nameOption "Should have mutable value"
        Expect.equal name "test" "Should have correct mutable value"

        let idOption = ReflectionUtils.tryGetPropertyValue p "Id"
        let id = Expect.wantSome idOption "Should have immutable value"
        Expect.equal id "1" "Should have correct immutable value"

    testCase "non-existing" <| fun _ ->
        let p = TestObject("1", "test")
        let option = ReflectionUtils.tryGetPropertyValue p "NonExisting"
        Expect.equal option None "Should not have value"
] 

let tests_TrySetPropertyValue = testList "TrySetPropertyValue" [
    testCase "mutable" <| fun _ ->
        let p = TestObject("1", "test")
        let wasSet = ReflectionUtils.trySetPropertyValue p "Name" "newName"
        Expect.isTrue wasSet "Should have set value"
        let nameOption = ReflectionUtils.tryGetPropertyValue p "Name"
        let name = Expect.wantSome nameOption "Should have mutable value"
        Expect.equal name "newName" "Should have correct mutable value"

    testCase "immutable" <| fun _ ->
        let p = TestObject("1", "test")
        let wasSet = ReflectionUtils.trySetPropertyValue p "Id" "newId"
        Expect.isFalse wasSet "Should not have set value"

        let idOption = ReflectionUtils.tryGetPropertyValue p "Id"
        let id = Expect.wantSome idOption "Should have immutable value"
        Expect.equal id "1" "Should have correct immutable value"

    testCase "non-existing" <| fun _ ->
        let p = TestObject("1", "test")
        let wasSet = ReflectionUtils.trySetPropertyValue p "address" "newAddress"
        Expect.isFalse wasSet "Should not have set value"

        let addressOption = ReflectionUtils.tryGetPropertyValue p "address"
        Expect.isNone addressOption "Should not have value"
]

let main = testList "ReflectionUtils" [
    tests_PropertyHelper
    tests_TryGetPropertyValue
    tests_TrySetPropertyValue
]