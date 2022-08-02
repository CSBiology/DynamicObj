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


[<Fact>]
let ``Equality test 5`` () =
    let a = DynamicObj ()
    a.SetValue("bvbb", 5)
    let b = DynamicObj ()
    b.SetValue("aaa", 5)

    a.SetValue("aaa", 5)
    a.Remove "bvbb" |> ignore

    Assert.Equal(a, b)
    Assert.Equal(a.GetHashCode(), b.GetHashCode())
    
[<Fact>]
let ``Equality test 6`` () =
    //nesting
    let a = DynamicObj ()
    let b = DynamicObj ()
    b.SetValue("a", 5)
    a.SetValue("inner",b)
    let c = DynamicObj ()
    let d = DynamicObj ()
    d.SetValue("a", 5)
    c.SetValue("inner",d)

    Assert.Equal(a, c)
    Assert.Equal(a.GetHashCode(), c.GetHashCode())

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

[<Fact>]
let ``Format string 1`` () =

    let foo = DynamicObj()
    foo?bar <- [1;2;3;4]

    let expected = "?bar: [1; 2; 3; ... ]"

    Assert.Equal(expected, (foo |> DynObj.format))

[<Fact>]
let ``Format string 2`` () =

    // nested
    let foo = DynamicObj()
    foo?corgi <- "corgi"
    let inner = DynamicObj()
    inner?bar <- "baz"
    foo?foo <- inner

    let expected = $"""?corgi: corgi{Environment.NewLine}?foo:{Environment.NewLine}    ?bar: baz"""

    Assert.Equal(expected, (foo |> DynObj.format))



[<Fact>]
let ``combine flat DOs``() = 
    let target = DynamicObj()

    target.SetValue("target-unique", [42])
    target.SetValue("will-be-overridden", "WAS_NOT_OVERRIDDEN!")

    let source = DynamicObj()

    source.SetValue("source-unique", [42; 32])
    source.SetValue("will-be-overridden", "WAS_OVERRIDDEN =)")

    let combined = DynObj.combine target source

    let expected = DynamicObj()

    expected.SetValue("target-unique", [42])
    expected.SetValue("source-unique", [42; 32])
    expected.SetValue("will-be-overridden", "WAS_OVERRIDDEN =)")

    Assert.Equal(expected, combined)

[<Fact>]
let ``combine nested DOs``() = 

    let target = DynamicObj()

    target.SetValue("target-unique", 1337)
    target.SetValue("will-be-overridden", -42)
    let something2BeCombined = DynamicObj()
    something2BeCombined.SetValue("inner","I Am")
    let something2BeOverriden = DynamicObj()
    something2BeOverriden.SetValue("inner","NOT_OVERRIDDEN")
    target.SetValue("nested-will-be-combined", something2BeCombined)
    target.SetValue("nested-will-be-overridden", something2BeOverriden)
    
    let source = DynamicObj()

    source.SetValue("source-unique", 69)
    source.SetValue("will-be-overridden", "WAS_OVERRIDDEN")
    let alsoSomething2BeCombined = DynamicObj()
    alsoSomething2BeCombined.SetValue("inner_combined","Complete")
    source.SetValue("nested-will-be-combined", alsoSomething2BeCombined)
    source.SetValue("nested-will-be-overridden", "WAS_OVERRIDDEN")
    
    let combined = DynObj.combine target source
    
    let expected = DynamicObj()

    expected.SetValue("source-unique", 69)
    expected.SetValue("target-unique", 1337)
    expected.SetValue("will-be-overridden", "WAS_OVERRIDDEN")
    expected.SetValue("nested-will-be-overridden", "WAS_OVERRIDDEN")
    expected.SetValue("nested-will-be-combined", 
        let inner = DynamicObj()
        inner.SetValue("inner","I Am")
        inner.SetValue("inner_combined","Complete")
        inner
        )

    Assert.Equal(expected, combined)

[<Fact>]
let ``Test nested prints (GitHub issue 14)``() = 
    // https://github.com/CSBiology/DynamicObj/issues/14
    let outer = DynamicObj()
    let inner = DynamicObj()
    inner.SetValue("Level", "Information")
    inner.SetValue("MessageTemplate","{Method} Request at {Path}")
    outer.SetValue("serilog", inner)

    let print =
        try 
            outer |> DynObj.print
            true
        with
            | e -> false

    Assert.True(print)


// DynamicObj of json tests

let private minifyJson(json:string) = 
    json
        .Replace(" ","")
        // if i add this line, my tests break
        // .Replace("\n",System.Environment.NewLine)
        .Replace(System.Environment.NewLine,"") 

