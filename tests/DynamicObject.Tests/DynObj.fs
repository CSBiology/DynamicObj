module DynObj.Tests

open System
open System.Collections.Generic
open Fable.Pyxpecto
open DynamicObj

let tests_ofDict = ptestList "ofDict" [
    testCase "Test ofDict" <| fun _ ->
        let d = Dictionary()
        d.Add("key1", box 1)
        d.Add("key2", box 2)
        let dyn = DynObj.ofDict d
        Expect.equal (dyn.GetPropertyValue("key1")) 1 "Value should be 1"
        Expect.equal (dyn.GetPropertyValue("key2")) 2 "Value should be 2"
]

let tests_ofSeq = ptestList "ofSeq" [
    testCase "Test ofDict" <| fun _ ->
        let d = 
            seq {
                "key1", box 1
                "key2", box 2
            }
        let dyn = DynObj.ofSeq d
        Expect.equal (dyn.GetPropertyValue("key1")) 1 "Value should be 1"
        Expect.equal (dyn.GetPropertyValue("key2")) 2 "Value should be 2"
]

let tests_ofList = ptestList "ofList" [
    testCase "Test ofDict" <| fun _ ->
        let d = [
            "key1", box 1
            "key2", box 2
        ]
        let dyn = DynObj.ofList d
        Expect.equal (dyn.GetPropertyValue("key1")) 1 "Value should be 1"
        Expect.equal (dyn.GetPropertyValue("key2")) 2 "Value should be 2"
]

let tests_ofArray = ptestList "ofArray" [
    testCase "Test ofDict" <| fun _ ->
        let d = [|
            "key1", box 1
            "key2", box 2
        |]
        let dyn = DynObj.ofArray d
        Expect.equal (dyn.GetPropertyValue("key1")) 1 "Value should be 1"
        Expect.equal (dyn.GetPropertyValue("key2")) 2 "Value should be 2"
]

type Inner() =
    inherit DynamicObj()
    static member init(
        ?inner_value: string
    ) =
        Inner()
        |> DynObj.withOptionalProperty "inner_value" inner_value

type Outer() =
    inherit DynamicObj()
    static member init(
        ?A: int,
        ?B: string,
        ?Inner: Inner
    ) =
        Outer()
        |> DynObj.withOptionalProperty "A" A
        |> DynObj.withOptionalProperty "B" B
        |> DynObj.withOptionalProperty "Inner" Inner

