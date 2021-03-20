namespace NanoFX.Builder.Files

open NanoFX.Configure

type NanoAudioCatlog = {
    audios: NanoAudio list
    blockName: string
    blockId: string
    config: AudioConfig
}

module NanoAudioCatlog =
    let from config audios = {
        audios = audios
        blockId = config.id
        blockName = config.title
        config = config
    }
