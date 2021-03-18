namespace NanoFX.Configure.Internal

open YamlDotNet.Serialization

[<AllowNullLiteral>]
type SiteConfig () =
    
    [<YamlMember(Alias="name")>]
    member val SiteName: string = null with get, set
    
    [<YamlMember(Alias="favicon")>]
    member val FavIconPath: string = null with get, set
    
    [<YamlMember(Alias="headericon")>]
    member val HeaderIconPath: string = null with get, set
    
    [<YamlMember(Alias="copyright")>]
    member val CopyRight: string = null with get, set