namespace NanoFX.Logger

open System

[<AbstractClass; Sealed>]
type NanoGraph private() =
    static member DrawLogo() =
        Console.WriteLine "======================================="
        Console.WriteLine "    _   __                  _______  __"
        Console.WriteLine "   / | / /___ _____  ____  / ____/ |/ /"
        Console.WriteLine "  /  |/ / __ `/ __ \/ __ \/ /_   |   / "
        Console.WriteLine " / /|  / /_/ / / / / /_/ / __/  /   |  "
        Console.WriteLine "/_/ |_/\__,_/_/ /_/\____/_/    /_/|_|  "
        Console.WriteLine "                                       "
        Console.WriteLine "======================================="
        Console.WriteLine "NanoFX button site generator"
        Console.WriteLine "Developed by Crestruction.org"
        Console.WriteLine "Opensourced under APACHE 2.0 LICENSE "
        Console.WriteLine "======================================="
