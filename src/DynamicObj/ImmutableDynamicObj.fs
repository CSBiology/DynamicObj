namespace DynamicObj

open DynamicObj
open System
open System.Runtime.CompilerServices

/// Represents an DynamicObj's counterpart
/// with immutability enabled only.
type ImmutableDynamicObj (map : Map<string, obj>) = 
    
    let properties = map
    
    // they're public, but because they're inline,
    // they won't be visible from other assemblies
    member this.Properties = properties
    
    static member private NewIfNeededCLSCompliant (object : 'a when 'a :> ImmutableDynamicObj) map : 'a =
        if obj.ReferenceEquals(map, object.Properties) then
            object
        else
            Activator.CreateInstance(typeof<'a>, [| map :> obj |]) :?> 'a

    static member inline NewIfNeeded (object : ^T when ^T :> ImmutableDynamicObj) map : ^T =
        if obj.ReferenceEquals(map, object.Properties) then
            object
        else
            (^T: (new: Map<string, obj> -> ^T) map)
            

    /// Empty instance
    new () = ImmutableDynamicObj Map.empty



    /// Indexes ; if no key found, throws
    member this.Item
        with get(index) =
            this.Properties.[index]

    /// Returns an instance with:
    /// 1. this property added if it wasn't present
    /// 2. this property updated otherwise
    /// Use With from F#. This one is only for non-F# code.
    static member WithCLSCompliant name newValue (object : 'a when 'a :> ImmutableDynamicObj) =
        object.Properties
        |> Map.add name newValue
        |> ImmutableDynamicObj.NewIfNeededCLSCompliant object

    /// Returns an instance with:
    /// 1. this property added if it wasn't present
    /// 2. this property updated otherwise
    static member inline With name newValue (object : ^T when ^T :> ImmutableDynamicObj) : ^T =
        object.Properties
        |> Map.add name newValue
        |> ImmutableDynamicObj.NewIfNeeded object

    /// Returns an instance:
    /// 1. the same if there was no requested property
    /// 2. without the requested property if there was
    /// Use Without from F#. This one is only for non-F# code.
    static member WithoutCLSCompliant name (object : 'a when 'a :> ImmutableDynamicObj) =
        object.Properties
        |> Map.remove name
        |> ImmutableDynamicObj.NewIfNeededCLSCompliant object

    /// Returns an instance:
    /// 1. the same if there was no requested property
    /// 2. without the requested property if there was
    static member inline Without name (object : ^T when ^T :> ImmutableDynamicObj) =
        object.Properties
        |> Map.remove name
        |> ImmutableDynamicObj.NewIfNeeded object

    /// Returns an instance with:
    /// 1. this property added if it wasn't present
    /// 2. this property updated otherwise
    static member inline (++) (object, (name, newValue)) = ImmutableDynamicObj.With name newValue object

    /// Returns an instance:
    /// 1. the same if there was no requested property
    /// 2. without the requested property if there was
    static member inline (--) (object, name) = ImmutableDynamicObj.Without name object

    member this.TryGetValue name = 
        match properties.TryGetValue name with
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
    static member With (this : 'a when 'a :> ImmutableDynamicObj) (name, newValue) =
        ImmutableDynamicObj.WithCLSCompliant name newValue this

    /// Returns an instance:
    /// 1. the same if there was no requested property
    /// 2. without the requested property if there was
    /// use this one only from C#
    [<Extension>]
    static member Without (this : 'a when 'a :> ImmutableDynamicObj, name) =
        ImmutableDynamicObj.WithoutCLSCompliant name this