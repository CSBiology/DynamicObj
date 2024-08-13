#r @"C:\Users\HLWei\source\repos\other\DynamicObj\tests\DynamicObject.Tests\bin\Release\net6.0\Fable.Pyxpecto.dll"
#r @"C:\Users\HLWei\source\repos\other\DynamicObj\tests\DynamicObject.Tests\bin\Release\net6.0\Fable.Core.dll"
#r @"C:\Users\HLWei\source\repos\other\DynamicObj\tests\DynamicObject.Tests\bin\Release\net6.0\Fable.Python.dll"
#r @"C:\Users\HLWei\source\repos\other\DynamicObj\tests\DynamicObject.Tests\bin\Release\net6.0\Microsoft.TestPlatform.Utilities.dll"
#r @"C:\Users\HLWei\source\repos\other\DynamicObj\tests\DynamicObject.Tests\bin\Release\net6.0\Microsoft.TestPlatform.CommunicationUtilities.dll"
#r @"C:\Users\HLWei\source\repos\other\DynamicObj\tests\DynamicObject.Tests\bin\Release\net6.0\Newtonsoft.Json.dll"
#r @"C:\Users\HLWei\source\repos\other\DynamicObj\tests\DynamicObject.Tests\bin\Release\net6.0\Microsoft.VisualStudio.CodeCoverage.Shim.dll"
#r @"C:\Users\HLWei\source\repos\other\DynamicObj\tests\DynamicObject.Tests\bin\Release\net6.0\DynamicObject.Tests.dll"
#r @"C:\Users\HLWei\source\repos\other\DynamicObj\tests\DynamicObject.Tests\bin\Release\net6.0\DynamicObj.dll"

open DynamicObj

//let a = DynamicObj ()

//a.SetValue("aaa", 5)

//a.GetHashCode()

//ReflectionUtils.getStaticProperties (a)

//let o = a

//let t = o.GetType()
//[|
//    for propInfo in t.GetProperties() -> propInfo
//    for i in t.GetInterfaces() do yield! i.GetProperties()
//|]
//|> Array.map PropertyHelper.fromPropertyInfo






type Person(name : string) =
    
    inherit DynamicObj()

    let mutable name = name

    member this.Name
        with get() = name
        //and set(value) = name <- value

let p = Person("John")
p.SetValue("Name", "Jane")

p.TryGetValue("Name")

ReflectionUtils.tryGetPropertyValue p "Name"

ReflectionUtils.tryGetPropertyInfo p "Name"