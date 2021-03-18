namespace NanoFX.Configure.Internal

open YamlDotNet.Serialization

[<AllowNullLiteral>]
type SiteConfig () =
    
    [<YamlMember(Alias="name")>]
    member val SiteName: string = null with get, set
    
    [<YamlMember(Alias="icon")>]
    member val IconPath: string = null with get, set
    
    [<YamlMember(Alias="copyright")>]
    member val CopyRight: string = null with get, set