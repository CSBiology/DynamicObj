namespace DynamicObj

//open System.Dynamic
open System.Collections.Generic
open Fable.Core

module Fable = 

    [<Emit("$0[$1]")>]
    let getProperty (o:obj) (propName:string) : 'a =
        jsNative

    [<Emit("Object.getOwnPropertyNames($0)")>]
    let getPropertyNames (o:obj) : string seq = 
        jsNative

type DynamicObj internal (dict:Dictionary<string, obj>) = 
    


    let properties = dict//new Dictionary<string, obj>()

    member this.Properties = properties

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

        match ReflectionUtils.trySetPropertyValue this name value with
        | Some _ ->
            ()
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


    /// Returns both instance and dynamic properties when passed true, only dynamic properties otherwise. 
    /// Properties are returned as a key value pair of the member names and the boxed values
    member this.GetProperties includeInstanceProperties =        
        #if FABLE_COMPILER
        Fable.getPropertyNames this
        |> Seq.map (fun name ->  new KeyValuePair<string, obj>(name, this.TryGetValue name))
        #else
        seq [
            if includeInstanceProperties then                
                for prop in ReflectionUtils.getPublicProperties (this.GetType()) -> 
                    new KeyValuePair<string, obj>(prop.Name, prop.GetValue(this, null))
            for key in properties.Keys ->
               new KeyValuePair<string, obj>(key, properties.[key]);
        ]
        #endif

    member this.GetDynamicMemberNames() =
        properties.Keys
        |> seq

    /// Operator to access a dynamic member by name
    static member (?) (lookup:#DynamicObj,name:string) =
        match lookup.TryGetValue name with
        | Some(value) -> value
        | None -> raise <| System.MemberAccessException()        

    /// Operator to set a dynamic member
    static member (?<-) (lookup:#DynamicObj,name:string,value:'v) =
        lookup.SetValue (name,value)
    
    ///// Copies all dynamic members of the DynamicObj to the target DynamicObj.
    //member this.CopyDynamicPropertiesTo(target:#DynamicObj) =
    //    this.GetProperties(false)
    //    |> Seq.iter (fun kv ->
    //        target?(kv.Key) <- kv.Value
    //    )

    ///// Returns a new DynamicObj with only the dynamic properties of the original DynamicObj (sans instance properties).
    //member this.CopyDynamicProperties() =
    //    let target = DynamicObj()
    //    this.CopyDynamicPropertiesTo(target)
    //    target

    static member GetValue (lookup:DynamicObj,name) =
        lookup.TryGetValue(name).Value

    static member Remove (lookup:DynamicObj,name) =
        lookup.Remove(name)

    override this.Equals o =
        match o with
        | :? DynamicObj as other ->
            let subdictOf (super : Dictionary<'a, 'b>) (dict : Dictionary<'a, 'b>) =
                dict
                |> Seq.forall (fun pair ->
                    let (contains, value) = super.TryGetValue pair.Key
                    contains && value.Equals(pair.Value))
            subdictOf this.Properties other.Properties
        | _ -> false

    override this.GetHashCode () =
        this.Properties
        |> List.ofSeq
        |> List.sortBy (fun pair -> pair.Key)
        |> List.map (fun pair -> struct (pair.Key, pair.Value))
        |> (fun l -> l.GetHashCode())