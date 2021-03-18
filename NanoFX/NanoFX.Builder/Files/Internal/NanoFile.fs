namespace NanoFX.Builder.Files.Internal

open System
open System.IO
open System.Security.Cryptography

type NanoFile() =
    member val HashedFileName: string = null with get, set
    member val OriginFile: FileInfo = null with get, set
    member val OriginName: string = null with get, set
    
    member this.SetFrom(file: FileInfo) =
        let hash = this.Hash(file)
        this.HashedFileName <- hash + file.Extension
        this.OriginFile <- file
        this.OriginName <- Path.GetFileNameWithoutExtension file.FullName
        
    member this.Hash(file: FileInfo) =
        let hash = SHA1.Create()
        use stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read)
        let bytes = hash.ComputeHash(stream)

        stream.Close()

        BitConverter.ToString(bytes).Replace("-", String.Empty)
        
    member this.Organize(destDir: DirectoryInfo) =
        let dest = Path.Combine(destDir.FullName, this.HashedFileName)
        if not (File.Exists dest) then
            File.Copy(this.OriginFile.FullName, dest)