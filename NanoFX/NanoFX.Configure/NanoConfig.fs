namespace NanoFX.Configure

open System.IO
open NanoFX.Configure.Internal
open YamlDotNet.Serialization

[<AllowNullLiteral>]
type NanoConfig() =
    
    [<YamlMember(Alias="base")>]
    member val BasePath: string = null with get, set
    
    [<YamlMember(Alias="site")>]
    member val Site: SiteConfig = null with get, set

    [<YamlMember(Alias="resources")>]
    member val Resources: ResourceConfig = null with get, set
    
    [<YamlMember(Alias="styles")>]
    member val Styles: StyleConfig = null with get, set
    
    member this.Read (path:string) : NanoConfig =
        let deserializer = Deserializer()
        let content = File.ReadAllText path
        
        let conf = deserializer.Deserialize<NanoConfig>(content)
        this.BasePath <- conf.BasePath;        
        this.Site <- conf.Site
        this.Resources <- conf.Resources
        this.Styles <- conf.Styles
        
        conf