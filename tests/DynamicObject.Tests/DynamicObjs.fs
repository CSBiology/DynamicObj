module DynamicObj.Tests

open System
open Fable.Pyxpecto
open DynamicObj
open TestUtils

let tests_TryGetPropertyValue = testList "TryGetPropertyValue" [
    testCase "NonExisting" <| fun _ -> 
        let a = DynamicObj()
        let b = a.TryGetPropertyValue "a"
        Expect.isNone b "Value should not exist"

    testCase "Correct boxed Int" <| fun _ -> 
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b = a.TryGetPropertyValue "a"
        Expect.equal (b) (Some (box 1)) "Value should be 1"

    testCase "Correct unboxed Int" <| fun _ -> 
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b = a.TryGetPropertyValue "a"
        Expect.equal (b |> Option.map unbox<int>) (Some 1) "Value should be 1"

    testCase "Correct boxed String" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", "1")
        let b = a.TryGetPropertyValue "a"
        Expect.equal (b) (Some (box "1")) "Value should be '1'"

    testCase "Correct unboxed String" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", "1")
        let b = a.TryGetPropertyValue "a"
        Expect.equal (b |> Option.map unbox<string>) (Some "1") "Value should be '1'"

    testCase "Correct boxed List" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", [1; 2; 3])
        let b = a.TryGetPropertyValue "a"
        Expect.equal (b) (Some (box [1; 2; 3])) "Value should be [1; 2; 3]"

    testCase "Correct unboxed List" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", [1; 2; 3])
        let b = a.TryGetPropertyValue "a"
        Expect.equal (b |> Option.map unbox<int list>) (Some [1; 2; 3]) "Value should be [1; 2; 3]"

    testCase "Correct boxed DynamicObj" <| fun _ ->
        let a = DynamicObj()
        let b = DynamicObj()
        a.SetProperty("a", b)
        let c = a.TryGetPropertyValue "a"
        Expect.equal (c) (Some (box b)) "Value should be a DynamicObj"

    testCase "Correct unboxed DynamicObj" <| fun _ ->
        let a = DynamicObj()
        let b = DynamicObj()
        a.SetProperty("a", b)
        let c = a.TryGetPropertyValue "a"
        Expect.equal (c |> Option.map unbox<DynamicObj>) (Some b) "Value should be a DynamicObj"

]

let tests_GetPropertyValue = testList "GetPropertyValue" [
    testCase "NonExisting" <| fun _ -> 
        let a = DynamicObj()
        Expect.throws (fun () -> a.GetPropertyValue("b") |> ignore) "Value should not exist"

    testCase "Correct boxed Int" <| fun _ -> 
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b = a.GetPropertyValue "a"
        Expect.equal (b) (box 1) "Value should be 1"

    testCase "Correct unboxed Int" <| fun _ -> 
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b = a.GetPropertyValue "a"
        Expect.equal (b |> unbox<int>) (1) "Value should be 1"

    testCase "Correct boxed String" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", "1")
        let b = a.GetPropertyValue "a"
        Expect.equal (b) (box "1") "Value should be '1'"

    testCase "Correct unboxed String" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", "1")
        let b = a.GetPropertyValue "a"
        Expect.equal (b |> unbox<string>) ("1") "Value should be '1'"

    testCase "Correct boxed List" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", [1; 2; 3])
        let b = a.GetPropertyValue "a"
        Expect.equal (b) (box [1; 2; 3]) "Value should be [1; 2; 3]"

    testCase "Correct unboxed List" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", [1; 2; 3])
        let b = a.GetPropertyValue "a"
        Expect.equal (b |> unbox<int list>) ([1; 2; 3]) "Value should be [1; 2; 3]"

    testCase "Correct boxed DynamicObj" <| fun _ ->
        let a = DynamicObj()
        let b = DynamicObj()
        a.SetProperty("a", b)
        let c = a.GetPropertyValue "a"
        Expect.equal (c) (box b) "Value should be a DynamicObj"

    testCase "Correct unboxed DynamicObj" <| fun _ ->
        let a = DynamicObj()
        let b = DynamicObj()
        a.SetProperty("a", b)
        let c = a.GetPropertyValue "a"
        Expect.equal (c |> unbox<DynamicObj>) (b) "Value should be a DynamicObj"

]

