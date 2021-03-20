namespace NanoFX.Commands

open System.IO
open Crestruction.Utilities.CommandLine
open NanoFX

[<Command("Create", Description = "Create a new nanofx project.")>]
type NanoCreateCommand() =
    inherit CommandTask()
    
    [<CommandOption("-o|--output", Description = "Project template output path.")>]
    member val private outputPath: string = null with get, set
    override this.Run() =
        let mutable path = "./"
        
        if this.outputPath <> null then
            path <- this.outputPath
                
        let dir = DirectoryInfo path
        
        let res = dir.CreateSubdirectory "res"
        let audio = res.CreateSubdirectory "audio"
        let src = res.CreateSubdirectory "src"
        
        let cssdir = src.CreateSubdirectory "css"
        let csstpl = Templates.get "nanofx.css"
        File.WriteAllText(Path.Combine(cssdir.FullName, "nanofx.css"), csstpl)
        
        let configtpl = Templates.get "config.yaml"
        File.WriteAllText(Path.Combine(path, "config.yaml"), configtpl)
        
        let nya = audio.CreateSubdirectory "nya"
        let audiotpl = Templates.getBytes "NyaNya.mp3"
        File.WriteAllBytes(Path.Combine(nya.FullName, "NyaNya.mp3"), audiotpl)
        
        let favicon = Templates.getBytes "icon.ico"
        File.WriteAllBytes(Path.Combine(res.FullName, "icon.ico"), favicon)
        
        let headericon = Templates.getBytes "icon.png"
        File.WriteAllBytes(Path.Combine(res.FullName, "icon.png"), headericon)
        
        NanoLog.logBlock "Project created..." 
        0