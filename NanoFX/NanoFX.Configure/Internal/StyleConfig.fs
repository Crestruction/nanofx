namespace NanoFX.Configure.Internal

open System.Collections.Generic
open System.Text
open YamlDotNet.Serialization

[<AllowNullLiteral>]
type StyleConfig() =
    
    [<YamlMember(Alias="page")>]
    member val Page: List<string> = List<string>() with get, set
    
    [<YamlMember(Alias="section")>]
    member val Section: List<string> = List<string>() with get, set
    
    [<YamlMember(Alias="header")>]
    member val Header: List<string> = List<string>() with get, set
    
    [<YamlMember(Alias="button")>]
    member val Button: List<string> = List<string>() with get, set
           
    member this.GetHtmlClass(definations:List<string>) =
        let builder = StringBuilder()        
        definations.ForEach(fun p -> builder.AppendFormat(" {0} ", p) |> ignore)
        builder.ToString()
    
    member this.GetButtonClass() =
        this.GetHtmlClass(this.Button)
    
    member this.GetSectionClass() =
        this.GetHtmlClass(this.Section)