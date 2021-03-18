namespace NanoFX.Builder.Internal

open System.Collections.Generic

type NanoBlock() =
    inherit List<NanoAudio>()
    member val BlockName: string = null with get, set
    