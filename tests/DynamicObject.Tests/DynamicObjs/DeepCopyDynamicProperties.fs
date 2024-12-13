module DynamicObj.Tests.DeepCopyDynamicProperties

open Fable.Pyxpecto
open DynamicObj
open TestUtils

let tests_DeepCopyDynamicProperties = testList "DeepCopyDynamicProperties" [

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
                let original, clone = constructDeepCopiedClone originalProps
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
                let original, clone = constructDeepCopiedClone ["dyn", inner]
                inner.SetProperty("inner int", 1)
                Expect.notEqual original clone "Original and clone should not be equal after mutating DynamicObj prop on original"
                Expect.equal (original |> DynObj.getNestedPropAs<int> ["dyn";"inner int"]) 1 "Original should have mutated properties"
                Expect.equal (clone |> DynObj.getNestedPropAs<int> ["dyn";"inner int"]) 2 "Clone should have original properties"
            
            testCase "Nested DynamicObj" <| fun _ ->
                let first_level = DynamicObj() |> DynObj.withProperty "lvl1" 1
                let second_level = DynamicObj() |> DynObj.withProperty "lvl2" 2
                first_level.SetProperty("second_level", second_level)
                let original, clone = constructDeepCopiedClone ["first_level", first_level]
                second_level.SetProperty("lvl2", -1)
                Expect.notEqual original clone "Original and clone should not be equal after mutating DynamicObj prop on original"
                Expect.equal (original |> DynObj.getNestedPropAs<int> ["first_level";"second_level";"lvl2"]) -1 "Original should have mutated properties"
                Expect.equal (clone |> DynObj.getNestedPropAs<int> ["first_level";"second_level";"lvl2"]) 2 "Clone should have original properties"
            
            testCase "DynamicObj array" <| fun _ ->
                let item1 = DynamicObj() |> DynObj.withProperty "item" 1
                let item2 = DynamicObj() |> DynObj.withProperty "item" 2
                let item3 = DynamicObj() |> DynObj.withProperty "item" 3
                let arr = [|item1; item2; item3|]
                let original, clone = constructDeepCopiedClone ["arr", box arr]
                item1.SetProperty("item", -1)
                item2.SetProperty("item", -1)
                item3.SetProperty("item", -1)
                let originalProp = original |> DynObj.getNestedPropAs<DynamicObj array> ["arr"] |> Array.map (fun dyn -> DynObj.getNestedPropAs<int> ["item"] dyn)
                let clonedProp = clone |> DynObj.getNestedPropAs<DynamicObj array> ["arr"] |> Array.map (fun dyn -> DynObj.getNestedPropAs<int> ["item"] dyn)
                Expect.notEqual original clone "Original and clone should not be equal after mutating DynamicObj prop on original"
                Expect.sequenceEqual originalProp [|-1; -1; -1|]  "Original should have mutated properties"
                Expect.sequenceEqual clonedProp [|1; 2; 3|] "Clone should have original properties"
            
            testCase "DynamicObj list" <| fun _ ->
                let item1 = DynamicObj() |> DynObj.withProperty "item" 1
                let item2 = DynamicObj() |> DynObj.withProperty "item" 2
                let item3 = DynamicObj() |> DynObj.withProperty "item" 3
                let l = [item1; item2; item3]
                let original, clone = constructDeepCopiedClone ["list", box l]
                item1.SetProperty("item", -1)
                item2.SetProperty("item", -1)
                item3.SetProperty("item", -1)
                let originalProp = original |> DynObj.getNestedPropAs<DynamicObj list> ["list"] |> List.map (fun dyn -> DynObj.getNestedPropAs<int> ["item"] dyn)
                let clonedProp = clone |> DynObj.getNestedPropAs<DynamicObj list> ["list"] |> List.map (fun dyn -> DynObj.getNestedPropAs<int> ["item"] dyn)
                Expect.notEqual original clone "Original and clone should not be equal after mutating DynamicObj prop on original"
                Expect.sequenceEqual originalProp [-1; -1; -1]  "Original should have mutated properties"
                Expect.sequenceEqual clonedProp [1; 2; 3] "Clone should have original properties"
            
            testCase "DynamicObj ResizeArray" <| fun _ ->
                let item1 = DynamicObj() |> DynObj.withProperty "item" 1
                let item2 = DynamicObj() |> DynObj.withProperty "item" 2
                let item3 = DynamicObj() |> DynObj.withProperty "item" 3
                let r = ResizeArray([item1; item2; item3])
                let original, clone = constructDeepCopiedClone ["resizeArr", box r]
                item1.SetProperty("item", -1)
                item2.SetProperty("item", -1)
                item3.SetProperty("item", -1)
                let originalProp = original |> DynObj.getNestedPropAs<ResizeArray<DynamicObj>> ["resizeArr"] |> Seq.map (fun dyn -> DynObj.getNestedPropAs<int> ["item"] dyn) |> ResizeArray
                let clonedProp = clone |> DynObj.getNestedPropAs<ResizeArray<DynamicObj>> ["resizeArr"] |> Seq.map (fun dyn -> DynObj.getNestedPropAs<int> ["item"] dyn) |> ResizeArray
                Expect.notEqual original clone "Original and clone should not be equal after mutating DynamicObj prop on original"
                Expect.sequenceEqual originalProp (ResizeArray[-1; -1; -1])  "Original should have mutated properties"
                Expect.sequenceEqual clonedProp (ResizeArray[1; 2; 3]) "Clone should have original properties"
        ]
        testList "Un-Cloneable dynamic properties" [
            testCase "Class with mutable fields is reference equal" <| fun _ ->
                let item = MutableClass("initial")
                let original, clone = constructDeepCopiedClone ["item", box item]
                item.stat <- "mutated"
                let originalProp = original |> DynObj.getNestedPropAs<MutableClass>["item"]
                let clonedProp = clone |> DynObj.getNestedPropAs<MutableClass> ["item"]
                Expect.equal original clone "Original and clone should be equal after mutating mutable field on original"
                Expect.equal originalProp.stat "mutated" "Original property has mutated value"
                Expect.equal clonedProp.stat "mutated" "Cloned property has mutated value"
                Expect.referenceEqual originalProp clonedProp "Original and cloned property should be reference equal"
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
        testList "static properties" [
            testCase "Class with mutable fields is reference equal" <| fun _ ->
                ()
        ]
    ]
    testList "Derived class implementing ICloneable" [
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
                let original = DerivedClass(stat = "stat", dyn = "dyn")
                bulkMutate originalProps original

                let clone = original.DeepCopyDynamicProperties()
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

                Expect.sequenceEqual 
                    (
                        original.GetProperties(false) |> Seq.map (fun p -> p.Key, p.Value)
                        |> Seq.sortBy fst
                    )
                    (
                        Seq.append 
                            mutatedProps
                            [("dyn", "dyn")]
                        |> Seq.sortBy fst
                    ) 
                    "Original should have mutated properties"
                Expect.sequenceEqual 
                    (
                        clone.GetProperties(false) |> Seq.map (fun p -> p.Key, p.Value)
                        |> Seq.sortBy fst
                    )
                    (
                        Seq.append 
                            originalProps
                            [("dyn", "dyn"); ("stat", "stat")]  // copy should have static prop as dynamic prop
                        |> Seq.sortBy fst
                    ) 
                    "Clone should have original and static properties"
                Expect.isTrue (original.GetType() = typeof<DerivedClass>) "Original is of type DerivedClass"
                Expect.isTrue (clone.GetType() = typeof<DynamicObj>) "Clone is of type DynamicObj"
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