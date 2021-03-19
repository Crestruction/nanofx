namespace NanoFx.Templates

open System.IO

type NanoTemplates() =
    member this.Get(name: string) =
        let asm = this.GetType().Assembly
        use stream = asm.GetManifestResourceStream($"{asm.GetName().Name}.Resources.{name}")
        use reader = new StreamReader(stream)
        
        reader.ReadToEnd()
        
    member this.GetBytes(name: string) =
        let asm = this.GetType().Assembly
        use stream = asm.GetManifestResourceStream($"{asm.GetName().Name}.Resources.{name}")
        let length = int32(stream.Length)
        let mutable bytes = Array.zeroCreate length
        stream.Read(bytes, 0, length) |> ignore
        
        bytes