let tests_combine = testList "combine" [

    testCase "Combine flat DOs" <| fun _ ->
        let target = DynamicObj()

        target.SetProperty("target-unique", [42])
        target.SetProperty("will-be-overridden", "WAS_NOT_OVERRIDDEN!")

        let source = DynamicObj()

        source.SetProperty("source-unique", [42; 32])
        source.SetProperty("will-be-overridden", "WAS_OVERRIDDEN =)")

        let combined = DynObj.combine target source

        let expected = DynamicObj()

        expected.SetProperty("target-unique", [42])
        expected.SetProperty("source-unique", [42; 32])
        expected.SetProperty("will-be-overridden", "WAS_OVERRIDDEN =)")

        Expect.equal expected combined "Combine flat DOs failed"

    testCase "Combine nested DOs" <| fun _ ->
        let target = DynamicObj()

        target.SetProperty("target-unique", 1337)
        target.SetProperty("will-be-overridden", -42)
        let something2BeCombined = DynamicObj()
        something2BeCombined.SetProperty("inner","I Am")
        let something2BeOverriden = DynamicObj()
        something2BeOverriden.SetProperty("inner","NOT_OVERRIDDEN")
        target.SetProperty("nested-will-be-combined", something2BeCombined)
        target.SetProperty("nested-will-be-overridden", something2BeOverriden)
    
        let source = DynamicObj()

        source.SetProperty("source-unique", 69)
        source.SetProperty("will-be-overridden", "WAS_OVERRIDDEN")
        let alsoSomething2BeCombined = DynamicObj()
        alsoSomething2BeCombined.SetProperty("inner_combined","Complete")
        source.SetProperty("nested-will-be-combined", alsoSomething2BeCombined)
        source.SetProperty("nested-will-be-overridden", "WAS_OVERRIDDEN")
    
        let combined = DynObj.combine target source
    
        let expected = DynamicObj()

        expected.SetProperty("source-unique", 69)
        expected.SetProperty("target-unique", 1337)
        expected.SetProperty("will-be-overridden", "WAS_OVERRIDDEN")
        expected.SetProperty("nested-will-be-overridden", "WAS_OVERRIDDEN")
        expected.SetProperty("nested-will-be-combined", 
            let inner = DynamicObj()
            inner.SetProperty("inner","I Am")
            inner.SetProperty("inner_combined","Complete")
            inner
            )

        Expect.equal expected combined "Combine nested DOs failed"

    testCase "Combine nested DOs with inheriting types" <| fun _ ->
        let outer1 = Outer.init(A = 1, B = "first", Inner = Inner.init(inner_value = "inner_first"))
        let outer2 = Outer.init(A = 2, B = "second", Inner = Inner.init(inner_value = "inner_second"))
        let expected = Outer.init(A = 2, B = "second", Inner = Inner.init(inner_value = "inner_second"))
        Expect.equal (expected) (DynObj.combine outer1 outer2 |> unbox) "Combine nested DOs with inheriting types failed"
]

let tests_tryGetTypedPropertyValue = testList "tryGetTypedPropertyValue" [
    
    testCase "typeof" <| fun _ -> 
        let a = typeof<int>
        Expect.equal a.Name "Int32" "Type should be Int32"

    testCase "NonExisting" <| fun _ -> 
        let a = DynamicObj()
        let b = DynObj.tryGetTypedPropertyValue<int> "a" a
        Expect.isNone b "Value should not exist"

    testCase "Correct Int" <| fun _ -> 
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b = DynObj.tryGetTypedPropertyValue<int> "a" a
        Expect.equal b (Some 1) "Value should be 1"

    testCase "Incorrect Int" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", "1")
        let b = DynObj.tryGetTypedPropertyValue<int> "a" a
        Expect.isNone b "Value should not be an int"

    testCase "Correct String" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", "1")
        let b = DynObj.tryGetTypedPropertyValue<string> "a" a
        Expect.equal b (Some "1") "Value should be '1'"

    testCase "Incorrect String" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b = DynObj.tryGetTypedPropertyValue<string> "a" a
        Expect.isNone b "Value should not be a string"

    testCase "Correct List" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", [1; 2; 3])
        let b = DynObj.tryGetTypedPropertyValue<int list> "a" a
        Expect.equal b (Some [1; 2; 3]) "Value should be [1; 2; 3]"

    ptestCase "Incorrect List" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", [1; 2; 3])
        let b = DynObj.tryGetTypedPropertyValue<string list> "a" a
        Expect.isNone b "Value should not be a string list"

    testCase "Correct DynamicObj" <| fun _ ->
        let a = DynamicObj()
        let b = DynamicObj()
        a.SetProperty("a", b)
        let c = DynObj.tryGetTypedPropertyValue<DynamicObj> "a" a
        Expect.equal c (Some b) "Value should be a DynamicObj"

    testCase "Incorrect DynamicObj" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b = DynObj.tryGetTypedPropertyValue<DynamicObj> "a" a
        Expect.isNone b "Value should not be a DynamicObj"
]

