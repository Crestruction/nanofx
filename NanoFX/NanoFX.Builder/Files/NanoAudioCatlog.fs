namespace NanoFX.Builder.Files

open System.Collections.Generic
open NanoFX.Configure.Internal

type NanoAudioCatlog(config: AudioConfig) =
    inherit List<NanoAudio>()
    
    member val BlockName = config.Title with get
    member val BlockId = config.Id with get
    member val Config = config with get