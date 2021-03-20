namespace NanoFX.Builder.Files

open System
open System.IO
open System.Security.Cryptography

type NanoFile = {
    hashedFileName: string
    originFile: FileInfo
    originName: string
}

module NanoFile =
    let private hash (fileInfo: FileInfo) =
        fileInfo.FullName
        |> File.ReadAllBytes
        |> SHA1.HashData
        |> BitConverter.ToString
        |> fun x -> x.Replace ("-", "")

    let from file = {
        originFile = file
        hashedFileName = hash file + file.Extension
        originName = Path.GetFileNameWithoutExtension file.FullName
    }

    let organize (destDir: DirectoryInfo) nanoFile =
        let dest = Path.Combine (destDir.FullName, nanoFile.hashedFileName)
        if not <| File.Exists dest then
            File.Copy (nanoFile.originFile.FullName, dest)
