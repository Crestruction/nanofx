namespace NanoFX.Configure.Internal

open System.Collections.Generic
open YamlDotNet.Serialization

[<AllowNullLiteral>]
type ResourceConfig () =
    
    [<YamlMember(Alias="audiosources")>]
    member val AudioSources: List<AudioConfig> = null with get, set
    
    [<YamlMember(Alias="stylesheets")>]
    member val StyleSheets: List<string> = null with get, set
    
    [<YamlMember(Alias="javascripts")>]
    member val JavaScripts: List<string> = null with get, set