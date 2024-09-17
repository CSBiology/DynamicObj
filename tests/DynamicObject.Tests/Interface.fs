﻿module Interface.Tests

open System
open Fable.Pyxpecto
open DynamicObj
open Fable.Core

type IPerson = 
    abstract member Id : string with get
    abstract member Name : string with get,set

[<AttachMembers>]
type Person(id : string, name : string) =

    inherit DynamicObj()

    let id = id
    let mutable name = name

    interface IPerson with
        member this.Id = id
        member this.Name
            with get() = name
            and set(value) = name <- value


let tests_set = testList "Set" [

    testCase "Static Property" <| fun _ ->
        let p = Person("123","John")
        p.SetValue("Name", "Jane")
        Expect.equal (p : IPerson).Name "Jane" "Static property should be set"
        Expect.equal (p.TryGetValue("Name")) (Some "Jane") "Static property should be retreivable dynamically"

    testCase "Static Immutable Property" <| fun _ ->
        let p = Person("123","John")
        let f = fun () -> p.SetValue("Id", "321")
        Expect.throws f "Cannot set static property"

    testCase "Dynamic Property" <| fun _ ->
        let p = Person("123","John")
        p.SetValue("Age", 42)
        Expect.equal (p.TryGetValue("Age")) (Some 42) "Dynamic property should be set"
        Expect.equal (p.TryGetValue("Name")) (Some "John") "Static property should be retreivable dynamically"

    testCase "Dynamic Property Equality" <| fun _ ->
        let p1 = Person("123","John")
        let p2 = Person("123","John")

        p1.SetValue("Age", 42)
        p2.SetValue("Age", 42)

        Expect.equal p1 p2 "Values should be equal"
        Expect.equal (p1.GetHashCode()) (p2.GetHashCode()) "Hash codes should be equal"

    testCase "Dynamic Property Only on one" <| fun _ ->
        let p1 = Person("123","John")
        let p2 = Person("123","John")

        p1.SetValue("Age", 42)

        Expect.notEqual p1 p2 "Values should not be equal"
        Expect.notEqual p2 p1 "Values should not be equal (Reversed equality)"
    ]

let tests_remove = testList "Remove" [
  
    testCase "Remove Static" <| fun _ ->
        let p = Person("123","John")

        p.Remove("Name") |> ignore
       
        Expect.equal (p : IPerson).Name null "Static property should "

    testCase "Remove Static Immutable" <| fun _ ->
        let p = Person("123","John")
        let f = fun () -> p.Remove("Id") |> ignore
        Expect.throws f "Cannot remove static property"

    testCase "Remove Dynamic" <| fun _ ->
        let p = Person("123","John")
       
        p.SetValue("Age", 42)

        p.Remove "Age" |> ignore

        let r = p.TryGetValue("Age")

        Expect.isNone r "Dynamic property should be removed"

    testCase "Remove only on one" <| fun _ ->
        let p1 = Person("123","John")
        let p2 = Person("123","John")

        p1.SetValue("Age", 42)
        p2.SetValue("Age", 42)

        p1.Remove "Age" |> ignore
            
        Expect.notEqual p1 p2 "Values should be unequal"
        Expect.notEqual (p1.GetHashCode()) (p2.GetHashCode()) "Hash codes should be unequal"

]

let tests_getProperties = testList "GetProperties" [

    testCase "Get Properties" <| fun _ ->
        let p = Person("123","John")
        p.SetValue("Age", 42)
        let properties = p.GetPropertyHelpers(true)
        let names = properties |> Seq.map (fun p -> p.Name)
        Expect.equal (Seq.toList names) ["Id"; "Name"; "Age"] "Should have all properties"
]



let tests_formatString = testList "FormatString" [

    testCase "Format string 1" <| fun _ ->
        
        let id = "123"
        let name = "John"
        let age = 20
        let p = Person(id, name)
        p.SetValue("age", age)
        let expected = $"Id: {id}{System.Environment.NewLine}Name: {name}{System.Environment.NewLine}?age: {age}"
        Expect.equal (p |> DynObj.format) expected "Format string 1 failed"
]


let tests_print = testList "Print" [

    testCase "Test Print For Issue 14" <| fun _ ->
        let outer = DynamicObj()
        let inner = DynamicObj()
        inner.SetValue("Level", "Information")
        inner.SetValue("MessageTemplate","{Method} Request at {Path}")
        outer.SetValue("serilog", inner)

        let print =
            try 
                outer |> DynObj.print
                true
            with
                | e -> false

        Expect.isTrue print "Print failed for issue 14"
]

let main = testList "Interface" [
    tests_set
    tests_remove
    tests_getProperties
    tests_print
    tests_formatString
]