let tests_setProperty = testList "setProperty" [

    testCase "Same String" <| fun _ ->
        let a = DynamicObj ()
        a |> DynObj.setProperty "aaa" 5
        let b = DynamicObj ()
        b |> DynObj.setProperty "aaa" 5
        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Different Strings" <| fun _ ->
        let a = DynamicObj ()
        a |> DynObj.setProperty "aaa" 1212
        let b = DynamicObj ()
        b |> DynObj.setProperty "aaa" 5
        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should not be equal"   

    testCase "String only on one" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()
        b |> DynObj.setProperty "aaa" 5

        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual b a "Values should not be equal (Reversed equality)"

    testCase "Same lists different keys" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()
        a |> DynObj.setProperty "quack!" [1; 2; 3]
        b |> DynObj.setProperty "quack!1" [1; 2; 3]

        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should not be equal"   
   
    testCase "Different lists" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()
        a |> DynObj.setProperty "quack!" [1; 2; 3]
        b |> DynObj.setProperty "quack!" [1; 2; 3; 4; 34]
        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should not be equal"

    testCase "Nested Same List Same String" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        let a' = DynamicObj ()
        let b' = DynamicObj ()

        a' |> DynObj.setProperty "quack!" [1; 2; 3]
        b' |> DynObj.setProperty "quack!" [1; 2; 3]

        a |> DynObj.setProperty "aaa" a'
        b |> DynObj.setProperty "aaa" b'

        Expect.equal a' b' "New Values should be equal"
        Expect.equal a b "Old Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Old Hash codes should be equal"
        Expect.equal (a'.GetHashCode()) (b'.GetHashCode()) "New Hash codes should be equal"

    testCase "Nested Same List Different Strings" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        let a' = DynamicObj ()
        let b' = DynamicObj ()
        a' |> DynObj.setProperty "quack!" [1; 2; 3]
        b' |> DynObj.setProperty "quack!" [1; 2; 3]

        a |> DynObj.setProperty "aaa" a'
        b |> DynObj.setProperty "aaa1" b'

        Expect.equal a' b' "New Values should be equal"
        Expect.notEqual a b "Old Values should not be equal"
        Expect.equal (a'.GetHashCode()) (b'.GetHashCode()) "New Hash codes should be equal"
    ]

