# DynamicObj
F# library supporting Dynamic Objects including inheritance in functional style.

The library is compatible with [Fable](https://github.com/fable-compiler/Fable), allowing transpilation to `javascript` and `python`.



## Usage example

### Get started

```fsharp
#r "nuget: DynamicObj"
#r "nuget: Fable.Core" // Needed if working with Fable

open DynamicObj
open Fable.Core // Needed if working with Fable

[<AttachMembers>] // AttachMembers needed if working with Fable
type Person(id : int, name : string) =
    
    // Include this in your class
    inherit DynamicObj()

    let mutable name = name

    // Mutable property
    member this.Name
        with get() = name
        and set(value) = name <- value

    // Immutable property
    member this.ID 
        with get() = id

let p = Person(1337,"John")
```

### Accessing static and dynamic properties

```fsharp

// Access Static Properties
p.GetValue("Name") // val it: obj = "John"
p.GetValue("ID")   // val it: obj = 1337


// Overwrite mutable static property
p.SetValue("Name","Jane") // val it: unit = ()
// Overwrite immutable static property
p.SetValue("ID",1234) // System.Exception: Cannot set value for static, immutable property "ID"
// Set dynamic property
p.SetValue("Address","FunStreet") // val it: unit = ()


// Access Properties
p.GetValue("Name")    // val it: obj = "Jane"
p.Name                // val it: string = "Jane"
p.GetValue("ID")      // val it: obj = 1337
p.ID                  // val it: int = 1337
p.GetValue("Address") // val it: obj = "FunStreet"
```

### Practical helpers

```fsharp
DynObj.format p
|> printfn "%s"
```
-> 
```
Name: Jane
ID: 1337
?Address: FunStreet
```

## Development

#### Requirements

- [nodejs and npm](https://nodejs.org/en/download)
    - verify with `node --version` (Tested with v18.16.1)
    - verify with `npm --version` (Tested with v9.2.0)
- [.NET SDK](https://dotnet.microsoft.com/en-us/download)
    - verify with `dotnet --version` (Tested with 7.0.306)
- [Python](https://www.python.org/downloads/)
    - verify with `py --version` (Tested with 3.12.2, known to work only for >=3.11)

#### Local Setup

On windows you can use the `setup.cmd` to run the following steps automatically!

1. Setup dotnet tools

   `dotnet tool restore`


2. Install NPM dependencies
   
    `npm install`

3. Setup python environment
    
    `py -m venv .venv`

4. Install [Poetry](https://python-poetry.org/) and dependencies

   1. `.\.venv\Scripts\python.exe -m pip install -U pip setuptools`
   2. `.\.venv\Scripts\python.exe -m pip install poetry`
   3. `.\.venv\Scripts\python.exe -m poetry install --no-root`

Verify correct setup with `./build.cmd runtests` ✨