#if !FABLE_COMPILER
// instance method TryGetTypedPropertyValue is not Fable-compatible
let tests_TryGetTypedPropertyValue = testList "TryGetTypedPropertyValue" [
    
    testCase "typeof" <| fun _ -> 
        let a = typeof<int>
        Expect.equal a.Name "Int32" "Type should be Int32"

    testCase "NonExisting" <| fun _ -> 
        let a = DynamicObj()
        let b = a.TryGetTypedPropertyValue<int> "a"
        Expect.isNone b "Value should not exist"

    testCase "Correct Int" <| fun _ -> 
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b = a.TryGetTypedPropertyValue<int> "a"
        Expect.equal b (Some 1) "Value should be 1"

    testCase "Incorrect Int" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", "1")
        let b = a.TryGetTypedPropertyValue<int> "a"
        Expect.isNone b "Value should not be an int"

    testCase "Correct String" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", "1")
        let b = a.TryGetTypedPropertyValue<string> "a"
        Expect.equal b (Some "1") "Value should be '1'"

    testCase "Incorrect String" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b = a.TryGetTypedPropertyValue<string> "a"
        Expect.isNone b "Value should not be a string"

    testCase "Correct List" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", [1; 2; 3])
        let b = a.TryGetTypedPropertyValue<int list> "a"
        Expect.equal b (Some [1; 2; 3]) "Value should be [1; 2; 3]"

    testCase "Incorrect List" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", [1; 2; 3])
        let b = a.TryGetTypedPropertyValue<string list> "a"
        Expect.isNone b "Value should not be a string list"

    testCase "Correct DynamicObj" <| fun _ ->
        let a = DynamicObj()
        let b = DynamicObj()
        a.SetProperty("a", b)
        let c = a.TryGetTypedPropertyValue<DynamicObj> "a"
        Expect.equal c (Some b) "Value should be a DynamicObj"

    testCase "Incorrect DynamicObj" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b = a.TryGetTypedPropertyValue<DynamicObj> "a"
        Expect.isNone b "Value should not be a DynamicObj"
]
#endif

let tests_TryGetStaticPropertyHelper = testList "TryGetStaticPropertyHelper" [
    testCase "NonExisting" <| fun _ -> 
        let a = DynamicObj()
        let b = a.TryGetStaticPropertyHelper("a")
        Expect.isNone b "Value should not exist"

    testCase "Properties dictionary is static property" <| fun _ -> 
        let a = DynamicObj()
        let b =  Expect.wantSome (a.TryGetStaticPropertyHelper("Properties")) "Value should exist"
        Expect.isTrue b.IsStatic "Properties should be static"
        Expect.isFalse b.IsDynamic "Properties should not be dynamic"
        Expect.isTrue b.IsMutable "Properties should be mutable"
        Expect.isFalse b.IsImmutable "Properties should not be immutable"

    testCase "dynamic property not retrieved as static" <| fun _ -> 
        let a = DynamicObj()
        a.SetProperty("a", 1)
        Expect.isNone (a.TryGetStaticPropertyHelper("a")) "dynamic property should not be retrieved via TryGetStaticPropertyInfo"
]

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

    testCase "Existing static property" <| fun _ -> 
        let a = DynamicObj()
        let b =  Expect.wantSome (a.TryGetPropertyHelper("Properties")) "Value should exist"
        Expect.isTrue b.IsStatic "Properties should be static"
        Expect.isFalse b.IsDynamic "Properties should not be dynamic"
        Expect.isTrue b.IsMutable "Properties should be mutable"
        Expect.isFalse b.IsImmutable "Properties should not be immutable"
]

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

