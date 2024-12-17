module DeepCopyProperties

open Fable.Pyxpecto
open DynamicObj
open TestUtils

let tests_DeepCopyProperties = testList "DeepCopyProperties" [

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
                let original, clone = constructDeepCopiedClone<DynamicObj> originalProps
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
                let original, clone = constructDeepCopiedClone<DynamicObj> ["dyn", inner]
                inner.SetProperty("inner int", 1)
                Expect.notEqual original clone "Original and clone should not be equal after mutating DynamicObj prop on original"
                Expect.equal (original |> DynObj.getNestedPropAs<int> ["dyn";"inner int"]) 1 "Original should have mutated properties"
                Expect.equal (clone |> DynObj.getNestedPropAs<int> ["dyn";"inner int"]) 2 "Clone should have original properties"
            
            testCase "Nested DynamicObj" <| fun _ ->
                let first_level = DynamicObj() |> DynObj.withProperty "lvl1" 1
                let second_level = DynamicObj() |> DynObj.withProperty "lvl2" 2
                first_level.SetProperty("second_level", second_level)
                let original, clone = constructDeepCopiedClone<DynamicObj> ["first_level", first_level]
                second_level.SetProperty("lvl2", -1)
                Expect.notEqual original clone "Original and clone should not be equal after mutating DynamicObj prop on original"
                Expect.equal (original |> DynObj.getNestedPropAs<int> ["first_level";"second_level";"lvl2"]) -1 "Original should have mutated properties"
                Expect.equal (clone |> DynObj.getNestedPropAs<int> ["first_level";"second_level";"lvl2"]) 2 "Clone should have original properties"
            
            testCase "DynamicObj array" <| fun _ ->
                let item1 = DynamicObj() |> DynObj.withProperty "item" 1
                let item2 = DynamicObj() |> DynObj.withProperty "item" 2
                let item3 = DynamicObj() |> DynObj.withProperty "item" 3
                let arr = [|item1; item2; item3|]
                let original, clone = constructDeepCopiedClone<DynamicObj> ["arr", box arr]
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
                let original, clone = constructDeepCopiedClone<DynamicObj> ["list", box l]
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
                let original, clone = constructDeepCopiedClone<DynamicObj> ["resizeArr", box r]
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
                let original, clone = constructDeepCopiedClone<DynamicObj> ["item", box item]
                item.stat <- "mutated"
                let originalProp = original |> DynObj.getNestedPropAs<MutableClass>["item"]
                let clonedProp = clone |> DynObj.getNestedPropAs<MutableClass> ["item"]
                Expect.equal original clone "Original and clone should be equal after mutating mutable field on original"
                Expect.equal originalProp.stat "mutated" "Original property has mutated value"
                Expect.equal clonedProp.stat "mutated" "Cloned property has mutated value"
                Expect.referenceEqual originalProp clonedProp "Original and cloned property should be reference equal"
        ]
    ]
    testList "Derived class implementing ICloneable" [
        testList "SpecialCases" [
            testCase "can unbox copy as DerivedClassCloneable" <| fun _ ->
                Expect.pass (
                    let original = DerivedClassCloneable(stat = "stat", dyn = "dyn")
                    let clone = original.DeepCopyProperties() |> unbox<DerivedClassCloneable>
                    ()
                )
            testCase "copy is of type DerivedClassCloneable" <| fun _ ->
                let original = DerivedClassCloneable(stat = "stat", dyn = "dyn")
                let clone = original.DeepCopyProperties() |> unbox<DerivedClassCloneable>
                Expect.equal (clone.GetType()) typeof<DerivedClassCloneable> "Clone is of type DerivedClassCloneable"
            ptestCase "copy has NO instance prop as dynamic prop" <| fun _ ->
                let original = DerivedClassCloneable(stat = "stat", dyn = "dyn")
                let clone = original.DeepCopyProperties() |> unbox<DerivedClassCloneable>
                let clonedProps = clone.GetProperties(false) |> Seq.map (fun p -> p.Key, p.Value)
                Expect.sequenceEqual clonedProps ["dyn", "dyn"] "Clone should have no dynamic properties"
            testCase "copy has static and dynamic props of original" <| fun _ ->
                let original = DerivedClassCloneable(stat = "stat", dyn = "dyn")
                let clone = original.DeepCopyProperties() |> unbox<DerivedClassCloneable>
                Expect.equal clone original "Clone and original should be equal"
                Expect.equal (clone.stat) (original.stat) "Clone should have static prop from derived class"
                Expect.equal (clone |> DynObj.getNestedPropAs<string> ["dyn"]) (original |> DynObj.getNestedPropAs<string> ["dyn"]) "Clone should have dynamic prop from derived class"
            testCase "can use instance method on copied derived class" <| fun _ ->
                let original = DerivedClassCloneable(stat = "stat", dyn = "dyn")
                let clone = original.DeepCopyProperties() |> unbox<DerivedClassCloneable>
                Expect.pass (clone.PrintStat())
            testCase "instance method on copied derived class returns correct value" <| fun _ ->
                let original = DerivedClassCloneable(stat = "stat", dyn = "dyn")
                let clone = original.DeepCopyProperties() |> unbox<DerivedClassCloneable>
                Expect.equal (clone.FormatStat()) "stat: stat" "instance method should return correct value"
        ]
    ]
    testList "Derived class" [
        testList "SpecialCases" [

            #if !FABLE_COMPILER
            // this test is transpiled as Expect_throws(() => {} and can never fail, so let's just test it in F# for now
            testCase "Cannot unbox clone as original type" <| fun _ ->
                let original = DerivedClass(stat = "stat", dyn = "dyn")
                let clone = original.DeepCopyProperties() 
                let unboxMaybe() = clone |> unbox<DerivedClass> |> ignore
                Expect.throws unboxMaybe "Clone cannot be unboxed as DerivedClass"
            #endif

            testCase "copy has instance prop as dynamic prop" <| fun _ ->
                let original = DerivedClass(stat = "stat", dyn = "dyn")
                let clone = original.DeepCopyProperties() |> unbox<DynamicObj>
                let clonedProps = clone.GetProperties(false) |> Seq.map (fun p -> p.Key, p.Value)
                Expect.containsAll clonedProps ["stat","stat"] "Clone should have static prop from derived class as dynamic prop"
            testCase "mutable instance prop is reference equal on clone" <| fun _ ->
                let original = DerivedClass(stat = "stat", dyn = "dyn")
                let mut = MutableClass("initial")
                original.SetProperty("mutable", mut)
                let clone = original.DeepCopyProperties() |> unbox<DynamicObj>
                mut.stat <- "mutated"
                let originalProp = original |> DynObj.getNestedPropAs<MutableClass>["mutable"]
                let clonedProp = clone |> DynObj.getNestedPropAs<MutableClass> ["mutable"]
                Expect.equal originalProp clonedProp "Original and clone should be equal after mutating mutable field on original"
        ]
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

                let clone = original.DeepCopyProperties() |> unbox<DynamicObj>
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
                let inner = DynamicObj() |> DynObj.withProperty "inner int" 2
                let original = DerivedClass(stat = "stat", dyn = "dyn")
                original.SetProperty("inner", inner)
                let clone = original.DeepCopyProperties() |> unbox<DynamicObj>
                inner.SetProperty("inner int", 1)

                Expect.equal (original |> DynObj.getNestedPropAs<int> ["inner";"inner int"]) 1 "Original should have mutated properties"
                Expect.equal (clone |> DynObj.getNestedPropAs<int> ["inner";"inner int"]) 2 "Clone should have original properties"
            
            testCase "DynamicObj array" <| fun _ ->
                let item1 = DynamicObj() |> DynObj.withProperty "item" 1
                let item2 = DynamicObj() |> DynObj.withProperty "item" 2
                let item3 = DynamicObj() |> DynObj.withProperty "item" 3
                let arr = [|item1; item2; item3|]
                let original = DerivedClass(stat = "stat", dyn = "dyn")
                original.SetProperty("arr", arr)
                let clone = original.DeepCopyProperties() |> unbox<DynamicObj>
                item1.SetProperty("item", -1)
                item2.SetProperty("item", -1)
                item3.SetProperty("item", -1)
                let originalProp = original |> DynObj.getNestedPropAs<DynamicObj array> ["arr"] |> Array.map (fun dyn -> DynObj.getNestedPropAs<int> ["item"] dyn)
                let clonedProp = clone |> DynObj.getNestedPropAs<DynamicObj array> ["arr"] |> Array.map (fun dyn -> DynObj.getNestedPropAs<int> ["item"] dyn)
                Expect.sequenceEqual originalProp [|-1; -1; -1|]  "Original should have mutated properties"
                Expect.sequenceEqual clonedProp [|1; 2; 3|] "Clone should have original properties"
            
            testCase "DynamicObj list" <| fun _ ->
                let item1 = DynamicObj() |> DynObj.withProperty "item" 1
                let item2 = DynamicObj() |> DynObj.withProperty "item" 2
                let item3 = DynamicObj() |> DynObj.withProperty "item" 3
                let l = [item1; item2; item3]
                let original = DerivedClass(stat = "stat", dyn = "dyn") 
                original.SetProperty("list", l)
                let clone = original.DeepCopyProperties() |> unbox<DynamicObj>
                item1.SetProperty("item", -1)
                item2.SetProperty("item", -1)
                item3.SetProperty("item", -1)
                let originalProp = original |> DynObj.getNestedPropAs<DynamicObj list> ["list"] |> List.map (fun dyn -> DynObj.getNestedPropAs<int> ["item"] dyn)
                let clonedProp = clone |> DynObj.getNestedPropAs<DynamicObj list> ["list"] |> List.map (fun dyn -> DynObj.getNestedPropAs<int> ["item"] dyn)
                Expect.sequenceEqual originalProp [-1; -1; -1]  "Original should have mutated properties"
                Expect.sequenceEqual clonedProp [1; 2; 3] "Clone should have original properties"
            
            testCase "DynamicObj ResizeArray" <| fun _ ->
                let item1 = DynamicObj() |> DynObj.withProperty "item" 1
                let item2 = DynamicObj() |> DynObj.withProperty "item" 2
                let item3 = DynamicObj() |> DynObj.withProperty "item" 3
                let r = ResizeArray([item1; item2; item3])
                let original = DerivedClass(stat = "stat", dyn = "dyn")
                original.SetProperty("resizeArr", r)
                let clone = original.DeepCopyProperties() |> unbox<DynamicObj>
                item1.SetProperty("item", -1)
                item2.SetProperty("item", -1)
                item3.SetProperty("item", -1)
                let originalProp = original |> DynObj.getNestedPropAs<ResizeArray<DynamicObj>> ["resizeArr"] |> Seq.map (fun dyn -> DynObj.getNestedPropAs<int> ["item"] dyn) |> ResizeArray
                let clonedProp = clone |> DynObj.getNestedPropAs<ResizeArray<DynamicObj>> ["resizeArr"] |> Seq.map (fun dyn -> DynObj.getNestedPropAs<int> ["item"] dyn) |> ResizeArray
                Expect.sequenceEqual originalProp (ResizeArray[-1; -1; -1])  "Original should have mutated properties"
                Expect.sequenceEqual clonedProp (ResizeArray[1; 2; 3]) "Clone should have original properties"
        ]
        testList "Un-Cloneable dynamic properties" [
            testCase "Class with mutable fields is reference equal" <| fun _ ->
                let item = MutableClass("initial")
                let original = DerivedClass(stat = "stat", dyn = "dyn")
                original.SetProperty("item", item)
                let clone = original.DeepCopyProperties() |> unbox<DynamicObj>
                item.stat <- "mutated"
                let originalProp = original |> DynObj.getNestedPropAs<MutableClass>["item"]
                let clonedProp = clone |> DynObj.getNestedPropAs<MutableClass> ["item"]
                Expect.equal originalProp.stat "mutated" "Original property has mutated value"
                Expect.equal clonedProp.stat "mutated" "Cloned property has mutated value"
                Expect.referenceEqual originalProp clonedProp "Original and cloned property should be reference equal"
        ]
    ]
]