let tests_withProperty = testList "withProperty" [

    testCase "Same String" <| fun _ ->
        let a = 
            DynamicObj () 
            |> DynObj.withProperty "aaa" 5
            |> DynObj.withProperty "aaaa" 6
        let b = 
            DynamicObj () 
            |> DynObj.withProperty "aaa" 5
            |> DynObj.withProperty "aaaa" 6
        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Different Strings" <| fun _ ->
        let a = 
            DynamicObj () 
            |> DynObj.withProperty "111" 5
            |> DynObj.withProperty "1111" 6
        let b = 
            DynamicObj () 
            |> DynObj.withProperty "aaa" 5
            |> DynObj.withProperty "aaaa" 6
        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should not be equal"   

    testCase "String different amounts" <| fun _ ->
        let a = 
            DynamicObj () 
            |> DynObj.withProperty "aaa" 5
        let b = 
            DynamicObj () 
            |> DynObj.withProperty "aaa" 5
            |> DynObj.withProperty "aaaa" 6

        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual b a "Values should not be equal (Reversed equality)"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should not be equal"   

    testCase "Same lists different keys" <| fun _ ->
        let a = 
            DynamicObj () 
            |> DynObj.withProperty "a" [1; 2; 3]
            |> DynObj.withProperty "aa" [1; 2; 3]
        let b = 
            DynamicObj () 
            |> DynObj.withProperty "aa" [1; 2; 3]
            |> DynObj.withProperty "bbb" [1; 2; 3]

        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should not be equal"   
   
    testCase "Different lists" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()
        a |> DynObj.setProperty "quack!" [1; 2; 3]
        b |> DynObj.setProperty "quack!" [1; 2; 3; 4; 34]

        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should not be equal"

    testCase "Nested Same List Same String" <| fun _ ->
        let a' = DynamicObj ()
        let b' = DynamicObj ()

        let a = DynamicObj () |> DynObj.withProperty "aaa" (a' |> DynObj.withProperty "quack!" [1; 2; 3])
        let b = DynamicObj () |> DynObj.withProperty "aaa" (b' |> DynObj.withProperty "quack!" [1; 2; 3])

        Expect.equal a' b' "New Values should be equal"
        Expect.equal a b "Old Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Old Hash codes should be equal"
        Expect.equal (a'.GetHashCode()) (b'.GetHashCode()) "New Hash codes should be equal"

    testCase "Nested Same List Different Strings" <| fun _ ->
        let a' = DynamicObj ()
        let b' = DynamicObj ()

        let a = DynamicObj () |> DynObj.withProperty "aaa" (a' |> DynObj.withProperty "quack!" [1; 2; 3])
        let b = DynamicObj () |> DynObj.withProperty "aaa1" (b' |> DynObj.withProperty "quack!" [1; 2; 3])

        Expect.equal a' b' "New Values should be equal"
        Expect.notEqual a b "Old Values should not be equal"
        Expect.equal (a'.GetHashCode()) (b'.GetHashCode()) "New Hash codes should be equal"
    ]

let tests_setOptionalProperty = testList "setOptionalProperty" [
    testCase "Some" <| fun _ ->
        let a = DynamicObj ()
        a |> DynObj.setOptionalProperty "aaa" (Some 5)
        let b = DynamicObj ()
        b |> DynObj.setProperty "aaa" 5

        Expect.equal a b "Values should be equal"
        Expect.equal (a |> DynObj.tryGetTypedPropertyValue<int> "aaa" |> Option.get) 5 "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"
    testCase "None" <| fun _ ->
        let a = DynamicObj ()
        a |> DynObj.setOptionalProperty "aaa" None
        let b = DynamicObj ()

        Expect.equal a b "Values should be equal"
        Expect.throws (fun _ -> a |> DynObj.tryGetTypedPropertyValue<int> "aaa" |> Option.get |> ignore) "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"
    testCase "Some and None" <| fun _ ->
        let a = DynamicObj ()
        a |> DynObj.setOptionalProperty "aaa" (Some 5)
        a |> DynObj.setOptionalProperty "aaaa" None
        let b = DynamicObj ()
        b |> DynObj.setProperty "aaa" 5
        b |> DynObj.setProperty "aaaa" 5

        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"
        Expect.equal (a |> DynObj.tryGetTypedPropertyValue<int> "aaa" |> Option.get) 5 "Values should be equal"
        Expect.throws (fun _ -> a |> DynObj.tryGetTypedPropertyValue<int> "aaaa" |> Option.get |> ignore) "Values should not exist"
]

let tests_withOptionalProperty = testList "withOptionalProperty" [
    testCase "Some" <| fun _ ->
        let a = DynamicObj () |> DynObj.withOptionalProperty "aaa" (Some 5)
        let b = DynamicObj () |> DynObj.withProperty "aaa" 5

        Expect.equal a b "Values should be equal"
        Expect.equal (a |> DynObj.tryGetTypedPropertyValue<int> "aaa" |> Option.get) 5 "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "None" <| fun _ ->
        let a = DynamicObj () |> DynObj.withOptionalProperty "aaa" None
        let b = DynamicObj ()

        Expect.equal a b "Values should be equal"
        Expect.throws (fun _ -> a |> DynObj.tryGetTypedPropertyValue<int> "aaa" |> Option.get |> ignore) "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"
    testCase "Some and None" <| fun _ ->
        let a = 
            DynamicObj ()
            |> DynObj.withOptionalProperty "aaa" (Some 5)
            |> DynObj.withOptionalProperty "aaaa" None
        let b = 
            DynamicObj ()
            |> DynObj.withProperty "aaa" 5
            |> DynObj.withProperty "aaaa" 5

        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"
        Expect.equal (a |> DynObj.tryGetTypedPropertyValue<int> "aaa" |> Option.get) 5 "Values should be equal"
        Expect.throws (fun _ -> a |> DynObj.tryGetTypedPropertyValue<int> "aaaa" |> Option.get |> ignore) "Values should not exist"
]

let tests_setOptionalPropertyBy = testList "setOptionalPropertyBy" [
    testCase "Some" <| fun _ ->
        let a = DynamicObj ()
        a |> DynObj.setOptionalPropertyBy "aaa" (Some 5) (fun x -> x + 1)
        let b = DynamicObj ()
        b |> DynObj.setProperty "aaa" 6

        Expect.equal a b "Values should be equal"
        Expect.equal (a |> DynObj.tryGetTypedPropertyValue<int> "aaa" |> Option.get) 6 "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"
    testCase "None" <| fun _ ->
        let a = DynamicObj ()
        a |> DynObj.setOptionalPropertyBy "aaa" None id
        let b = DynamicObj ()

        Expect.equal a b "Values should be equal"
        Expect.throws (fun _ -> a |> DynObj.tryGetTypedPropertyValue<int> "aaa" |> Option.get |> ignore) "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"
    testCase "Some and None" <| fun _ ->
        let a = DynamicObj ()
        a |> DynObj.setOptionalPropertyBy "aaa" (Some 5) (fun x -> x + 1)
        a |> DynObj.setOptionalPropertyBy "aaaa" None id
        let b = DynamicObj ()
        b |> DynObj.setProperty "aaa" 6
        b |> DynObj.setProperty "aaaa" 6

        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"
        Expect.equal (a |> DynObj.tryGetTypedPropertyValue<int> "aaa" |> Option.get) 6 "Values should be equal"
        Expect.throws (fun _ -> a |> DynObj.tryGetTypedPropertyValue<int> "aaaa" |> Option.get |> ignore) "Values should not exist"
]

let tests_withOptionalPropertyBy = testList "withOptionalPropertyBy" [
    testCase "Some" <| fun _ ->
        let a = DynamicObj () |> DynObj.withOptionalPropertyBy "aaa" (Some 5) (fun x -> x+1)
        let b = DynamicObj () |> DynObj.withProperty "aaa" 6

        Expect.equal a b "Values should be equal"
        Expect.equal (a |> DynObj.tryGetTypedPropertyValue<int> "aaa" |> Option.get) 6 "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "None" <| fun _ ->
        let a = DynamicObj () |> DynObj.withOptionalPropertyBy "aaa" None id 
        let b = DynamicObj ()

        Expect.equal a b "Values should be equal"
        Expect.throws (fun _ -> a |> DynObj.tryGetTypedPropertyValue<int> "aaa" |> Option.get |> ignore) "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"
    testCase "Some and None" <| fun _ ->
        let a = 
            DynamicObj ()
            |> DynObj.withOptionalPropertyBy "aaa" (Some 5) (fun x -> x+1)
            |> DynObj.withOptionalPropertyBy "aaaa" None id
        let b = 
            DynamicObj ()
            |> DynObj.withProperty "aaa" 6
            |> DynObj.withProperty "aaaa" 6

        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"
        Expect.equal (a |> DynObj.tryGetTypedPropertyValue<int> "aaa" |> Option.get) 6 "Values should be equal"
        Expect.throws (fun _ -> a |> DynObj.tryGetTypedPropertyValue<int> "aaaa" |> Option.get |> ignore) "Values should not exist"
]

let tests_tryGetPropertyValue = testList "tryGetPropertyValue" [

    testCase "NonExisting" <| fun _ -> 
        let a = DynamicObj()
        let b = a |> DynObj.tryGetPropertyValue "a"
        Expect.isNone b "Value should not exist"

    testCase "Correct boxed Int" <| fun _ -> 
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b = a |> DynObj.tryGetPropertyValue "a"
        Expect.equal (b) (Some (box 1)) "Value should be 1"

    testCase "Correct unboxed Int" <| fun _ -> 
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b = a |> DynObj.tryGetPropertyValue "a"
        Expect.equal (b |> Option.map unbox<int>) (Some 1) "Value should be 1"

    testCase "Correct boxed String" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", "1")
        let b = a |> DynObj.tryGetPropertyValue "a"
        Expect.equal (b) (Some (box "1")) "Value should be '1'"

    testCase "Correct unboxed String" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", "1")
        let b = a |> DynObj.tryGetPropertyValue "a"
        Expect.equal (b |> Option.map unbox<string>) (Some "1") "Value should be '1'"

    testCase "Correct boxed List" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", [1; 2; 3])
        let b = a |> DynObj.tryGetPropertyValue "a"
        Expect.equal (b) (Some (box [1; 2; 3])) "Value should be [1; 2; 3]"

    testCase "Correct unboxed List" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", [1; 2; 3])
        let b = a |> DynObj.tryGetPropertyValue "a"
        Expect.equal (b |> Option.map unbox<int list>) (Some [1; 2; 3]) "Value should be [1; 2; 3]"

    testCase "Correct boxed DynamicObj" <| fun _ ->
        let a = DynamicObj()
        let b = DynamicObj()
        a.SetProperty("a", b)
        let c = a |> DynObj.tryGetPropertyValue "a"
        Expect.equal (c) (Some (box b)) "Value should be a DynamicObj"

    testCase "Correct unboxed DynamicObj" <| fun _ ->
        let a = DynamicObj()
        let b = DynamicObj()
        a.SetProperty("a", b)
        let c = a |> DynObj.tryGetPropertyValue "a"
        Expect.equal (c |> Option.map unbox<DynamicObj>) (Some b) "Value should be a DynamicObj"

]

let tests_removeProperty = testList "removeProperty" [
  
    testCase "Remove" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        a.SetProperty("quack!", "hello")

        a |> DynObj.removeProperty "quack!" |> ignore

        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Remove Non-Existing" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        a.SetProperty("quack!", "hello")
        b.SetProperty("quack!", "hello")

        a |> DynObj.removeProperty "quecky!" |> ignore

        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Remove only on one" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        a.SetProperty("quack!", "hello")
        b.SetProperty("quack!", "hello")

        a |> DynObj.removeProperty "quack!" |> ignore

        Expect.notEqual a b "Values should be unequal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be unequal"

    testCase "Nested Remove Non-Existing" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        let a' = DynamicObj ()
        let b' = DynamicObj ()
        a'.SetProperty("quack!", [1; 2; 3])
        b'.SetProperty("quack!", [1; 2; 3])

        a.SetProperty("aaa", a')
        a |> DynObj.removeProperty "quack!" |> ignore
        b.SetProperty("aaa", b')

        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Nested Remove only on one" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        let a' = DynamicObj ()
        let b' = DynamicObj ()
        a'.SetProperty("quack!", [1; 2; 3])
        b'.SetProperty("quack!", [1; 2; 3])

        a.SetProperty("aaa", a')
        a' |> DynObj.removeProperty "quack!" |> ignore
        b.SetProperty("aaa", b')

        Expect.notEqual a b "Values should be unequal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be unequal"

    testCase "Nested Remove on both" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        let a' = DynamicObj ()
        let b' = DynamicObj ()
        a'.SetProperty("quack!", [1; 2; 3])
        b'.SetProperty("quack!", [1; 2; 3])

        a.SetProperty("aaa", a')
        a |> DynObj.removeProperty "quack!" |> ignore
        b.SetProperty("aaa", b')
        b |> DynObj.removeProperty "quack!" |> ignore

        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

]

let tests_withoutProperty = testList "withoutProperty" [
  
    testCase "Remove" <| fun _ ->
        let a = 
            DynamicObj ()
            |> DynObj.withProperty "quack!" "hello"
            |> DynObj.withoutProperty "quack!"

        let b = DynamicObj ()


        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Remove Non-Existing" <| fun _ ->
        let a = 
            DynamicObj ()
            |> DynObj.withProperty "quack!" "hello"
            |> DynObj.withoutProperty "quecky!"

        let b = DynamicObj () |> DynObj.withProperty "quack!" "hello"


        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Remove only on one" <| fun _ ->
        let a = 
            DynamicObj ()
            |> DynObj.withProperty "quack!" "hello"
            |> DynObj.withoutProperty "quack!"
        let b = DynamicObj () |> DynObj.withProperty "quack!" "hello"

        Expect.notEqual a b "Values should be unequal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be unequal"

    testCase "Nested Remove Non-Existing" <| fun _ ->
        let a = 
            DynamicObj ()
            |> DynObj.withProperty "a" [1; 2; 3]
            |> DynObj.withProperty "aaa" (DynamicObj ())
            |> DynObj.withoutProperty "quack!"

        let b =            
            DynamicObj ()
            |> DynObj.withProperty "a" [1; 2; 3]
            |> DynObj.withProperty "aaa" (DynamicObj ())

        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Nested Remove only on one" <| fun _ ->
        let a = 
            DynamicObj ()
            |> DynObj.withProperty "a" [1; 2; 3]
            |> DynObj.withProperty "aaa" (DynamicObj ())
            |> DynObj.withoutProperty "a"

        let b =            
            DynamicObj ()
            |> DynObj.withProperty "a" [1; 2; 3]
            |> DynObj.withProperty "aaa" (DynamicObj ())

        Expect.notEqual a b "Values should be unequal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be unequal"

    testCase "Nested Remove on both" <| fun _ ->
        let a = 
            DynamicObj ()
            |> DynObj.withProperty "a" [1; 2; 3]
            |> DynObj.withProperty "aaa" (DynamicObj ())
            |> DynObj.withoutProperty "a"

        let b =            
            DynamicObj ()
            |> DynObj.withProperty "a" [1; 2; 3]
            |> DynObj.withProperty "aaa" (DynamicObj ())
            |> DynObj.withoutProperty "a"

        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

]

let tests_formatString = testList "FormatString" [

    testCase "Format string 1" <| fun _ ->
        let foo = DynamicObj()
        let list = [1;2;3;4]
        foo.SetProperty("bar", list)
        let expected = $"?bar: {list}"
        Expect.equal (foo |> DynObj.format) expected "Format string 1 failed"

    testCase "Format string 2" <| fun _ ->
        let foo = DynamicObj()
        let corgi = "corgi"
        foo.SetProperty("corgi", corgi)
        let inner = DynamicObj()
        let baz = "baz"
        inner.SetProperty("bar", baz)
        foo.SetProperty("foo", inner)
        let expected = $"""?corgi: {corgi}{Environment.NewLine}?foo:{Environment.NewLine}    ?bar: {baz}"""
        Expect.equal (foo |> DynObj.format) expected "Format string 2 failed"

]

let tests_print = testList "Print" [

    testCase "Test Print For Issue 14" <| fun _ ->
        let outer = DynamicObj()
        let inner = DynamicObj()
        inner.SetProperty("Level", "Information")
        inner.SetProperty("MessageTemplate","{Method} Request at {Path}")
        outer.SetProperty("serilog", inner)

        let print =
            try 
                outer |> DynObj.print
                true
            with
                | e -> false

        Expect.isTrue print "Print failed for issue 14"
]

let main = testList "DynObj (Module)" [
    tests_ofDict
    tests_ofSeq
    tests_ofList
    tests_ofArray
    tests_combine
    tests_tryGetTypedPropertyValue
    tests_setProperty
    tests_withProperty
    tests_setOptionalProperty
    tests_withOptionalProperty
    tests_setOptionalPropertyBy
    tests_withOptionalPropertyBy
    tests_tryGetPropertyValue
    tests_withoutProperty
    tests_formatString
    tests_print
]