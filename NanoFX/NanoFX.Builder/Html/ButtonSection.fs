namespace NanoFX.Builder.Html

open System.Text
open NanoFX.Builder.Files
open NanoFX.Configure

type ButtonSection (config: NanoConfig, header: string, audios: NanoAudioCatlog) =
    inherit HtmlElement()
    
    override this.Parse() =
        let builder = StringBuilder()
        
        let blockClass = config.Styles.GetSectionClass()
        let buttonClass = config.Styles.GetButtonClass()
        
        builder.AppendLine $"<div class=\"nanofx-block {blockClass}\">" |> ignore
        
        builder.AppendLine $"\t<h3 class=\"nanofx-header\">{header}</h3>" |> ignore
        
        builder.AppendLine $"\t<div class=\"nanofx-button-container\">" |> ignore
        
        for audio in audios do
            builder.AppendLine $"\t\t<button class=\"nanofx-button {buttonClass}\" onClick=\"nanoPlay('{audio.HashedFileName}')\">{audio.OriginName}</button>" |> ignore
        
        builder.AppendLine "\t</div>" |> ignore

        builder.AppendLine "</div>" |> ignore
        
        builder.ToString()