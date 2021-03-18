namespace NanoFX.Builder.Html

open System.Text
open NanoFX.Builder.Files
open NanoFX.Builder.Files.Internal
open NanoFX.Configure
open NanoFx.Templates

type NanoPage(config: NanoConfig, sources: NanoSourceCatlog) =
    member this.GetTemplate() =
        NanoTemplates().Get("nanofx.html")
    
    member this.Build(content: string) =
        let tpl = this.GetTemplate()
        
        let css = StringBuilder()
        for src in sources.Get(NanoSourceType.StyleSheet) do
            css.AppendLine(src.ToHtml()) |> ignore
        
        let js = StringBuilder()
        for src in sources.Get(NanoSourceType.JavaScript) do
            js.AppendLine(src.ToHtml()) |> ignore
        
        tpl.Replace("${SITE_NAME}", config.Site.SiteName)
            .Replace("${CSS_IMPORT}", css.ToString())
            .Replace("${COPYRIGHT}", config.Site.CopyRight)
            .Replace("${JS_IMPORT}", js.ToString())
            .Replace("${NANOFX_CONTENT}", content)