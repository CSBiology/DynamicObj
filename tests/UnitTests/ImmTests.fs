﻿module Tests.ImmTests

open Xunit
open DynamicObj
open DynamicObj.Operators
open System
open Newtonsoft.Json

[<Fact>]
let ``Value test 1`` () =
    let obj1 = 
        ImmutableDynamicObj.empty
        ++ ("aaa", 5)
    Assert.Equal(obj1.["aaa"], 5);

[<Fact>]
let ``Value test 2`` () =
    let obj1 = 
        ImmutableDynamicObj.empty
        ++ ("aaa", 5)
        ++ ("bbb", "quack")
    Assert.Equal(obj1.["aaa"], 5);
    Assert.Equal(obj1.["bbb"], "quack");
   
[<Fact>]
let ``Value test 3`` () =
    let obj1 =
        ImmutableDynamicObj.empty
        ++ ("aaa", 5)
        -- "aaa"
    match obj1.TryGetValue "aaa" with
    | Some(value) -> Assert.False(true, "Should return None")
    | _ -> ()


[<Fact>]
let ``No mutation test 1`` () =
    let obj1 =
        ImmutableDynamicObj ()
        |> ImmutableDynamicObj.add "aa" 5
    let obj2 = 
        ImmutableDynamicObj ()
        |> ImmutableDynamicObj.add "bb" 10

    let objBase = ImmutableDynamicObj ()
    let objA = 
        objBase
        |> ImmutableDynamicObj.add "aa" 5
    let objB = 
        objBase
        |> ImmutableDynamicObj.add "bb" 10
    Assert.Equal(obj1, objA)
    Assert.Equal(obj2, objB)

    Assert.True((obj1 = objA))
    Assert.True((obj2 = objB))

[<Fact>]
let ``No mutation test 1 with operators`` () =
    let obj1 =
        ImmutableDynamicObj ()
        ++ ("aa", 5)
    let obj2 = 
        ImmutableDynamicObj ()
        ++ ("bb", 10)

    let objBase = ImmutableDynamicObj ()
    let objA = 
        objBase
        ++ ("aa", 5)
    let objB = 
        objBase
        ++ ("bb", 10)
    Assert.Equal(obj1, objA)
    Assert.Equal(obj2, objB)

    Assert.True((obj1 = objA))
    Assert.True((obj2 = objB))

[<Fact>]
let ``Deterministic 1`` () =
    let obj1 =
        ImmutableDynamicObj ()
        ++ ("aaa", 5)
        ++ ("bbb", "ccc")

    let obj2 =
        ImmutableDynamicObj ()
        ++ ("aaa", 5)
        ++ ("bbb", "ccc")

    Assert.Equal(obj1, obj2)

[<Fact>]
let ``Determinstic 2`` () =
    let obj1 =
        ImmutableDynamicObj ()
        ++ ("aaa", 5)
        ++ ("bbb", "ccc")
        -- "aaa"

    let obj2 =
        ImmutableDynamicObj ()
        ++ ("bbb", "ccc")

    Assert.Equal(obj1, obj2)

[<Fact>]
let ``Determinstic 3`` () =
    let obj1 =
        ImmutableDynamicObj ()
        -- "aaa"
        ++ ("bbb", "ccc")

    let obj2 =
        ImmutableDynamicObj ()
        ++ ("bbb", "ccc")

    Assert.Equal(obj1, obj2)

[<Fact>]
let ``Non-equal test 1`` () =
    let obj1 =
        ImmutableDynamicObj ()

    let obj2 = 
        ImmutableDynamicObj ()
        ++ ("quack", 5)

    Assert.NotEqual(obj1, obj2)

type Quack (map) =
    inherit ImmutableDynamicObj (map)

    new() = Quack (Map.empty)


[<Fact>]
let ``Type preserved F# 1`` () =
    let obj1 =
        Quack ()
        ++ ("aaa", 5)
    Assert.IsType<Quack>(obj1)

[<Fact>]
let ``Type preserved F# 2`` () =
    let obj1 =
        Quack ()
        ++ ("aaa", 5)
        -- "aaa"
        ++ ("bbb", 5)
    Assert.IsType<Quack>(obj1)


type QuackWithField (map, field) =
    inherit ImmutableDynamicObj (map)
    let field = field
    member _.Field = field

    new() = QuackWithField (Map.empty, 5)


[<Fact>]
let ``Fields of the closest inheritor preserved 1`` () =
    let obj1 =
        QuackWithField (Map.empty, 100)
        ++ ("aaa", 5)
        -- "bbb"
    Assert.Equal(100, obj1.Field);

type FarQuackWithField (someOtherField) =
    inherit QuackWithField (Map.empty, 55)
    let otherField = someOtherField
    member _.OtherField = otherField

    new() = FarQuackWithField(3)

