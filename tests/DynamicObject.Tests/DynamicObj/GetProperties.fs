module GetProperties

open Fable.Pyxpecto
open Fable.Core
open DynamicObj
open TestUtils

#if FABLE_COMPILER
module Native =
    #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
    [<Emit("$0[$1] = $2")>]
    let setMember (o: obj) (propertyName: string) (propertyValue: obj) : unit =
        jsNative
    #endif

    #if FABLE_COMPILER_PYTHON
    [<Emit("setattr($0, $1, $2)")>]
    let setMember (o: obj) (propertyName: string) (propertyValue: obj) : unit =
        nativeOnly
    #endif
#endif

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
        Expect.sequenceEqual properties expected "Should have all properties"
    testCase "returns static instance members of derived class when wanted" <| fun _ ->
        let a = DerivedClass(stat = "stat", dyn = "dyn")
        let properties = a.GetProperties(true) |> List.ofSeq |> List.sortBy (fun kv -> kv.Key)
        let expected = 
            [
                System.Collections.Generic.KeyValuePair("dyn", box "dyn")
                System.Collections.Generic.KeyValuePair("stat", box "stat")
            ] 
            |> Seq.sortBy (fun kv -> kv.Key)
        Expect.sequenceEqual properties expected "Should have all properties"

    testCase "ignores compiler backing fields when returning dynamic properties" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("dyn", "dyn")
        #if FABLE_COMPILER
        Native.setMember a "_stat" "stat"
        #endif
        let properties = a.GetProperties(false) |> List.ofSeq
        let expected = [
            System.Collections.Generic.KeyValuePair("dyn", box "dyn")
        ]
        Expect.sequenceEqual properties expected "Should only return explicit dynamic properties"
]
