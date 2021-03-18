namespace NanoFX.Builder.Html

open System.Text
open NanoFX.Configure

type BlockDivision (styles: string, header: string) =
    inherit HtmlElement()
    
    override this.Parse() =
        let builder = StringBuilder()
        
        builder.AppendLine "<div>" |> ignore
                   
        
        builder.AppendLine "</div>" |> ignore
        
        builder.ToString()