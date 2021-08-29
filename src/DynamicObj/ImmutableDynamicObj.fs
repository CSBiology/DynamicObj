namespace DynamicObj

open DynamicObj
open System
open System.Runtime.CompilerServices

[<InternalsVisibleToAttribute("UnitTests")>]
do()

/// Represents an DynamicObj's counterpart
/// with immutability enabled only.
type ImmutableDynamicObj (map : Map<string, obj>) = 
    
    let mutable properties = map
    
    member private this.Properties = properties
    member private this.MutateSetMap newMap =
        properties <- newMap
    
    static member private NewIfNeeded (object : 'a when 'a :> ImmutableDynamicObj) map : 'a =
        if obj.ReferenceEquals(map, object.Properties) then
            object
        else
            let res = new 'a()
            res.MutateSetMap map
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
    static member With name newValue (object : #ImmutableDynamicObj) =
        object.Properties
        |> Map.add name newValue
        |> ImmutableDynamicObj.NewIfNeeded object

    /// Returns an instance:
    /// 1. the same if there was no requested property
    /// 2. without the requested property if there was
    static member Without name (object : #ImmutableDynamicObj) =
        object.Properties
        |> Map.remove name
        |> ImmutableDynamicObj.NewIfNeeded object

    /// Returns an instance with:
    /// 1. this property added if it wasn't present
    /// 2. this property updated otherwise
    static member (++) (object, (name, newValue)) = ImmutableDynamicObj.With name newValue object

    /// Returns an instance:
    /// 1. the same if there was no requested property
    /// 2. without the requested property if there was
    static member (--) (object, name) = ImmutableDynamicObj.Without name object

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
    static member With (this, name, newValue) =
        ImmutableDynamicObj.With name newValue this

    /// Returns an instance:
    /// 1. the same if there was no requested property
    /// 2. without the requested property if there was
    /// use this one only from C#
    [<Extension>]
    static member Without (this, name) =
        ImmutableDynamicObj.Without name this