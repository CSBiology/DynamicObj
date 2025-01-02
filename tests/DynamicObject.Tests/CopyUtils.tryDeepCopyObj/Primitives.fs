module DeepCopyPrimitives

open System
open Fable.Pyxpecto
open DynamicObj
open Fable.Core
open TestUtils

let tests_DeepCopyPrimitives = testList "Primitives" [
    testCase "bool" <| fun _ -> 
        let original, copy = constructDeepCopiedObj true
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "byte" <| fun _ -> 
        let original, copy = constructDeepCopiedObj 1uy
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "sbyte" <| fun _ -> 
        let original, copy = constructDeepCopiedObj 1y
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "int16" <| fun _ -> 
        let original, copy = constructDeepCopiedObj 1s
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "uint16" <| fun _ -> 
        let original, copy = constructDeepCopiedObj 1us
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "int" <| fun _ -> 
        let original, copy = constructDeepCopiedObj 1
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "uint" <| fun _ -> 
        let original, copy = constructDeepCopiedObj 1u
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "int64" <| fun _ -> 
        let original, copy = constructDeepCopiedObj 1L
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "uint64" <| fun _ -> 
        let original, copy = constructDeepCopiedObj 1uL
        Expect.equal copy original "Expected values of copy and original to be equal"
    #if !FABLE_COMPILER
    testCase "nativeint" <| fun _ -> 
        let original, copy = constructDeepCopiedObj (System.IntPtr(1))
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "unativeint" <| fun _ -> 
        let original, copy = constructDeepCopiedObj (System.UIntPtr(1u))
        Expect.equal copy original "Expected values of copy and original to be equal"
    #endif
    testCase "float" <| fun _ -> 
        let original, copy = constructDeepCopiedObj 1.0
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "float32" <| fun _ -> 
        let original, copy = constructDeepCopiedObj 1.0f
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "char" <| fun _ -> 
        let original, copy = constructDeepCopiedObj 'A'
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "string" <| fun _ -> 
        let original, copy = constructDeepCopiedObj "Hi"
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "unit" <| fun _ -> 
        let original, copy = constructDeepCopiedObj ()
        Expect.equal copy original "Expected values of copy and original to be equal"
]