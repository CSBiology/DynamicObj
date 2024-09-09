namespace DynamicObj

open System.Collections.Generic

module DynObj =

    /// <summary>
    /// Creates a new DynamicObj from a Dictionary containing dynamic properties.
    /// </summary>
    /// <param name="dynamicProperties">The dictionary with the dynamic properties</param>
    let ofDict (dynamicProperties: Dictionary<string, obj>) = DynamicObj.fromDict dynamicProperties

    /// <summary>
    /// Creates a new DynamicObj from a sequence of key value pairs containing dynamic properties.
    /// </summary>
    /// <param name="dynamicProperties"></param>
    let ofSeq (dynamicProperties: seq<string * obj>) = 
        dynamicProperties
        |> dict
        |> Dictionary
        |> DynamicObj.fromDict

    /// <summary>
    /// Creates a new DynamicObj from a list of key value pairs containing dynamic properties.
    /// </summary>
    /// <param name="dynamicProperties"></param>
    let ofList (dynamicProperties: (string * obj) list) = 
        dynamicProperties
        |> ofSeq

    /// <summary>
    /// Creates a new DynamicObj from an array of key value pairs containing dynamic properties.
    /// </summary>
    /// <param name="dynamicProperties"></param>
    let ofArray (dynamicProperties: (string * obj) array) = 
        dynamicProperties
        |> ofSeq

    /// <summary>
    /// Combines the dynamic properties of the second DynamicObj onto the first. 
    ///
    /// In case of duplicate property names the members of the second object override those of the first.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <remarks>This function mutates the first input DynamicObj</remarks>
    let rec combine (first:DynamicObj) (second:DynamicObj) =
        //printfn "Type %A" (first.GetType())
        /// Consider deep-copy of first
        for kv in (second.GetProperties true) do 
            match kv.Value with
            | :? DynamicObj as valueS -> 
                // is dynObj in second
                match first.TryGetValue (kv.Key) with
                | Some valueF -> 
                    let tmp = combine (unbox valueF) (unbox valueS)
                    first.SetValue(kv.Key,tmp)
                | None -> first.SetValue(kv.Key,valueS)
            | _ -> first.SetValue(kv.Key,kv.Value)
        first

    /// <summary>
    /// Returns Some('TPropertyValue) when a dynamic property with the given name and type exists on the input DynamicObj, otherwise None.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="dynObj"></param>
    let inline tryGetTypedValue<'TPropertyValue> (propertyName:string) (dynObj : DynamicObj) : 'TPropertyValue option =
        match (dynObj.TryGetValue propertyName) with
        | None -> None
        | Some o -> 
            match o with
            | :? 'TPropertyValue as o -> o |> Some
            | _ -> None

    /// <summary>
    /// Sets the given dynamic property name and value on the given DynamicObj.
    /// </summary>
    /// <param name="propertyName">The name of the dynamic property to set</param>
    /// <param name="propertyValue">The value of the dynamic property to set</param>
    /// <param name="dynObj">The DynamicObj to set the property on</param>
    /// <remarks>This function mutates the input DynamicObj</remarks>
    let setValue (propertyName:string) (propertyValue:'TPropertyValue) (dynObj : DynamicObj) =
        dynObj.SetValue(propertyName,propertyValue)

    /// <summary>
    /// Sets the given dynamic property name and value on the given DynamicObj and returns it.
    /// </summary>
    /// <param name="propertyName">The name of the dynamic property to set</param>
    /// <param name="propertyValue">The value of the dynamic property to set</param>
    /// <param name="dynObj">The DynamicObj to set the property on</param>
    /// <remarks>This function mutates the input DynamicObj</remarks>
    let withValue (propertyName:string) (propertyValue:'TPropertyValue) (dynObj: DynamicObj) =
        setValue propertyName propertyValue dynObj
        dynObj

    /// <summary>
    /// Sets the given dynamic property name and value on the given DynamicObj if the value is Some('TPropertyValue).
    /// If the given propertyValue is None, does nothing to the input DynamicObj.
    /// </summary>
    /// <param name="propertyName">The name of the dynamic property to set</param>
    /// <param name="propertyValue">The value of the dynamic property to set</param>
    /// <param name="dynObj">The DynamicObj to set the property on</param>
    /// <remarks>This function mutates the input DynamicObj</remarks>
    let setValueOpt (propertyName: string) (propertyValue: 'TPropertyValue option) (dynObj: DynamicObj) = 
        match propertyValue with
        | Some pv -> dynObj |> setValue propertyName pv
        | None -> ()

    /// <summary>
    /// Sets the given dynamic property name and value on the given DynamicObj if the value is Some('TPropertyValue) and returns it.
    /// If the given propertyValue is None, returns the unchanged DynamicObj.
    /// </summary>
    /// <param name="propertyName">The name of the dynamic property to set</param>
    /// <param name="propertyValue">The value of the dynamic property to set</param>
    /// <param name="dynObj">The DynamicObj to set the property on</param>
    /// <remarks>This function mutates the input DynamicObj</remarks>
    let withValueOpt (propertyName: string) (propertyValue: 'TPropertyValue option) (dynObj: DynamicObj) = 
        match propertyValue with
        | Some pv -> dynObj |> withValue propertyName pv
        | None -> dynObj

    /// <summary>
    /// Sets the given dynamic property name with the result of a mapping function applied to the given property value on the given DynamicObj if the value is Some('TPropertyValue).
    /// If the given propertyValue is None, does nothing to the input DynamicObj.
    /// </summary>
    /// <param name="propertyName">The name of the dynamic property to set</param>
    /// <param name="propertyValue">The value of the dynamic property to set</param>
    /// <param name="mapping">A function to apply to the property value before setting it on the DynamicObj</param>
    /// <param name="dynObj">The DynamicObj to set the property on</param>
    /// <remarks>This function mutates the input DynamicObj</remarks>
    let setValueOptBy (propertyName: string) (propertyValue: 'TPropertyValue option) (mapping: 'TPropertyValue -> 'UPropertyValue) (dynObj: DynamicObj) = 
        match propertyValue with
        | Some pv -> dynObj |> setValue propertyName (mapping pv)
        | None -> ()

    /// <summary>
    /// Sets the given dynamic property name with the result of a mapping function applied to the given property value on the given DynamicObj if the value is Some('TPropertyValue) and returns it.
    /// If the given propertyValue is None, returns the unchanged DynamicObj.
    /// </summary>
    /// <param name="propertyName">The name of the dynamic property to set</param>
    /// <param name="propertyValue">The value of the dynamic property to set</param>
    /// <param name="mapping">A function to apply to the property value before setting it on the DynamicObj</param>
    /// <param name="dynObj">The DynamicObj to set the property on</param>
    /// <remarks>This function mutates the input DynamicObj</remarks>
    let withValueOptBy (propertyName: string) (propertyValue: 'TPropertyValue option) (mapping: 'TPropertyValue -> 'UPropertyValue) (dynObj: DynamicObj) = 
        match propertyValue with
        | Some pv -> dynObj |> withValue propertyName (mapping pv)
        | None -> dynObj

    /// <summary>
    /// Returns Some(boxed value) if the DynamicObj contains a dynamic property with the given name, and None otherwise.
    /// </summary>
    /// <param name="propertyName">The name of the dynamic property to get</param>
    /// <param name="dynObj">The DynamicObj to get the property from</param>
    let tryGetValue (propertyName: string) (dynObj: DynamicObj) = 
        dynObj.TryGetValue propertyName

    /// <summary>
    /// Removes any dynamic property with the given name from the input DynamicObj.
    /// </summary>
    /// <param name="propertyName">The name of the dynamic property to remove</param>
    /// <param name="dynObj">The DynamicObj to remove the property from</param>
    /// <remarks>This function mutates the input DynamicObj</remarks>
    let remove (propertyName: string) (dynObj: DynamicObj) = 
        DynamicObj.remove (dynObj, propertyName) |> ignore

    /// <summary>
    /// Returns the input DynamicObj with any dynamic property with the given name removed.
    /// </summary>
    /// <param name="propertyName">The name of the dynamic property to remove</param>
    /// <param name="dynObj">The DynamicObj to remove the property from</param>
    /// <remarks>This function mutates the input DynamicObj</remarks>
    let withoutProperty(propertyName: string) (dynObj: DynamicObj) = 
        dynObj |> remove propertyName
        dynObj

    /// <summary>
    /// Returns a formatted string containing all static and dynamic properties of the given DynamicObj
    /// </summary>
    /// <param name="dynObj">The DynamicObj for which to generate a formatted string for</param>
    let format (dynObj:DynamicObj) =
    
        let members = dynObj.GetPropertyHelpers(true) |> List.ofSeq

        let rec loop (object:DynamicObj) (indentationLevel:int) (membersLeft:PropertyHelper list) (acc:string list) =
            let indent = [for i in 0 .. indentationLevel-1 do yield "    "] |> String.concat ""
            match membersLeft with
            | [] -> acc |> List.rev |> String.concat System.Environment.NewLine
            | m::rest ->
                let item = m.GetValue object
                let dynamicIndicator = if m.IsDynamic then "?" else ""
                let name = m.Name
                match item with
                | :? DynamicObj as item -> 
                    let innerMembers = item.GetPropertyHelpers(true) |> List.ofSeq
                    let innerPrint = (loop item (indentationLevel + 1) innerMembers [])              
                    loop object indentationLevel rest ($"{indent}{dynamicIndicator}{name}:{System.Environment.NewLine}{innerPrint}" :: acc)
                | item -> 
                    loop object indentationLevel rest ($"{indent}{dynamicIndicator}{name}: {item}"::acc)
    
        loop dynObj 0 members []

    /// <summary>
    /// Prints a formatted string containing all static and dynamic properties of the given DynamicObj
    /// </summary>
    /// <param name="dynObj">The DynamicObj for which to print a formatted string for</param>
    let print (dynObj:DynamicObj) = printfn "%s" (dynObj |> format)