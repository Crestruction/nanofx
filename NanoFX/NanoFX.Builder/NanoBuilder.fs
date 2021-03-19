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
open NanoFX.Logger

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
        NanoLog.Log ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
        NanoLog.Log $"Start exporting audios..."
        NanoLog.Log ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"

        if this.Config.Resources.AudioSources = null then
            NanoLog.LogError("Audio source configure cannot be empty.")
        
        for res in this.Config.Resources.AudioSources do
            NanoLog.Log "================================"
            NanoLog.Log $"Start exporting {res.Title}..."
            let path = DirectoryInfo <| Path.Combine(this.Config.BasePath, res.Path)
            NanoLog.Log $"Audio directory at {path.FullName}..."
            NanoLog.Log "================================"
            
            let files = path.GetFiles().ToList()
            let block = NanoAudioCatlog(res)
            
            for fi in files do
                
                try
                    let audio = NanoAudio()
                    
                    NanoLog.Log($"Audio found: {fi.FullName}...", ConsoleColor.Green)
                    NanoLog.Log "|---Collecting File info..."
                    
                    audio.SetFrom(fi)
                    block.Add(audio)
                    
                    NanoLog.Log $"|---Copy file to destination path..."
                    audio.Organize this.AudioDir
                    NanoLog.Log("|---File exporting finish...", ConsoleColor.Green)
                with
                | :? Exception -> NanoLog.LogError "|---Exporting failed..."
            
            this.AudioBlock.Add(block.BlockName, block)
    
    member private this.RecordFile(fi: FileInfo, fileType: NanoSourceType) =     
        let src = NanoSource(fileType)
        src.SetFrom(fi)
        src.Organize(this.SourceDir)
        this.Sources.Add(src)

    ///Organize source code files.
    member this.OrganizeSource() =
        NanoLog.Log ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
        NanoLog.Log $"Start exporting javascripts..."
        NanoLog.Log ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
        
        for path in this.Config.Resources.JavaScripts do
            let jsPath = Path.Combine(this.Config.BasePath, path)
            if jsPath <> null then
                let fi = FileInfo jsPath
                this.RecordFile(fi, NanoSourceType.JavaScript)
                NanoLog.Log($"Javascript found: {fi.FullName}", ConsoleColor.Green)
        
        NanoLog.Log ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
        NanoLog.Log $"Start exporting stylesheets..."
        NanoLog.Log ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
        
        for path in this.Config.Resources.StyleSheets do
            let cssPath = Path.Combine(this.Config.BasePath, path)
            if cssPath <> null then
                let fi = FileInfo cssPath
                this.RecordFile(fi, NanoSourceType.StyleSheet)
                NanoLog.Log($"Stylesheet found: {fi.FullName}", ConsoleColor.Green)
        
        NanoLog.Log ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
        NanoLog.Log $"Start exporting icons..."
        NanoLog.Log ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"

        if this.Config.Site.FavIconPath <> null then
            let favicon = Path.Combine(this.Config.BasePath, this.Config.Site.FavIconPath)
            File.Copy(favicon, Path.Combine(outPath, "favicon.ico"), true)
            NanoLog.Log($"Favicon found: {favicon}", ConsoleColor.Green)
            
        if this.Config.Site.HeaderIconPath <> null then
            let headericon = Path.Combine(this.Config.BasePath, this.Config.Site.HeaderIconPath)
            File.Copy(headericon, Path.Combine(outPath, "icon.png"), true)
            NanoLog.Log($"Header icon found: {headericon}", ConsoleColor.Green)
    
    member this.GenerateHtml() =
        let builder = StringBuilder()
        for kvp in this.AudioBlock do
            let block = ButtonSection(this.Config, kvp.Value)
            builder.AppendLine(block.Parse()) |> ignore
        
        NanoLog.Log ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
        NanoLog.Log $"Start exporting HTML page..."
        NanoLog.Log ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
        let page = NanoPage(this.Config, this.Sources)
        let result = page.Build(builder.ToString())
        NanoLog.Log($"HTML build finished...", ConsoleColor.Green)
        result
    
    member this.Build() =
        NanoGraph.DrawLogo()
        self.Config.Read configPath |> ignore 
        this.OrganizeAudio()
        this.OrganizeSource()
        let html = this.GenerateHtml()
        File.WriteAllText(Path.Combine(outPath, "index.html"), html)
        
        NanoLog.Log "==================================="
        NanoLog.Log $"Finished. Exit now..."
        NanoLog.Log "==================================="