open System
open System.Reflection
open System.Text
open Microsoft.Extensions.CommandLineUtils
open Crestruction.Utilities.CommandLine

[<EntryPoint>]
let main argv =
    Console.OutputEncoding <- Encoding.UTF8
    
    let app = CommandLineApplication()
    app.Name <- "NanoFX"
    app.Description <- "A vtuber's button site generator."
    
    app.HelpOption "-?|-h|-help" |> ignore 
    app.VersionOption ("-v|--version", "NanoFX 1.0.0") |> ignore
    
    app.OnExecute(
        fun() ->
            app.ShowHelp |> ignore
            0
    )
    
    app.ReflectFrom <| Assembly.GetCallingAssembly()
    
    let mutable exit_code = 0
    exit_code <- app.Execute(argv)
    
    Console.Read |> ignore
    exit_code
     