module DeepCopyResizeArrays

open System
open Fable.Pyxpecto
open DynamicObj
open Fable.Core
open TestUtils

let tests_DeepCopyResizeArrays = testList "ResizeArrays" [
    testCase "bool" <| fun _ -> 
        let arr = ResizeArray([true; false])
        let original, copy = constructDeepCopiedObj arr
        Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
        Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
        arr[0] <- false
        Expect.sequenceEqual original (ResizeArray([false; false])) "Original schould have been mutated"
        Expect.sequenceEqual copy (ResizeArray([true; false])) "Clone should not be affected by original mutation"
    testCase "byte" <| fun _ -> 
        let arr = ResizeArray([1uy; 2uy])
        let original, copy = constructDeepCopiedObj arr
        Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
        Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
        arr[0] <- 2uy
        Expect.sequenceEqual original (ResizeArray([2uy; 2uy])) "Original schould have been mutated"
        Expect.sequenceEqual copy (ResizeArray([1uy; 2uy])) "Clone should not be affected by original mutation"
    testCase "sbyte" <| fun _ -> 
        let arr = ResizeArray([1y; 2y])
        let original, copy = constructDeepCopiedObj arr
        Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
        Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
        arr[0] <- 2y
        Expect.sequenceEqual original (ResizeArray([2y; 2y])) "Original schould have been mutated"
        Expect.sequenceEqual copy (ResizeArray([1y; 2y])) "Clone should not be affected by original mutation"
    testCase "int16" <| fun _ -> 
        let arr = ResizeArray([1s; 2s])
        let original, copy = constructDeepCopiedObj arr
        Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
        Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
        arr[0] <- 2s
        Expect.sequenceEqual original (ResizeArray([2s; 2s])) "Original schould have been mutated"
        Expect.sequenceEqual copy (ResizeArray([1s; 2s])) "Clone should not be affected by original mutation"
    testCase "uint16" <| fun _ ->
        let arr = ResizeArray([1us; 2us])
        let original, copy = constructDeepCopiedObj arr
        Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
        Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
        arr[0] <- 2us
        Expect.sequenceEqual original (ResizeArray([2us; 2us])) "Original schould have been mutated"
        Expect.sequenceEqual copy (ResizeArray([1us; 2us])) "Clone should not be affected by original mutation"
    testCase "int" <| fun _ -> 
        let arr = ResizeArray([1; 2])
        let original, copy = constructDeepCopiedObj arr
        Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
        Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
        arr[0] <- 2
        Expect.sequenceEqual original (ResizeArray([2; 2])) "Original schould have been mutated"
        Expect.sequenceEqual copy (ResizeArray([1; 2])) "Clone should not be affected by original mutation"
    testCase "uint" <| fun _ -> 
        let arr = ResizeArray([1u; 2u])
        let original, copy = constructDeepCopiedObj arr
        Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
        Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
        arr[0] <- 2u
        Expect.sequenceEqual original (ResizeArray([2u; 2u])) "Original schould have been mutated"
        Expect.sequenceEqual copy (ResizeArray([1u; 2u])) "Clone should not be affected by original mutation"
    testCase "int64" <| fun _ -> 
        let arr = ResizeArray([1L; 2L])
        let original, copy = constructDeepCopiedObj arr
        Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
        Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
        arr[0] <- 2L
        Expect.sequenceEqual original (ResizeArray([2L; 2L])) "Original schould have been mutated"
        Expect.sequenceEqual copy (ResizeArray([1L; 2L])) "Clone should not be affected by original mutation"
    testCase "uint64" <| fun _ -> 
        let arr = ResizeArray([1uL; 2uL])
        let original, copy = constructDeepCopiedObj arr
        Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
        Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
        arr[0] <- 2uL
        Expect.sequenceEqual original (ResizeArray([2uL; 2uL])) "Original schould have been mutated"
        Expect.sequenceEqual copy (ResizeArray([1uL; 2uL])) "Clone should not be affected by original mutation"
    testCase "float" <| fun _ -> 
        let arr = ResizeArray([1.0; 2.0])
        let original, copy = constructDeepCopiedObj arr
        Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
        Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
        arr[0] <- 2.0
        Expect.sequenceEqual original (ResizeArray([2.0; 2.0])) "Original schould have been mutated"
        Expect.sequenceEqual copy (ResizeArray([1.0; 2.0])) "Clone should not be affected by original mutation"
    testCase "float32" <| fun _ -> 
        let arr = ResizeArray([1.0f; 2.0f])
        let original, copy = constructDeepCopiedObj arr
        Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
        Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
        arr[0] <- 2.0f
        Expect.sequenceEqual original (ResizeArray([2.0f; 2.0f])) "Original schould have been mutated"
        Expect.sequenceEqual copy (ResizeArray([1.0f; 2.0f])) "Clone should not be affected by original mutation"
    testCase "char" <| fun _ -> 
        let arr = ResizeArray(['A'; 'B'])
        let original, copy = constructDeepCopiedObj arr
        Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
        Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
        arr[0] <- 'B'
        Expect.sequenceEqual original (ResizeArray(['B'; 'B'])) "Original schould have been mutated"
        Expect.sequenceEqual copy (ResizeArray(['A'; 'B'])) "Clone should not be affected by original mutation"
    testCase "string" <| fun _ -> 
        let arr = ResizeArray(["Hi"; "Bye"])
        let original, copy = constructDeepCopiedObj arr
        Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
        Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
        arr[0] <- "Bye"
        Expect.sequenceEqual original (ResizeArray(["Bye"; "Bye"])) "Original schould have been mutated"
        Expect.sequenceEqual copy (ResizeArray(["Hi"; "Bye"])) "Clone should not be affected by original mutation"
    testCase "unit" <| fun _ -> 
        let arr = ResizeArray([(); ()])
        let original, copy = constructDeepCopiedObj arr
        Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
        Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
        // transpilation fun
        let arr2 = ResizeArray([()])
        arr.Add(arr2[0])
        Expect.sequenceEqual original (ResizeArray([(); (); ()])) "Original schould have been mutated"
        Expect.sequenceEqual copy (ResizeArray([(); ()])) "Clone should not be affected by original mutation"
        
    // some cases are not transpilable

    #if !FABLE_COMPILER_PYTHON
    testCase "decimal" <| fun _ -> 
        let arr = ResizeArray([1.0M; 2.0M])
        let original, copy = constructDeepCopiedObj arr
        Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
        Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
        arr[0] <- 2.0M
        Expect.sequenceEqual original (ResizeArray([2.0M; 2.0M])) "Original schould have been mutated"
        Expect.sequenceEqual copy (ResizeArray([1.0M; 2.0M])) "Clone should not be affected by original mutation"
    #endif

    #if !FABLE_COMPILER
    testCase "nativeint" <| fun _ -> 
        let original, copy = constructDeepCopiedObj (ResizeArray([System.IntPtr(1); System.IntPtr(2)]))
        Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
        Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
    testCase "unativeint" <| fun _ -> 
        let original, copy = constructDeepCopiedObj (ResizeArray([System.UIntPtr(1u); System.UIntPtr(2u)]))
        Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
        Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
    #endif
]