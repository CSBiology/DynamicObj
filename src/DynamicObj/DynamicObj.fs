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

[<AttachMembers>]
type DynamicObj() = 
    


    let mutable properties = new Dictionary<string, obj>()

    member this.Properties
        with get() = properties
        and internal set(value) = properties <- value           

    static member fromDict dict = 
        let obj = DynamicObj()
        obj.Properties <- dict
        obj

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
        
        match ReflectionUtils.tryGetPropertyInfo this name  with
        | Some pi ->
            if pi.IsMutable then
                pi.SetValue this value
            else
                failwith $"Cannot set value for static, immutable property \"{name}\""
        | None -> 
            #if FABLE_COMPILER_JAVASCRIPT  || FABLE_COMPILER_TYPESCRIPT
            FableJS.setPropertyValue this name value
            #endif
            #if FABLE_COMPILER_PYTHON
            FablePy.setPropertyValue this name value
            #else
            // Next check the Properties collection for member
            match properties.TryGetValue name with            
            | true,_ -> properties.[name] <- value
            | _      -> properties.Add(name,value)
            #endif

    member this.Remove name =
        match ReflectionUtils.removeStaticProperty this name with
        | true -> true
        // Maybe in map
        | false -> properties.Remove(name)


    member this.GetPropertyHelpers (includeInstanceProperties) =
        #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT           
        FableJS.getPropertyHelpers this
        |> Seq.filter (fun pd ->  
            includeInstanceProperties || pd.IsDynamic
        )
        #endif
        #if FABLE_COMPILER_PYTHON
        FablePy.getPropertyHelpers this
        |> Seq.filter (fun pd ->  
            includeInstanceProperties || pd.IsDynamic
        )
        #else
        seq [
            if includeInstanceProperties then                
                yield! ReflectionUtils.getStaticProperties (this)
            for key in properties.Keys ->
                {
                    Name = key
                    IsStatic = false
                    IsDynamic = true
                    IsMutable = true
                    IsImmutable = false
                    GetValue = fun o -> properties.[key]
                    SetValue = fun o v -> properties.[key] <- v
                    RemoveValue = fun o -> properties.Remove(key) |> ignore
                }
        ]
        #endif
        |> Seq.filter (fun p -> p.Name.ToLower() <> "properties")

    /// Returns both instance and dynamic properties when passed true, only dynamic properties otherwise. 
    /// Properties are returned as a key value pair of the member names and the boxed values
    member this.GetProperties includeInstanceProperties : seq<KeyValuePair<string,obj>> =    
        #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT        
        FableJS.getPropertyHelpers this
        |> Seq.choose (fun pd ->  
            if includeInstanceProperties || pd.IsDynamic then
                new KeyValuePair<string, obj>(pd.Name, pd.GetValue this)
                |> Some
            else
                None  
        )
        #endif
        #if FABLE_COMPILER_PYTHON
        FablePy.getPropertyHelpers this
        |> Seq.choose (fun pd ->  
            if includeInstanceProperties || pd.IsDynamic then
                new KeyValuePair<string, obj>(pd.Name, pd.GetValue this)
                |> Some
            else
                None  
        )
        #else
        seq [
            if includeInstanceProperties then                
                for prop in ReflectionUtils.getStaticProperties (this) -> 
                    new KeyValuePair<string, obj>(prop.Name, prop.GetValue(this))
            for key in properties.Keys ->
               new KeyValuePair<string, obj>(key, properties.[key]);
        ]
        #endif
        |> Seq.filter (fun kv -> kv.Key.ToLower() <> "properties")

    member this.GetPropertyNames(includeInstanceProperties) =
        this.GetProperties(includeInstanceProperties)
        |> Seq.map (fun kv -> kv.Key)

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
            this.GetHashCode() = other.GetHashCode()
        | _ -> false

    override this.GetHashCode () =
        this.GetProperties(true)
        |> Seq.map (fun kv ->
            kv
        )
        |> Seq.sortBy (fun pair -> pair.Key)
        |> HashCodes.boxHashKeyValSeq
        |> fun x -> x :?> int