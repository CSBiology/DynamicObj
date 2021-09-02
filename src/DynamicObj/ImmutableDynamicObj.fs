namespace DynamicObj

open DynamicObj
open System.Reflection
open System.Runtime.CompilerServices

[<InternalsVisibleToAttribute("UnitTests")>]
do()

/// Represents an DynamicObj's counterpart
/// with immutability enabled only.
type ImmutableDynamicObj internal (map : Map<string, obj>) = 
    
    let mutable properties = map
    
    member private this.Properties 
        with get () =
            properties
        and set value =
            properties <- value
    
    // Copies the fields of one object to the other one
    // If their base is not ImmutableDynamicObj, then it
    // will go over fields from the base instance
    static member private copyMembers (ty : System.Type) sourceObject destinationObject =
        for fi in ty.GetFields(BindingFlags.Instance ||| BindingFlags.NonPublic ||| BindingFlags.Public) do
            let fieldValue = fi.GetValue sourceObject
            fi.SetValue(destinationObject, fieldValue)

    static member private newIfNeeded (object : 'a when 'a :> ImmutableDynamicObj) map : 'a =
        if obj.ReferenceEquals(map, object.Properties) then
            object
        else
            // otherwise we create a new instance
            let res = new 'a()

            // and then copy all current fields the new instance 
            ImmutableDynamicObj.copyMembers (typeof<'a>) object res
            res.Properties <- map
            res

    /// Empty instance
    new () = ImmutableDynamicObj Map.empty

    static member empty = ImmutableDynamicObj ()

    /// Indexes ; if no key found, throws
    member this.Item
        with get(index) =
            this.Properties.[index]

    /// Returns an instance with:
    /// 1. this property added if it wasn't present
    /// 2. this property updated otherwise
    static member add name newValue (object : #ImmutableDynamicObj) =
        object.Properties
        |> Map.add name newValue
        |> ImmutableDynamicObj.newIfNeeded object

    /// Returns an instance:
    /// 1. the same if there was no requested property
    /// 2. without the requested property if there was
    static member remove name (object : #ImmutableDynamicObj) =
        object.Properties
        |> Map.remove name
        |> ImmutableDynamicObj.newIfNeeded object

    member this.TryGetValue name = 
        match this.Properties.TryGetValue name with
        | true, value ->  Some value
        | _ -> ReflectionUtils.tryGetPropertyValue this name

    member this.TryGetTypedValue<'a> name = 
        match this.TryGetValue name with
        | None -> None
        | Some o -> 
            match o with
            | :? 'a as o -> o |> Some
            | _ -> None

    override this.Equals o =
        match o with
        | :? ImmutableDynamicObj as other -> other.Properties = this.Properties
        | _ -> false

    override this.GetHashCode () = ~~~map.GetHashCode()

[<Extension>]
type ImmutableDynamicObjExtensions =

    /// Returns an instance with:
    /// 1. this property added if it wasn't present
    /// 2. this property updated otherwise
    /// use this one only from C#
    [<Extension>]
    static member AddItem (this, name, newValue) =
        ImmutableDynamicObj.add name newValue this

    /// Returns an instance:
    /// 1. the same if there was no requested property
    /// 2. without the requested property if there was
    /// use this one only from C#
    [<Extension>]
    static member RemoveItem (this, name) =
        ImmutableDynamicObj.remove name this