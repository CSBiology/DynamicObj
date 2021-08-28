module ImmTests

open Xunit
open ImmutableDynamicObj

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
