namespace DynamicObj

open System.Dynamic
open System.Collections.Generic


type DynamicObj internal (dict:Dictionary<string, obj>) = 
    
    inherit DynamicObject () 
    
    let properties = dict//new Dictionary<string, obj>()

    /// 
    new () = DynamicObj(new Dictionary<string, obj>())

    /// Gets property value
    member this.TryGetValue name = 
        // first check the Properties collection for member
        match properties.TryGetValue name with
        | true,value ->  Some value
        // Next check for Public properties via Reflection
        | _ -> ReflectionUtils.tryGetPropertyValue this name


    /// Gets property value
    member this.TryGetTypedValue<'a> name = 
        match (this.TryGetValue name) with
        | None -> None
        | Some o -> 
            match o with
            | :? 'a -> o :?> 'a |> Some
            | _ -> None

        
    /// Sets property value, creating a new property if none exists
    member this.SetValue (name,value) = // private
        // first check to see if there's a native property to set

        match ReflectionUtils.tryGetPropertyInfo this name with
        | Some property ->
            try 
                // let t = property.ReflectedType
                // t.InvokeMember(name,Reflection.BindingFlags.SetProperty,null,this,[|value|]) |> ignore

                //let tmp = Convert.ChangeType(this, property.ReflectedType)
                //let tmp = downcast this : (typeof<t.GetType()>)
                property.SetValue(this, value, null)
            with
            | :? System.ArgumentException -> raise <| System.ArgumentException("Readonly property - Property set method not found.")
            | :? System.NullReferenceException -> raise <| System.NullReferenceException()
        
        | None -> 
            // Next check the Properties collection for member
            match properties.TryGetValue name with            
            | true,_ -> properties.[name] <- value
            | _      -> properties.Add(name,value)

    member this.Remove name =
        match ReflectionUtils.removeProperty this name with
        | true -> true
        // Maybe in map
        | false -> properties.Remove(name)


    override this.TryGetMember(binder:GetMemberBinder,result:obj byref ) =     
        match this.TryGetValue binder.Name with
        | Some value -> result <- value; true
        | None -> false

    override this.TrySetMember(binder:SetMemberBinder, value:obj) =        
        this.SetValue(binder.Name,value)
        true

    /// Returns and the properties of
    member this.GetProperties includeInstanceProperties =        
        seq [
            if includeInstanceProperties then                
                for prop in ReflectionUtils.getPublicProperties (this.GetType()) -> 
                    new KeyValuePair<string, obj>(prop.Name, prop.GetValue(this, null))
            for key in properties.Keys ->
               new KeyValuePair<string, obj>(key, properties.[key]);
        ]

    /// Return both instance and dynamic names.
    /// Important to return both so JSON serialization with Json.NET works.
    override this.GetDynamicMemberNames() =
        this.GetProperties(true) |> Seq.map (fun pair -> pair.Key)

    static member (?) (lookup:#DynamicObj,name:string) =
        match lookup.TryGetValue name with
        | Some(value) -> value
        | None -> raise <| System.MemberAccessException()        
    static member (?<-) (lookup:#DynamicObj,name:string,value:'v) =
        lookup.SetValue (name,value)

    static member GetValue (lookup:DynamicObj,name) =
        lookup.TryGetValue(name).Value

    static member Remove (lookup:DynamicObj,name) =
        lookup.Remove(name)
