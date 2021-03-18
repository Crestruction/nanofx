namespace NanoFX.Builder.Files

open System.Collections.Generic

type NanoAudioCatlog() =
    inherit List<NanoAudio>()
    member val BlockName: string = null with get, set
    