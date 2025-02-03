module InheritanceView.Tests

open System
open Fable.Pyxpecto
open DynamicObj
open Fable.Core

let id = "id"
let firstName = "firstName"
let lastName = "lastName"

// We introduce and test here the concept of an inheritanceView: 
// A derived type that is inherits a base type. 
// If the constructor of the derived type is called with a base type as input, no new object is created, but the base type is used as the base object.
// The derived type can therefore be seen as a view on the base type, with additional methods to access the dynamic properties of the base type.


[<AttachMembers>]
type BaseType(?baseOn : BaseType) 
    #if !FABLE_COMPILER
    as self 
    #endif
    =

    inherit DynamicObj()
    
    let _ = 
        
        match baseOn with
        | Some dynOb -> 
            #if !FABLE_COMPILER
                DynamicObj.Helper.setProperties self dynOb.Properties
            #endif
            #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
                do Fable.Core.JsInterop.emitJsStatement "" """
                    const protoType = Object.getPrototypeOf(this);
                    Object.setPrototypeOf(baseOn, protoType);
                    return baseOn;
                """
            #endif
            #if FABLE_COMPILER_PYTHON
                ()
            #endif
        | None -> ()
        
    #if FABLE_COMPILER_PYTHON
    do Fable.Core.PyInterop.emitPyStatement "" """
    def __new__(cls, base_on: "BaseType | None" = None):
        if base_on is not None and isinstance(base_on, cls):
            return base_on  

        if base_on is not None:
            base_on.__class__ = cls
            return base_on

        return super().__new__(cls)
    """
    #endif

    member this.GetID() = this.GetPropertyValue(id)

    member this.SetID(value) = this.SetProperty(id, value)

    member this.GetFirstName() = this.GetPropertyValue(firstName)

    member this.SetFirstName(value) = this.SetProperty(firstName, value)

[<AttachMembers>]
type DerivedType(?baseOn : BaseType) =

    inherit BaseType(?baseOn = baseOn)

    member this.GetLastName() = this.GetPropertyValue(lastName)

    member this.SetLastName(value) = this.SetProperty(lastName, value)


let tests_baseType = testList "BaseType" [

    testCase "AnotherPerson" <| fun _ ->
        let p1 = BaseType()
        let name = "John"
        p1.SetFirstName(name)
        let id = "123"
        p1.SetID(id)
        Expect.equal (p1.GetFirstName()) name "P1: First name should be set"
        Expect.equal (p1.GetID()) id "P1: ID should be set"
        let p2 = BaseType(baseOn = p1)
        Expect.equal (p2.GetFirstName()) name "P2: First name should be set"
        Expect.equal (p2.GetID()) id "P2: ID should be set"

    testCase "InheritedMutability" <| fun _ ->
        let p1 = BaseType()
        let name = "John"
        p1.SetFirstName(name)
        
        let p2 = BaseType(baseOn = p1)
        let newName = "Jane"
        p2.SetFirstName(newName)
        Expect.equal (p2.GetFirstName()) newName "P2: First name should be set"
        Expect.equal (p1.GetFirstName()) newName "P1: First name should be set"
]

let tests_derivedType = testList "DerivedType" [

    testCase "OnBase_AnotherPerson" <| fun _ ->
        let p1 = BaseType()
        let name = "John"       
        p1.SetFirstName(name)
        let id = "123"
        p1.SetID(id)
        let lN = "Doe"
        p1.SetProperty(lastName,lN)
        Expect.equal (p1.GetFirstName()) name "P1: First name should be set"
        Expect.equal (p1.GetID()) id "P1: ID should be set"
        Expect.equal (p1.GetPropertyValue(lastName)) lN "P1: Last name should be set"
        let p2 = DerivedType(baseOn = p1)
        Expect.equal (p2.GetFirstName()) name "P2: First name should be set"
        Expect.equal (p2.GetID()) id "P2: ID should be set"
        Expect.equal (p2.GetLastName()) lN "P2: Last name should be set"
        
    testCase "OnBase_InheritedMutability" <| fun _ ->
        let p1 = BaseType()
        let name = "John"
        p1.SetFirstName(name)
        let lN = "Doe"
        p1.SetProperty(lastName,lN)

        let p2 = DerivedType(baseOn = p1)
        let newName = "Jane"
        p2.SetFirstName(newName)
        let newLastName = "Smith"
        p2.SetLastName(newLastName)
        Expect.equal (p2.GetFirstName()) newName "P2: First name should be set"
        Expect.equal (p2.GetLastName()) newLastName "P2: Last name should be set"
        Expect.equal (p1.GetFirstName()) newName "P1: First name should be set"
        Expect.equal (p1.GetPropertyValue(lastName)) newLastName "P1: Last name should be set"

    testCase "OnDerived_AnotherPerson" <| fun _ ->
        let p1 = DerivedType()
        let name = "John"
        p1.SetFirstName(name)
        let id = "123"
        p1.SetID(id)
        let lastName = "Doe"
        p1.SetLastName(lastName)
        Expect.equal (p1.GetFirstName()) name "P1: First name should be set"
        Expect.equal (p1.GetID()) id "P1: ID should be set"
        Expect.equal (p1.GetLastName()) lastName "P1: Last name should be set"
        let p2 = DerivedType(baseOn = p1)
        Expect.equal (p2.GetFirstName()) name "P2: First name should be set"
        Expect.equal (p2.GetID()) id "P2: ID should be set"
        Expect.equal (p2.GetLastName()) lastName "P2: Last name should be set"

    testCase "OnDerived_InheritedMutability" <| fun _ ->
        let p1 = DerivedType()
        let firstName = "John"
        p1.SetFirstName(firstName)
        let lastName = "Doe"
        p1.SetLastName(lastName)

        let p2 = DerivedType(baseOn = p1)
        let newName = "Jane"
        p2.SetFirstName(newName)
        let newLastName = "Smith"
        p2.SetLastName(newLastName)
        Expect.equal (p2.GetFirstName()) newName "P2: First name should be set"
        Expect.equal (p2.GetLastName()) newLastName "P2: Last name should be set"
        Expect.equal (p1.GetFirstName()) newName "P1: First name should be set"
        Expect.equal (p1.GetLastName()) newLastName "P1: Last name should be set"
]

let main = testList "InheritanceView" [
    tests_baseType
    tests_derivedType
]