namespace NanoFX.Builder.Files

open FSharpHTML
open FSharpHTML.Elements

type NanoSource = {
    file: NanoFile
    srcType: NanoSourceType
}

module NanoSource =
    let toHtml source =
        match source.srcType with
        | NanoSourceType.StyleSheet ->
            link [
                "rel" %= "stylesheet"
                "href" %= $"./src/{source.file.hashedFileName}"
            ]
        | NanoSourceType.JavaScript ->
            script [
                "src" %= $"./src/{source.file.hashedFileName}";
                Text ""
            ]
        | _ -> failwith "Type should be JavaScript or StyleSheet"

