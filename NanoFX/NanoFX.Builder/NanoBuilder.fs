namespace NanoFX.Builder

open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Text
open NanoFX.Builder.Files
open NanoFX.Builder.Files.Internal
open NanoFX.Builder.Html
open NanoFX.Configure

type NanoBuilder(configPath: string, outPath: string) as self=
       
    member val Config: NanoConfig = NanoConfig() with get, set
    
    ///Audio files in directories.
    member val AudioBlock: Dictionary<string, NanoAudioCatlog>
        = Dictionary<string, NanoAudioCatlog>() with get, set

    ///Souece code resources. Such as Css and Js.
    member val Sources: NanoSourceCatlog = NanoSourceCatlog() with get, set
    
    member this.OutPutDir with get() =
        let dir = DirectoryInfo(outPath)
        
        if not dir.Exists then
            dir.Create()             
        dir

    member this.AudioDir with get() =
        this.GetSubDir "audio"

    member this.SourceDir with get() =
        this.GetSubDir "src"
    
    member this.GetSubDir(name: string) =
        let dir = this.OutPutDir
        let subdir = DirectoryInfo(Path.Combine(dir.FullName, name))
        
        if not subdir.Exists then
            subdir.Create()           
        subdir

    ///Organize audio files.
    member this.OrganizeAudio() =
        if this.Config.Resources.AudioSources = null then
            raise <| Exception("Audio source configure cannot be empty.")
        
        let path = DirectoryInfo <| Path.Combine(this.Config.BasePath, this.Config.Resources.AudioSources)
                
        let dirs = path.GetDirectories().ToList()
        
        if dirs.Count < 1 then
            raise <| Exception("Audio sources not found.")
        
        for dir in dirs do
            let files = dir.GetFiles().ToList()
            let block = NanoAudioCatlog(BlockName = dir.Name)
            
            for fi in files do
                let audio = NanoAudio()
                audio.SetFrom(fi)
                block.Add(audio)
                
                audio.Organize this.AudioDir
            
            this.AudioBlock.Add(block.BlockName, block)
    
    member private this.RecordFile(dir: DirectoryInfo, fileType: NanoSourceType) =
        let files = dir.GetFiles().ToList()
            
        for fi in files do
            let src = NanoSource(fileType)
            src.SetFrom(fi)
            src.Organize(this.SourceDir)
            this.Sources.Add(src)
            
    
    ///Organize source code files.
    member this.OrganizeSource() =
        let jsPath = Path.Combine(this.Config.BasePath, this.Config.Resources.JavaScripts)
        if jsPath <> null then
            let path = DirectoryInfo jsPath
            this.RecordFile(path, NanoSourceType.JavaScript)
               
        let cssPath = Path.Combine(this.Config.BasePath, this.Config.Resources.StyleSheets)
        if cssPath <> null then
            let path = DirectoryInfo cssPath
            this.RecordFile(path, NanoSourceType.StyleSheet)
        
        let favicon = Path.Combine(this.Config.BasePath, this.Config.Site.FavIconPath)
        File.Copy(favicon, Path.Combine(outPath, "favicon.ico"), true)
        let headericon = Path.Combine(this.Config.BasePath, this.Config.Site.HeaderIconPath)
        File.Copy(headericon, Path.Combine(outPath, "icon.png"), true)
    
    member this.GenerateHtml() =
        let builder = StringBuilder()
        for kvp in this.AudioBlock do
            let block = ButtonSection(this.Config, kvp.Key, kvp.Value)
            builder.AppendLine(block.Parse()) |> ignore
         
        let page = NanoPage(this.Config, this.Sources)
        page.Build(builder.ToString())
    
    member this.Build() =
        Console.WriteLine("Start Build")
        self.Config.Read configPath |> ignore 
        this.OrganizeAudio()
        this.OrganizeSource()
        let html = this.GenerateHtml()
        File.WriteAllText(Path.Combine(outPath, "index.html"), html)