[<Fact>]
let ``Fields of a far inheritor preserved 1`` () =
    let obj1 =
        FarQuackWithField (1234)
        ++ ("aaa", 5)
        -- "bbb"
    Assert.Equal(1234, obj1.OtherField);
    Assert.Equal(55, obj1.Field);
    Assert.Equal(5 :> obj, obj1.["aaa"]);

[<Fact>]
let ``Format string 1`` () =

    let foo = 
        ImmutableDynamicObj()
        ++ ("bar", [1;2;3;4])

    let expected = "?bar: [1; 2; 3; ... ]"

    Assert.Equal(expected, (foo |> ImmutableDynamicObj.format))

[<Fact>]
let ``Format string 2`` () =

    // nested
    let inner = 
        ImmutableDynamicObj()
        ++ ("bar", "baz")

    let foo = 
        ImmutableDynamicObj()
        ++ ("corgi", "corgi")
        ++ ("foo", inner)

    let expected = $"""?corgi: corgi{Environment.NewLine}?foo:{Environment.NewLine}    ?bar: baz"""

    Assert.Equal(expected, (foo |> ImmutableDynamicObj.format))

[<Fact>]
let ``Json serialization`` () =
    let o =
        ImmutableDynamicObj.empty
        ++ ("aaa", 5)
        ++ ("ohno", 10)
        ++ ("quack", "tt")
        ++ ("hh", [1; 2; 3])
    let actual = JsonConvert.SerializeObject o
    Assert.Equal("{\"aaa\":5,\"hh\":[1,2,3],\"ohno\":10,\"quack\":\"tt\"}", actual)

[<Fact>]
let ``Json serialization nested`` () =
    let o2 =
        ImmutableDynamicObj.empty
        ++ ("aaa", 5)
        ++ ("ohno", 10)
        ++ ("quack", "tt")
        ++ ("hh", [1; 2; 3])
    let o =
        ImmutableDynamicObj.empty
        ++ ("aaa", 5)
        ++ ("ohno", 10)
        ++ ("quack", "tt")
        ++ ("hh", [1; 2; 3])
        ++ ("inner", o2)
    let actual = JsonConvert.SerializeObject o

    Assert.Equal("""{"aaa":5,"hh":[1,2,3],"inner":{"aaa":5,"hh":[1,2,3],"ohno":10,"quack":"tt"},"ohno":10,"quack":"tt"}""", actual)

[<Fact>]
let ``combine flat IDOs``() = 
    let target =
        ImmutableDynamicObj.empty
        ++ ("target-unique", [42])
        ++ ("will-be-overridden", "WAS_NOT_OVERRIDDEN!")    

    let source =
        ImmutableDynamicObj.empty
        ++ ("source-unique", [|42|])
        ++ ("will-be-overridden", "WAS_OVERRIDDEN =)")

    let combined = target |> ImmutableDynamicObj.combineWith source

    let expected = 
        ImmutableDynamicObj.empty
        ++ ("target-unique", [42])
        ++ ("source-unique", [|42|])
        ++ ("will-be-overridden", "WAS_OVERRIDDEN =)")

    Assert.Equal(expected, combined)

[<Fact>]
let ``combine nested IDOs``() = 
    let target =
        ImmutableDynamicObj.empty
        ++ ("target-unique", 1337)
        ++ ("will-be-overridden", -42)
        ++ ("nested-will-be-combined", (ImmutableDynamicObj.empty ++ ("inner","I Am")))
        ++ ("nested-will-be-overridden", (ImmutableDynamicObj.empty ++ ("inner","NOT_OVERRIDDEN")))
    
    let source =
        ImmutableDynamicObj.empty
        ++ ("source-unique", 69)
        ++ ("will-be-overridden", "WAS_OVERRIDDEN")
        ++ ("nested-will-be-combined", (ImmutableDynamicObj.empty ++ ("inner_combined","Complete")))
        ++ ("nested-will-be-overridden", "WAS_OVERRIDDEN")
    
    let combined =
        target
        |> ImmutableDynamicObj.combineWith source
    
    let expected = 
        ImmutableDynamicObj.empty
        ++ ("source-unique", 69)
        ++ ("target-unique", 1337)
        ++ ("will-be-overridden", "WAS_OVERRIDDEN")
        ++ ("nested-will-be-overridden", "WAS_OVERRIDDEN")
        ++ ("nested-will-be-combined", 
            ImmutableDynamicObj.empty 
            ++ ("inner","I Am")
            ++ ("inner_combined","Complete"))

    Assert.Equal(expected, combined)

type Blabla() =
    inherit ImmutableDynamicObj()
    member _.quack = 4


[<Fact>]
let ``combine inheritors`` () =
    let bla =
        Blabla ()
        ++ ("aaa", 5)
    let another =
        ImmutableDynamicObj ()
        ++ ("bbb", 4)
    let combined = bla |> ImmutableDynamicObj.combineWith another
    Assert.Equal(combined, Blabla () ++ ("aaa", 5) ++ ("bbb", 4))
    Assert.Equal(4, combined.quack)
    