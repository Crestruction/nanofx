namespace NanoFX.Commands

open System
open Crestruction.Utilities.CommandLine
open NanoFX.Builder
open NanoFX.Configure

[<Command("build", Description = "Build nanofx static site.")>]
type NanoBuildCommand () =
    inherit CommandTask ()
    
    [<CommandOption("-o|--output", Description = "Site output path.")>]
    member val private outputPath: string = null with get, set
   
    [<CommandOption("-c|--config", Description = "Configure yaml file path.")>]
    member val private configPath: string = null with get, set
    override this.Run() =
        let builder = NanoBuilder(this.configPath, this.outputPath)

        Console.WriteLine "Start Build"
        builder.Build |> ignore
        0