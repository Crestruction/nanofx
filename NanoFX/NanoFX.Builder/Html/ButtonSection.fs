module NanoFX.Builder.Html.ButtonSection

open NanoFX.Builder.Files
open NanoFX.Configure

open FSharpHTML
open FSharpHTML.Elements

let generate (config: NanoConfig) (audios: NanoAudioCatlog) : HtmlGenerator = fun () ->
    let blockClass = getSectionClass config.styles
    let buttonClass = getButtonClass config.styles
    let headerClass = getHeaderClass config.styles

    div [
        "class" %= $"nanofx-block nanofx-block-{audios.blockId} {blockClass}"
        h3 [ "class" %= $"nanofx-block-header {headerClass}"; Text audios.blockName ]
        div [ 
            "class" %= $"nanofx-container nanofx-container-{audios.blockId}" 

            yield! 
                audios.audios |> Seq.map (fun audio ->
                    button [
                        "class" %= $"nanofx-button nanofx-button-{audios.blockId} {buttonClass}"
                        "onClick" %= $"nanoPlay('{audio.hashedFileName}')"
                        Text audio.originName
                    ]
                )
        ]
    ]

