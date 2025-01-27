# DynamicObj

[![Build and test](https://github.com/CSBiology/DynamicObj/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/CSBiology/DynamicObj/actions/workflows/build-and-test.yml)
[![](https://img.shields.io/nuget/vpre/DynamicObj)](https://www.nuget.org/packages/DynamicObj/)
[![](https://img.shields.io/nuget/dt/DynamicObj)](https://www.nuget.org/packages/DynamicObj/)

F# library supporting Dynamic Objects including inheritance in functional style.

The library is compatible with [Fable](https://github.com/fable-compiler/Fable), allowing transpilation to `javascript` and `python`.

The primary use case of DynamicObj is the **extension of F# classes with dynamic properties**.
This is useful when you want to add arbitrarily typed properties to a class **at runtime**.

> Why would you want to do that?

Yes, The type system is one of the core strengths of F#, and it is awesome.
However, there are cases where a static domain model is either unfeasible or not flexible enough, especially when interfacing with dynamic languages such as JavaScript or Python.

DynamicObj is transpilable into JS and Python via [Fable](https://github.com/fable-compiler/Fable), meaning you can use it to create classes that are usable in both .NET and those languages, while making their usage (e.g., the setting of dynamic properties) both safe in .NET and idiomatic in JS/Python.

## Docs

Documentation is hosted at https://csbiology.github.io/DynamicObj/

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

Verify correct setup with `./build.cmd runtests` ✨