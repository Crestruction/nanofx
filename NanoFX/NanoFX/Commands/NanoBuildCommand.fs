namespace NanoFX.Commands

open Crestruction.Utilities.CommandLine
open NanoFX.Builder

[<Command("build", Description = "Build nanofx static site.")>]
type NanoBuildCommand () =
    inherit CommandTask ()
    
    [<CommandOption("-o|--output", Description = "Site output path.")>]
    member val private outputPath: string = null with get, set
   
    [<CommandOption("-c|--config", Description = "Configure yaml file path.")>]
    member val private configPath: string = null with get, set
    
    override this.Run() =
        Builder.build this.configPath this.outputPath
        0