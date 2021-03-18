namespace NanoFX.Builder.Html

open NanoFX.Configure

[<AbstractClass>]
type HtmlElement ()=
    abstract member Parse: unit -> string