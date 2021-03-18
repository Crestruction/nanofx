namespace NanoFx.Templates

open System.IO

type NanoTemplates() =
    member this.Get(name: string) =
        let asm = this.GetType().Assembly
        use stream = asm.GetManifestResourceStream($"{asm.GetName().Name}.Resources.{name}")
        use reader = new StreamReader(stream)
        
        reader.ReadToEnd()