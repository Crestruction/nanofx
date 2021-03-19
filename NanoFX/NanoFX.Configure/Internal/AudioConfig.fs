namespace NanoFX.Configure.Internal

open YamlDotNet.Serialization

[<AllowNullLiteral>]
type AudioConfig() =
    
    [<YamlMember(Alias="id")>]
    member val Id: string = null with get, set
    
    [<YamlMember(Alias="path")>]
    member val Path: string = null with get, set
    
    [<YamlMember(Alias="title")>]
    member val Title: string = null with get, set