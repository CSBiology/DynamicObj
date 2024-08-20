namespace DynamicObj

open System.Collections.Generic

module DynObj =

    /// New DynamicObj of Dictionary
    let ofDict dict = DynamicObj.fromDict dict

    /// New DynamicObj of a sequence of key value
    let ofSeq kv = 
        let dict = new Dictionary<string, obj>()
        kv |> Seq.iter (fun (k,v) -> dict.Add(k,v))
        DynamicObj.fromDict dict

    /// New DynamicObj of a list of key value
    let ofList kv = 
        let dict = new Dictionary<string, obj>()
        kv |> List.iter (fun (k,v) -> dict.Add(k,v))
        DynamicObj.fromDict dict


    /// New DynamicObj of an array of key value
    let ofArray kv = 
        let dict = new Dictionary<string, obj>()
        kv |> Array.iter (fun (k,v) -> dict.Add(k,v))
        DynamicObj.fromDict dict

    
    // 
    // let rec merge (first:#DynamicObj) (second:#DynamicObj) = 
    //     let dict = new Dictionary<string, obj>()

    //     Seq.append (first.GetProperties true) (second.GetProperties true)
    //     |> Seq.iter (fun kv -> 
    //         if dict.ContainsKey(kv.Key) then
    //             match kv.Value with
    //             | :? #DynamicObj as o -> 
    //                 let oo = dict.[kv.Key] :?> #DynamicObj
    //                 dict.[kv.Key] <- merge o oo
    //             | _ -> dict.[kv.Key] <- kv.Value
    //         else 
    //             dict.Add(kv.Key, kv.Value)                
    //             )
    //     new DynamicObj(dict)

    //let rec combine<'t when 't :> DynamicObj > (first:'t) (second:'t) =
    

    /// Merges two DynamicObj (Warning: In case of duplicate property names the members of the second object override those of the first)
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

    let setValue (dyn:DynamicObj) propName o =
        dyn.SetValue(propName,o)

    let setValueOpt (dyn:DynamicObj) propName = 
        function
        | Some o -> 
            dyn.SetValue(propName,o)
        | None -> ()

    let setValueOptBy (dyn:DynamicObj) propName f = 
        function
        | Some o -> 
            dyn.SetValue(propName,f o)
        | None -> ()
    
    let tryGetValue (dyn:DynamicObj) name = 
        dyn.TryGetValue name

    let remove (dyn:DynamicObj) propName = 
        DynamicObj.remove (dyn, propName) |> ignore

    let format (d:DynamicObj) =
    
        let members = d.GetPropertyHelpers(true) |> List.ofSeq

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
    
        loop d 0 members []

    let print (d:DynamicObj) = printfn "%s" (d |> format)