[<Fact>]
let ``Test json string to DynamicObj compared to DynamicObj created by hand.``() = 
    let simpleJson = """{"firstLevel": "test"}"""
    let dynObjOfJson = DynamicObj.ofJson(simpleJson)
    let dynObj = 
        let l = DynamicObj()
        l.SetValue("firstLevel", "test")
        l

    Assert.Equal(dynObj,dynObjOfJson)

[<Fact>]
let ``Test json string to DynamicObj and back to json``() =
    let simpleJson = minifyJson """{"firstLevel": "test"}"""
    let dynObjOfJson = DynamicObj.ofJson(simpleJson)
    let revertToJson = DynamicObj.toJson(dynObjOfJson)

    Assert.Equal(simpleJson, revertToJson)

[<Fact>]
let ``Test nested simple json object``() =
    let json = minifyJson """{"firstLevel": {"name": "firstLevelName"}}"""
    let dynObjOfJson = DynamicObj.ofJson json
    let revertToJson = DynamicObj.toJson dynObjOfJson

    Assert.Equal(json, revertToJson)

[<Fact>]
let ``Test json number types``() =
    let json = minifyJson """{"depth": 2, "floatingBoat": 3.51}"""
    let dynObjOfJson = DynamicObj.ofJson json 
    let revertToJson = DynamicObj.toJson dynObjOfJson

    Assert.Equal(json, revertToJson)

[<Fact>]
let ``Test 3-level nested json object with string and number json types``() =
    let json = minifyJson """{"firstLevel": {"name": "firstLevelName","type": "object","firstLevelProperties": {"depth": 2,"floatingBoat": 3.51}}}"""
    let dynObjOfJson = DynamicObj.ofJson(json)
    let revertToJson = DynamicObj.toJson dynObjOfJson

    Assert.Equal(json,revertToJson)

[<Fact>]
let ``Test Integer, float, bool, null json types``() =
    let json = minifyJson """{"depth": 2,"floatingBoat": 3.51,"isTrue?": true,"isNull?": null}"""
    let dynObjOfJson = DynamicObj.ofJson json 
    let revertToJson = DynamicObj.toJson dynObjOfJson

    Assert.Equal(json,revertToJson)

[<Fact>]
let ``Test basic json array type``() =
    let json = minifyJson """{"myfirstArray": ["value1", "value2", "value3"]}"""
    let dynObjOfJson = DynamicObj.ofJson json
    let revertToJson = DynamicObj.toJson dynObjOfJson

    Assert.Equal(json,revertToJson)

[<Fact>]
let ``Compare 'ofJson' to DynamicObj created by hand, for json array type``() =
    let simpleJson = """{"myfirstArray": ["value1", "value2", "value3"]}"""
    let dynObjOfJson = DynamicObj.ofJson(simpleJson)
    let dynObj = 
        let l = DynamicObj()
        /// Sadly i am not able to avoid converting to 'obj list'.
        let list: obj list = ["value1"; "value2"; "value3"]
        l.SetValue("myfirstArray", list)
        l

    Assert.Equal(dynObjOfJson, dynObj)
    
[<Fact>]
let ``Test nested json array with object elements``() =
    let json = minifyJson """{"myfirstArray": [{"name": "John","age": 30},{"name": "Mary","age": 25},{"name": "Peter","age": 20}]}"""
    let dynObjOfJson = DynamicObj.ofJson json
    let revertToJson = DynamicObj.toJson dynObjOfJson

    Assert.Equal(json,revertToJson)

[<Fact>]
let ``Test root level json array with object elements``() =
    let json = minifyJson """[{"name": "John","age": 30},{"name": "Mary","age": 25},{"name": "Peter","age": 20}]"""
    let dynObjOfJson = DynamicObj.ofJson json
    let revertToJson = DynamicObj.toJson dynObjOfJson

    Assert.Equal(json,revertToJson)

[<Fact>]
let ``Test empty json objects``() =
    let json = minifyJson """{"name": {}}"""
    let dynObjOfJson = DynamicObj.ofJson json
    let revertToJson = DynamicObj.toJson dynObjOfJson

    Assert.Equal(json,revertToJson)

[<Fact>]
let ``Root json array with simple elements``() =
    let json = minifyJson """["Ford", "BMW", "Fiat"]"""
    let dynObjOfJson = DynamicObj.ofJson json
    let revertToJson = DynamicObj.toJson dynObjOfJson

    Assert.Equal(json,revertToJson)