let tests_RemoveProperty = testList "RemoveProperty" [
  
    //TODO: static property removal!

    testCase "Remove" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        a.SetProperty("quack!", "hello")

        a.RemoveProperty "quack!" |> ignore

        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Remove Non-Existing" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        a.SetProperty("quack!", "hello")
        b.SetProperty("quack!", "hello")

        a.RemoveProperty "quecky!" |> ignore

        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Remove only on one" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        a.SetProperty("quack!", "hello")
        b.SetProperty("quack!", "hello")

        a.RemoveProperty "quack!" |> ignore

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
        a.RemoveProperty "quack!" |> ignore
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
        a'.RemoveProperty "quack!" |> ignore
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
        a.RemoveProperty "quack!" |> ignore
        b.SetProperty("aaa", b')
        b.RemoveProperty "quack!" |> ignore

        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

]

let tests_GetPropertyHelpers = testList "GetPropertyHelpers" [
    testCase "GetPropertyHelpers" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", 1)
        a.SetProperty("b", 2)
        let properties = a.GetPropertyHelpers(true)
        let names = properties |> Seq.map (fun p -> p.Name)
        Expect.equal (Seq.toList names) ["a"; "b"] "Should have all properties"
]

let tests_GetProperties = testList "GetProperties" [
    testCase "GetProperties" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", 1)
        a.SetProperty("b", 2)
        let properties = a.GetProperties(true) |> List.ofSeq
        let expected = [
            System.Collections.Generic.KeyValuePair("a", box 1)
            System.Collections.Generic.KeyValuePair("b", box 2)
        ]
        Expect.equal properties expected "Should have all properties"
]

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

type DerivedClass(stat: string, dyn: string) as this =
    inherit DynamicObj()
    do
        this.SetProperty("dyn", dyn)
    member this.Stat = stat

type DerivedClassCloneable(stat: string, dyn: string) as this =
    inherit DynamicObj()
    do
        this.SetProperty("dyn", dyn)
    member this.Stat = stat
    interface ICloneable with
        member this.Clone() =
            let dyn = this.GetPropertyValue("dyn") |> unbox<string>
            DerivedClassCloneable(stat, dyn)

let tests_DeepCopyDynamicProperties = testList "DeepCopyDynamicProperties" [

    let constructClone (props: seq<string*obj>) =
        let original = DynamicObj()
        props
        |> Seq.iter (fun (propertyName, propertyValue) -> original.SetProperty(propertyName, propertyValue))
        let clone = original.DeepCopyDynamicProperties()
        original, clone

    let bulkMutate (props: seq<string*obj>) (dyn: DynamicObj) =
        props |> Seq.iter (fun (propertyName, propertyValue) -> dyn.SetProperty(propertyName, propertyValue))


    testList "DynamicObj" [
        testList "Cloneable dynamic properties" [
            testCase "primitives" <| fun _ ->
                let originalProps = [ 
                    "int", box 1
                    "float", box 1.0
                    "bool", box true
                    "string", box "hello"
                    "char", box 'a'
                    "byte", box (byte 1)
                    "sbyte", box (sbyte -1)
                    "int16", box (int16 -1)
                    "uint16", box (uint16 1)
                    "int32", box (int32 -1)
                    "uint32", box (uint32 1u)
                    "int64", box (int64 -1L)
                    "uint64", box (uint64 1UL)
                    "single", box (single 1.0f)
                    "decimal", box (decimal 1M) 
                ]
                let original, clone = constructClone originalProps
                let mutatedProps = [ 
                    "int", box 2
                    "float", box 2.0
                    "bool", box false
                    "string", box "bye"
                    "char", box 'b'
                    "byte", box (byte 2)
                    "sbyte", box (sbyte -2)
                    "int16", box (int16 -2)
                    "uint16", box (uint16 2)
                    "int32", box (int32 -2)
                    "uint32", box (uint32 2u)
                    "int64", box (int64 -2L)
                    "uint64", box (uint64 2UL)
                    "single", box (single 2.0f)
                    "decimal", box (decimal 2M)
                ]
                bulkMutate mutatedProps original
                Expect.notEqual original clone "Original and clone should not be equal after mutating primitive props on original"
                Expect.sequenceEqual (original.GetProperties(true) |> Seq.map (fun p -> p.Key, p.Value)) mutatedProps "Original should have mutated properties"
                Expect.sequenceEqual (clone.GetProperties(true) |> Seq.map (fun p -> p.Key, p.Value)) originalProps "Clone should have original properties"
            testCase "DynamicObj" <| fun _ ->
                let inner = DynamicObj() |> DynObj.withProperty "inner int" 2
                let original, clone = constructClone ["dyn", inner]
                inner.SetProperty("inner int", 1)
                Expect.notEqual original clone "Original and clone should not be equal after mutating DynamicObj prop on original"
                Expect.equal (original |> DynObj.getNestedPropAs<int> ["dyn";"inner int"]) 1 "Original should have mutated properties"
                Expect.equal (clone |> DynObj.getNestedPropAs<int> ["dyn";"inner int"]) 2 "Clone should have original properties"
            testCase "Nested DynamicObj" <| fun _ ->
                let first_level = DynamicObj() |> DynObj.withProperty "lvl1" 1
                let second_level = DynamicObj() |> DynObj.withProperty "lvl2" 2
                first_level.SetProperty("second_level", second_level)
                let original, clone = constructClone ["first_level", first_level]
                second_level.SetProperty("lvl2", -1)
                Expect.notEqual original clone "Original and clone should not be equal after mutating DynamicObj prop on original"
                Expect.equal (original |> DynObj.getNestedPropAs<int> ["first_level";"second_level";"lvl2"]) -1 "Original should have mutated properties"
                Expect.equal (clone |> DynObj.getNestedPropAs<int> ["first_level";"second_level";"lvl2"]) 2 "Clone should have original properties"
            testCase "DynamicObj array" <| fun _ ->
                let item1 = DynamicObj() |> DynObj.withProperty "item" 1
                let item2 = DynamicObj() |> DynObj.withProperty "item" 2
                let item3 = DynamicObj() |> DynObj.withProperty "item" 3
                let arr = [|item1; item2; item3|]
                let original, clone = constructClone ["arr", box arr]
                item1.SetProperty("item", -1)
                item2.SetProperty("item", -1)
                item3.SetProperty("item", -1)
                let originalProp = original |> DynObj.getNestedPropAs<DynamicObj array> ["arr"] |> Array.map (fun dyn -> DynObj.getNestedPropAs<int> ["item"] dyn)
                let clonedProp = clone |> DynObj.getNestedPropAs<DynamicObj array> ["arr"] |> Array.map (fun dyn -> DynObj.getNestedPropAs<int> ["item"] dyn)
                Expect.notEqual original clone "Original and clone should not be equal after mutating DynamicObj prop on original"
                Expect.sequenceEqual originalProp [|-1; -1; -1|]  "Original should have mutated properties"
                Expect.equal clonedProp [|1; 2; 3|] "Clone should have original properties"
            testCase "DynamicObj list" <| fun _ ->
                ()
            testCase "DynamicObj ResizeArray" <| fun _ ->
                ()
        ]
        testList "Un-Cloneable dynamic properties" [
            testCase "Class with mutable fields is reference equal" <| fun _ ->
                ()
        ]
    ]
    testList "Derived class" [
        testList "Cloneable dynamic properties" [
            testCase "primitives" <| fun _ ->
                ()
            testCase "DynamicObj" <| fun _ ->
                ()
            testCase "DynamicObj array" <| fun _ ->
                ()
            testCase "DynamicObj list" <| fun _ ->
                ()
            testCase "DynamicObj ResizeArray" <| fun _ ->
                ()
        ]
        testList "Un-Cloneable dynamic properties" [
            testCase "Class with mutable fields is reference equal" <| fun _ ->
                ()
        ]
    ]
]

let tests_Equals = testList "Equals" [
    testCase "Same Object" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("b", 2)
        Expect.isTrue (a.Equals(a)) "Values should be equal"

    testCase "Different Equal Objects" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("b", 2)
        let a2 = DynamicObj()
        a2.SetProperty("b", 2)
        Expect.isTrue (a.Equals(a2)) "Values should be equal"

    testCase "Different Unequal Objects" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("b", 2)
        let a2 = DynamicObj()
        a2.SetProperty("b", 3)
        Expect.isFalse (a.Equals(a2)) "Values should not be equal"

    testCase "nested DynamicObjs" <| fun _ ->
        let a = DynamicObj()
        let b = DynamicObj()
        b.SetProperty("c", 2)
        a.SetProperty("b", b)
        let a2 = DynamicObj()
        let b2 = DynamicObj()
        b2.SetProperty("c", 2)
        a2.SetProperty("b", b2)
        Expect.isTrue (a.Equals(a2)) "Values should be equal"

]

let tests_GetHashCode = testList "GetHashCode" [
    testCase "Same Object" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("b", 2)
        Expect.equal (a.GetHashCode()) (a.GetHashCode()) "Values should be equal"

    testCase "Different Equal Objects" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("b", 2)
        let a2 = DynamicObj()
        a2.SetProperty("b", 2)
        Expect.equal (a.GetHashCode()) (a2.GetHashCode()) "Values should be equal"

    testCase "Different Unequal Objects" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("b", 2)
        let a2 = DynamicObj()
        a.SetProperty("b", 3)
        Expect.notEqual (a.GetHashCode()) (a2.GetHashCode()) "Values should not be equal"

    testCase "nested DynamicObjs" <| fun _ ->
        let a = DynamicObj()
        let b = DynamicObj()
        b.SetProperty("c", 2)
        a.SetProperty("b", b)
        let a2 = DynamicObj()
        let b2 = DynamicObj()
        b2.SetProperty("c", 2)
        a2.SetProperty("b", b2)
        Expect.equal (a.GetHashCode()) (a2.GetHashCode()) "Values should be equal"

    testCase "null Value same key" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("b", null)
        let b = DynamicObj()
        b.SetProperty("b", null)
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Values should be equal"

    testCase "null Value different key" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("b", null)
        let b = DynamicObj()
        a.SetProperty("c", null)
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Values should not be equal"

]

let main = testList "DynamicObj (Class)" [
    tests_TryGetPropertyValue
    tests_GetPropertyValue

    #if !FABLE_COMPILER
    // instance method TryGetTypedValue is not Fable-compatible
    tests_TryGetTypedPropertyValue
    #endif

    tests_TryGetStaticPropertyHelper
    tests_TryGetDynamicPropertyHelper
    tests_TryGetPropertyHelper
    tests_SetProperty
    tests_RemoveProperty
    tests_GetPropertyHelpers
    tests_GetProperties
    tests_ShallowCopyDynamicPropertiesTo
    tests_ShallowCopyDynamicProperties
    tests_DeepCopyDynamicProperties
    tests_Equals
    tests_GetHashCode
]