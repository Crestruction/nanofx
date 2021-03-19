namespace NanoFX.Builder.Files

open NanoFX.Builder.Files.Internal

type NanoSource(Type: NanoSourceType) =
    inherit NanoFile()
    
    let mutable srcType = Type
    member this.Type with get() = srcType
    
    new() = NanoSource(NanoSourceType.Unknown)

    member this.ToHtml() =
        match Type with
        | NanoSourceType.StyleSheet ->
            $"<link rel=\"stylesheet\" href=\"./src/{this.HashedFileName}\">"
        | NanoSourceType.JavaScript ->
            $"<script src=\"./src/{this.HashedFileName}\"></script>"
        | _ -> failwith "Type should be JavaScript or StyleSheet"