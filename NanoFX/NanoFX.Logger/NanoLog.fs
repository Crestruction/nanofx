module NanoFX.NanoLog

open System

let private logc color content =
    let c = Console.ForegroundColor
    Console.ForegroundColor <- color
    printfn "%s" content
    Console.ForegroundColor <- c

let logError = logc ConsoleColor.Red
let logWarning = logc ConsoleColor.Yellow
let logSuccess = logc ConsoleColor.Green
let log = logc Console.ForegroundColor
let logBlock content = 
    let logb = logc ConsoleColor.Blue 
    logb ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
    logb content
    logb ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
