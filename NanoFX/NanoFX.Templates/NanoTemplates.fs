module NanoFX.Templates

open System.IO
open System.Reflection

let private getStream name =
    let asm = Assembly.GetExecutingAssembly ()
    asm.GetManifestResourceStream($"{asm.GetName().Name}.Resources.{name}")

let get name =
    use stream = getStream name
    use reader = new StreamReader(stream)
    reader.ReadToEnd()

let getBytes(name: string) =
    use stream = getStream name
    Array.init (int stream.Length) (ignore >> stream.ReadByte >> byte)
