module InheritanceView.Tests

open System
open Fable.Pyxpecto
open DynamicObj
open Fable.Core

let id = "id"

let firstName = "firstName"

[<AttachMembers>]
type Person(?baseOn : Person) 
    #if !FABLE_COMPILER
    as self     
    #endif
    =

    inherit DynamicObj()
    
    let _ = 
        
        match baseOn with
        | Some dynOb -> 
            #if !FABLE_COMPILER
                self.Properties <- dynOb.Properties
            #endif
            #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
                FableJS.baseObjectOn dynOb
            #endif
            #if FABLE_COMPILER_PYTHON
                ()
            #endif
        | None -> ()
        
    #if FABLE_COMPILER_PYTHON
    do Fable.Core.PyInterop.emitPyStatement "" """
    def __new__(cls, base_on: "Person | None" = None):
        if base_on is not None:
            return base_on  

        return super().__new__(cls)
    """
    #endif


    //#if FABLE_COMPILER_PYTHON
    //member this.__new__(cls, baseOn : Person) =
    //    Fable.Core.PyInterop.emitPyStatement (cls,baseOn) """if $1 is not None:
    //        return $1
        
    //    return super().__new__($0)
    //    """
        
    //#endif

    member this.GetID() = this.GetPropertyValue(id)

    member this.SetID(value) = this.SetProperty(id, value)

    member this.GetFirstName() = this.GetPropertyValue(firstName)

    member this.SetFirstName(value) = this.SetProperty(firstName, value)

let tests_baseOn = testList "BaseOn" [

    testCase "AnotherPerson" <| fun _ ->
        let p1 = Person()
        let name = "John"
        p1.SetFirstName(name)
        let id = "123"
        p1.SetID(id)
        Expect.equal (p1.GetFirstName()) name "P1: First name should be set"
        Expect.equal (p1.GetID()) id "P1: ID should be set"
        let p2 = Person(baseOn = p1)
        Expect.equal (p2.GetFirstName()) name "P2: First name should be set"
        Expect.equal (p2.GetID()) id "P2: ID should be set"

    testCase "InheritedMutability" <| fun _ ->
        let p1 = Person()
        let name = "John"
        p1.SetFirstName(name)
        
        let p2 = Person(baseOn = p1)
        let newName = "Jane"
        p2.SetFirstName(newName)
        Expect.equal (p2.GetFirstName()) newName "P2: First name should be set"
        Expect.equal (p1.GetFirstName()) newName "P1: First name should be set"
]

let main = testList "InheritanceView" [
    tests_baseOn
]