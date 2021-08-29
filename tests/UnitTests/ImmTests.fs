module Tests.ImmTests

open Xunit
open DynamicObj

[<Fact>]
let ``No mutation test 1`` () =
    let obj1 =
        ImmutableDynamicObj ()
        |> ImmutableDynamicObj.With "aa" 5
    let obj2 = 
        ImmutableDynamicObj ()
        |> ImmutableDynamicObj.With "bb" 10

    let objBase = ImmutableDynamicObj ()
    let objA = 
        objBase
        |> ImmutableDynamicObj.With "aa" 5
    let objB = 
        objBase
        |> ImmutableDynamicObj.With "bb" 10
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

[<Fact>]
let ``Type preserved CLS Compliant 1`` () =
    let obj1 =
        Quack ()
        |> Quack.WithCLSCompliant "aaa" 5
    Assert.IsType<Quack>(obj1)

[<Fact>]
let ``Type preserved CLS Compliant 2`` () =
    let obj1 =
        Quack ()
        |> Quack.WithCLSCompliant "aaa" 5
        |> Quack.WithoutCLSCompliant "aaa"
        |> Quack.WithCLSCompliant "bbb" 5
    Assert.IsType<Quack>(obj1)

[<Fact>]
let ``Type preserved CLS Compliant and F# the same thing 1`` () =
    let obj1 =
        Quack ()
        |> Quack.WithCLSCompliant "aaa" 5
        |> Quack.WithCLSCompliant "bbb" 5

    let obj2 =
        Quack ()
        ++ ("aaa", 5)
        ++ ("bbb", 5)

    Assert.Equal(obj1, obj2)

[<Fact>]
let ``Type preserved CLS Compliant and F# the same thing 2`` () =
    let obj1 =
        Quack ()
        |> Quack.WithCLSCompliant "aaa" 5
        |> Quack.WithCLSCompliant "bbb" 5
        |> Quack.WithoutCLSCompliant "aaa"

    let obj2 =
        Quack ()
        ++ ("aaa", 5)
        ++ ("bbb", 5)
        -- "aaa"

    Assert.Equal(obj1, obj2)
