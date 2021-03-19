namespace NanoFX.Builder.Html

open System.Text
open NanoFX.Builder.Files
open NanoFX.Configure

type ButtonSection (config: NanoConfig, audios: NanoAudioCatlog) =
    inherit HtmlElement()
    
    override this.Parse() =
        let builder = StringBuilder()
        
        let blockClass = config.Styles.GetSectionClass()
        let buttonClass = config.Styles.GetButtonClass()
        let headerClass = config.Styles.GetHeaderClass()
        
        builder.AppendLine $"<div class=\"nanofx-block nanofx-block-{audios.BlockId} {blockClass}\">" |> ignore
        
        builder.AppendLine $"\t<h3 class=\"nanofx-block-header {headerClass}\">{audios.BlockName}</h3>" |> ignore
        
        builder.AppendLine $"\t<div class=\"nanofx-container nanofx-container-{audios.BlockId}\">" |> ignore
        
        for audio in audios do
            builder.AppendLine $"\t\t<button class=\"nanofx-button nanofx-button-{audios.BlockId} {buttonClass}\" onClick=\"nanoPlay('{audio.HashedFileName}')\">{audio.OriginName}</button>" |> ignore
        
        builder.AppendLine "\t</div>" |> ignore

        builder.AppendLine "</div>" |> ignore
        
        builder.ToString()