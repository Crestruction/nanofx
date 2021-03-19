namespace NanoFX.Logger

open System

[<AbstractClass; Sealed>]
type NanoLog private() =
    static member Log(content: string, color) =
        let c = Console.ForegroundColor
        Console.ForegroundColor <- color
        Console.WriteLine content
        Console.ForegroundColor <- c

    static member LogError(content: string) =
        NanoLog.Log(content, ConsoleColor.Red)
        raise <| Exception content
    static member LogWarning(content: string) =
        NanoLog.Log(content, ConsoleColor.Yellow)

    static member Log(content: string) =
        NanoLog.Log(content, Console.ForegroundColor)

    static member LogBlock(content: string) =
        NanoLog.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>", ConsoleColor.Blue)
        NanoLog.Log(content, ConsoleColor.Blue)
        NanoLog.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>", ConsoleColor.Blue)
