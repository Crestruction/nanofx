namespace NanoFX.Builder.Files

open System.Collections.Generic
open NanoFX.Builder.Files.Internal

type NanoSourceCatlog() =
    inherit List<NanoSource>()
    
    member this.Get(fileType: NanoSourceType) =
        this.FindAll(fun item -> item.Type = fileType)