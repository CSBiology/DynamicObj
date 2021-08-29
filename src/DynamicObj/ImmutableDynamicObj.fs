module ImmutableDynamicObj

type ImmutableDynamicObj (map : Map<string, obj>) = 
    
    let properties = map
    
    member private this.Properties = properties

    new () = ImmutableDynamicObj Map.empty

    member this.Item
        with get(index) =
            this.Properties.[index]

    static member With name newValue (object : ImmutableDynamicObj) =
        match Map.tryFind name object.Properties with
        | Some(value) when value = newValue -> object
        | _ -> ImmutableDynamicObj (Map.add name newValue object.Properties)

    static member (+=) (object, (name, newValue)) = ImmutableDynamicObj.With name newValue object

    member this.TryGetTypedValue<'a> name = 
        match (this.Properties.TryFind name) with
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