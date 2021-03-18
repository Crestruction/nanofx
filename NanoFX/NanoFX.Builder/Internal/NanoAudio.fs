namespace NanoFX.Builder.Internal

open System
open System.IO
open System.Security.Cryptography

type NanoAudio() =
    member val FileName: string = null with get, set
    member val OriginFile: FileInfo = null with get, set
    member val AudioName: string = null with get, set
    member this.Set(file: FileInfo) =
        let hash = this.Hash(file)
        this.FileName <- hash + file.Extension
        this.OriginFile <- file
        this.AudioName <- Path.GetFileNameWithoutExtension file.FullName
        
    member this.Organize(destDir: DirectoryInfo) =
        let dest = Path.Combine(destDir.FullName, this.FileName)
        if not (File.Exists dest) then
            File.Copy(this.OriginFile.FullName, dest)
    
    member private this.Hash(file: FileInfo) =
        let hash = SHA1.Create()
        let stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read)
        let bytes = hash.ComputeHash(stream)

        stream.Close |> ignore
        stream.Dispose()

        BitConverter.ToString(bytes).Replace("-", String.Empty)