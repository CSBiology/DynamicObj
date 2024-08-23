module Tests.OperatorsTests

open Xunit
open DynamicObj
open DynamicObj.Operators

[<Fact>]
let ``Test ++? 1`` () =
    let obj1 = 
        ImmutableDynamicObj.empty
        ++? ("aaa", Some(5))
    let expected =
        ImmutableDynamicObj.empty
        ++ ("aaa", 5)
    Assert.Equal(expected, obj1)

[<Fact>]
let ``Test ++? 2`` () =
    let obj1 = 
        ImmutableDynamicObj.empty
        ++? ("aaa", None)
    let expected =
        ImmutableDynamicObj.empty
    Assert.Equal(expected, obj1)

[<Fact>]
let ``Test ++?? 1`` () =
    let obj1 = 
        ImmutableDynamicObj.empty
        ++?? ("aaa", Some(5), fun c -> c * 10)
    let expected =
        ImmutableDynamicObj.empty
        ++ ("aaa", 50)
    Assert.Equal(expected, obj1)

