namespace NanoFX.Configure.Internal

open YamlDotNet.Serialization

[<AllowNullLiteral>]
type ResourceConfig () =
    
    [<YamlMember(Alias="audiosources")>]
    member val AudioSources: string = null with get, set
    
    [<YamlMember(Alias="stylesheets")>]
    member val StyleSheets: string = null with get, set
    
    [<YamlMember(Alias="javascripts")>]
    member val JavaScripts: string = null with get, set