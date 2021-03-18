namespace NanoFX.Builder

open System
open System.Collections.Generic
open System.IO
open System.Linq
open NanoFX.Builder.Internal
open NanoFX.Configure

type NanoBuilder(configPath: string, outPath: string) as self=
       
    member val Config: NanoConfig = NanoConfig() with get, set
    
    member val Block: Dictionary<string, NanoBlock>
        = Dictionary<string, NanoBlock>() with get, set
        
    member this.OutPutDir with get() =
        let dir = DirectoryInfo(outPath)
        
        if not dir.Exists then
            dir.Create()             
        dir

    member this.AudioDir with get() =
        this.GetSubDir <| "audio" 

    member this.SourceDir with get() =
        this.GetSubDir <| "src" 
    
    member this.GetSubDir(name: string) =
        let dir = this.OutPutDir
        let subdir = DirectoryInfo(Path.Combine(dir.FullName, name))
        
        if not subdir.Exists then
            subdir.Create()           
        subdir

    member this.OrganizeAudio() =
        if this.Config.Resources.AudioSources = null then
            raise <| Exception("Audio source configure cannot be empty.")
        
        let path = DirectoryInfo this.Config.Resources.AudioSources
                
        let dirs = path.GetDirectories().ToList()
        
        if dirs.Count < 1 then
            raise <| Exception("Audio sources not found.")
        
        for dir in dirs do
            let files = dir.GetFiles().ToList()
            let block = NanoBlock(BlockName = dir.Name)
            
            for fi in files do
                let audio = NanoAudio()
                audio.Set fi
                block.Add audio
                
                audio.Organize this.AudioDir
            
            this.Block.Add(block.BlockName, block)
    
    member this.BuildHtml() =
        1
       
    member this.Build() =
        self.Config.Read configPath |> ignore 
        this.OrganizeAudio
