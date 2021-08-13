module Tests.EqualityHashcode

open System
open Xunit
open DynamicObj

[<Fact>]
let ``Equality test 1`` () =
    let a = DynamicObj ()
    a.SetValue("aaa", 5)
    let b = DynamicObj ()
    b.SetValue("aaa", 5)
    Assert.Equal(a, b)
    Assert.Equal(a.GetHashCode(), b.GetHashCode())

[<Fact>]
let ``Equality test 2`` () =
    let a = DynamicObj ()
    a.SetValue("aaa", 1212)
    let b = DynamicObj ()
    b.SetValue("aaa", 5)
    Assert.NotEqual(a, b)


[<Fact>]
let ``Equality test 3`` () =
    let a = DynamicObj ()
    let b = DynamicObj ()

    let a' = DynamicObj ()
    let b' = DynamicObj ()
    a'.SetValue("quack!", [1; 2; 3])
    b'.SetValue("quack!", [1; 2; 3])

    a.SetValue("aaa", a')
    b.SetValue("aaa", b')
    Assert.Equal(a', b')
    Assert.Equal(a, b)
    Assert.Equal(a.GetHashCode(), b.GetHashCode())
    Assert.Equal(a'.GetHashCode(), b'.GetHashCode())

[<Fact>]
let ``Equality test 4`` () =
    let a = DynamicObj ()
    let b = DynamicObj ()

    let a' = DynamicObj ()
    let b' = DynamicObj ()
    a'.SetValue("quack!", [1; 2; 3])
    b'.SetValue("quack!", [1; 2; 3])

    a.SetValue("aaa", a')
    b.SetValue("aaa1", b')
    Assert.Equal(a', b')
    Assert.NotEqual(a, b)
    Assert.Equal(a'.GetHashCode(), b'.GetHashCode())


// different objects do NOT have to have different hash
// codes, so here we rely on our luckiness.

[<Fact>]
let ``Hashcode inequality 1`` () =
    let a' = DynamicObj ()
    let b' = DynamicObj ()
    a'.SetValue("quack!", [1; 2; 3])
    b'.SetValue("quack!", [1; 2; 3; 4; 34])
    Assert.NotEqual(a'.GetHashCode(), b'.GetHashCode())


[<Fact>]
let ``Hashcode inequality 2`` () =
    let a' = DynamicObj ()
    let b' = DynamicObj ()
    a'.SetValue("quack!", [1; 2; 3])
    b'.SetValue("quack!1", [1; 2; 3])
    Assert.NotEqual(a'.GetHashCode(), b'.GetHashCode())
