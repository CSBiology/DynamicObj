module Inheritance.Tests

open System
open Fable.Pyxpecto
open DynamicObj
open Fable.Core

[<AttachMembers>]
type Person(name : string) =
    
    inherit DynamicObj()

    let mutable name = name

    member this.Name
        with get() = name
        and set(value) = name <- value

[<AttachMembers>]
type PersonImmutable(name : string) =
    
    inherit DynamicObj()

    member this.Name
        with get() = name

let tests_set = testList "Set" [

    testCase "Static Property" <| fun _ ->
        let p = Person("John")
        p.SetValue("Name", "Jane")
        Expect.equal p.Name "Jane" "Static property should be set"
        Expect.equal (p.TryGetValue("Name")) (Some "Jane") "Static property should be retreivable dynamically"

    testCase "Static Immutable Property" <| fun _ ->
        let p = PersonImmutable("John")
        let f = fun () -> p.SetValue("Name", "Jane")
        Expect.throws f "Cannot set static property"

    testCase "Dynamic Property" <| fun _ ->
        let p = Person("John")
        p.SetValue("Age", 42)
        Expect.equal (p.TryGetValue("Age")) (Some 42) "Dynamic property should be set"
        Expect.equal (p.TryGetValue("Name")) (Some "John") "Static property should be retreivable dynamically"

    testCase "Dynamic Property Equality" <| fun _ ->
        let p1 = Person("John")
        let p2 = Person("John")

        p1.SetValue("Age", 42)
        p2.SetValue("Age", 42)

        Expect.equal p1 p2 "Values should be equal"
        Expect.equal (p1.GetHashCode()) (p2.GetHashCode()) "Hash codes should be equal"

    ptestCase "Dynamic Property Only on one" <| fun _ ->
        let p1 = Person("John")
        let p2 = Person("John")

        p1.SetValue("Age", 42)

        Expect.notEqual p1 p2 "Values should not be equal"
        Expect.notEqual p2 p1 "Values should not be equal (Reversed equality)"
    ]

let tests_remove = testList "Remove" [
  
    testCase "Remove Static" <| fun _ ->
        let p = Person("John")

        p.Remove("Name") |> ignore
       
        Expect.equal p.Name null "Static property should "

    testCase "Remove Dynamic" <| fun _ ->
        let p = Person("John")
       
        p.SetValue("Age", 42)

        p.Remove "Age" |> ignore

        let r = p.TryGetValue("Age")

        Expect.isNone r "Dynamic property should be removed"

    testCase "Remove only on one" <| fun _ ->
        let p1 = Person("John")
        let p2 = Person("John")

        p1.SetValue("Age", 42)
        p2.SetValue("Age", 42)

        p1.Remove "Age" |> ignore
            
        Expect.notEqual p1 p2 "Values should be unequal"
        Expect.notEqual (p1.GetHashCode()) (p2.GetHashCode()) "Hash codes should be unequal"

]


let tests_formatString = testList "FormatString" [

    testCase "Format string 1" <| fun _ ->
        
        let name = "John"
        let age = 20
        let p = Person("John")
        p.SetValue("age", age)
        let expected = $"Name: {name}{System.Environment.NewLine}?age: {age}"
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

let main = testList "Inheritance" [
    tests_set
    tests_remove
    tests_formatString
]