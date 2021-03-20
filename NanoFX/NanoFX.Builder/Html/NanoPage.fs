module NanoFX.Builder.NanoPage

open NanoFX.Builder.Files
open NanoFX.Configure
open NanoFX
open FSharpHTML
open type System.Environment

let build (config: NanoConfig) (sources: NanoSourceCatlog) (content: HTMLContent seq) =
    let tpl = Templates.get "nanofx.html"

    let html2str: HTMLContent seq -> string = 
        Seq.map (HTMLDocument >> string >> (+) NewLine) >> Seq.fold (+) ""
        >> fun x -> x.Replace ("<!DOCTYPE html>", "")
        
    let get filetype =
        query {
            for i in sources do
            where (i.srcType = filetype)
            select (html2str [NanoSource.toHtml i])
        }
        |> Seq.fold (+) ""

    let css = get NanoSourceType.StyleSheet
    let js = get NanoSourceType.JavaScript

    seq {
        "${SITE_NAME}", config.site.name
        "${CSS_IMPORT}", css
        "${COPYRIGHT}", config.site.copyright
        "${JS_IMPORT}", js
        "${NANOFX_CONTENT}", html2str content
    }
    |> Seq.fold (fun (html: string) (a, b) -> html.Replace (a, b)) tpl
