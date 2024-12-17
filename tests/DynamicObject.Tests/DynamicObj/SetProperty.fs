module SetProperty

open Fable.Pyxpecto
open DynamicObj
open TestUtils

let tests_SetProperty = testList "SetProperty" [

    //TODO: static property accession!

    testCase "Same String" <| fun _ ->
        let a = DynamicObj ()
        a.SetProperty("aaa", 5)
        let b = DynamicObj ()
        b.SetProperty("aaa", 5)
        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Different Strings" <| fun _ ->
        let a = DynamicObj ()
        a.SetProperty("aaa", 1212)
        let b = DynamicObj ()
        b.SetProperty("aaa", 5)
        Expect.notEqual a b "Values should not be equal"

    testCase "String only on one" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()
        b.SetProperty("aaa", 5)

        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual b a "Values should not be equal (Reversed equality)"

    testCase "Same lists different keys" <| fun _ ->
        let a' = DynamicObj ()
        let b' = DynamicObj ()
        a'.SetProperty("quack!", [1; 2; 3])
        b'.SetProperty("quack!1", [1; 2; 3])
        Expect.notEqual (a'.GetHashCode()) (b'.GetHashCode()) "Hash codes should not be equal"   
   
    testCase "Different lists" <| fun _ ->
        let a' = DynamicObj ()
        let b' = DynamicObj ()
        a'.SetProperty("quack!", [1; 2; 3])
        b'.SetProperty("quack!", [1; 2; 3; 4; 34])
        Expect.notEqual (a'.GetHashCode()) (b'.GetHashCode()) "Hash codes should not be equal"

    testCase "Nested Same List Same String" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        let a' = DynamicObj ()
        let b' = DynamicObj ()
        a'.SetProperty("quack!", [1; 2; 3])
        b'.SetProperty("quack!", [1; 2; 3])

        a.SetProperty("aaa", a')
        b.SetProperty("aaa", b')
        Expect.equal a' b' "New Values should be equal"
        Expect.equal a b "Old Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Old Hash codes should be equal"
        Expect.equal (a'.GetHashCode()) (b'.GetHashCode()) "New Hash codes should be equal"

    testCase "Nested Same List Different Strings" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        let a' = DynamicObj ()
        let b' = DynamicObj ()
        a'.SetProperty("quack!", [1; 2; 3])
        b'.SetProperty("quack!", [1; 2; 3])

        a.SetProperty("aaa", a')
        b.SetProperty("aaa1", b')
        Expect.equal a' b' "New Values should be equal"
        Expect.notEqual a b "Old Values should not be equal"
        Expect.equal (a'.GetHashCode()) (b'.GetHashCode()) "New Hash codes should be equal"
    ]
