namespace NanoFX.Builder.Html

open System.Text
open NanoFX.Builder.Internal

type ButtonItem(styles: string, audio: NanoAudio) =
    inherit HtmlElement()
    
    override this.Parse() =
        let builder = StringBuilder()
        builder.AppendFormat("<button class=\"nanofx-button {0} \" onClick=\"nanoPlay('{1}')\">{2}</button>",
                             styles, audio.FileName, audio.AudioName) |>ignore
        
        builder.ToString()