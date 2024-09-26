(*** condition: prepare ***)
#r "../src/DynamicObj/bin/Release/netstandard2.0/DynamicObj.dll"

(**
# DynamicObj

The primary use case of DynamicObj is the **extension of F# classes with dynamic properties**.
This is useful when you want to add arbitrarily typed properties to a class **at runtime**.

> Why would you want to do that?

Yes, The type system is one of the core strengths of F#, and it is awesome.
However, there are cases where a static domain model is either unfeasible or not flexible enough, especially when interfacing with dynamic languages such as JavaScript or Python.

DynamicObj is transpilable into JS and Python via [Fable](https://github.com/fable-compiler/Fable), meaning you can use it to create classes that are usable in both .NET and those languages, while making their usage (e.g., the setting of dynamic properties) both safe in .NET and idiomatic in JS/Python.

## Get started

*)

#r "nuget: Fable.Core" // Needed if working with Fable

open DynamicObj
open Fable.Core // Needed if working with Fable

[<AttachMembers>] // AttachMembers needed if working with Fable
type Person(id : int, name : string) =
    
    // Include this in your class
    inherit DynamicObj()

    // Mutable static property
    let mutable _name = name
    member this.Name
        with get() = _name
        and set(value) = _name <- value

    // Immutable static property
    member this.ID 
        with get() = id

let p = Person(1337,"John")

(**
### Accessing static and dynamic properties

Any class inheriting from `DynamicObj` can have static and dynamic properties, and both are accessible via various instance methods.

### Access Static Properties:
*)
p.Name
(***include-it***)
p.GetPropertyValue("Name")
(***include-it***)

(**
### Overwrite mutable static property
*)
p.SetProperty("Name","Jane")
p.GetPropertyValue("Name")
(***include-it***)

(**
### You cannot overwrite mutable static properties
*)

(***do-not-eval***)
p.SetProperty("ID",1234) // throws an excpection
(***)
p.GetPropertyValue("ID")
(***include-it***)

(**
### Set dynamic properties
*)
p.SetProperty("Address","FunStreet")
p.GetPropertyValue("Address")
(***include-it***)

(**
### Safe and typed access to dynamic properties

Note that all properties returted by `GetPropertyValue` are boxed in .NET.

If you want to get the value in a typed manner, you can use the `TryGetTypedPropertyValue` method:
*)

p.TryGetTypedPropertyValue<string>("Name")
(***include-it***)
p.TryGetTypedPropertyValue<int>("Name")
(***include-it***)
p.TryGetTypedPropertyValue<string>("I Do Not Exist")
(***include-it***)
(**
**Attention: the TryGetTypedPropertyValue<'T> method is not transpilable via Fable as it can only provide access to the types known at transpilation.
However, You can use the respective [module function](#the-dynobj-module) to transpile typed dynamic member access.**
*)
    
(**
## The DynObj module

This module provides a lot of API functions that are not not desired as static methods on `DynamicObj`, as it would be confusing if they ended up on inheriting classes.

It also supports pipeline chaining.
*)

p
|> DynObj.tryGetTypedPropertyValue<int> "ID"
(***include-it***)

p
|> DynObj.withProperty "Another" "prop"
|> DynObj.withProperty "Yes" 42
|> DynObj.withoutProperty "Address"
|> DynObj.withOptionalProperty "Maybe" (Some "yes")
|> DynObj.withOptionalProperty "Maybe not" None
|> DynObj.format
(***include-it***)



(**
## Serialization

Serialization to a JSON string that contains both static and dynamic properties is supported out-of-the-box when using [Newtonsoft.Json]():

*)

#r "nuget: Newtonsoft.Json"

open Newtonsoft.Json

p
|> JsonConvert.SerializeObject
(***include-it***)
