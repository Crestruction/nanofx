module NanoFX.Configure

open Legivel.Serialization

type AudioConfig = {
    id: string
    title: string
    path: string
}

type ResourceConfig = {
    audiosources: AudioConfig list
    stylesheets: string list
    javascripts: string list
}

type SiteConfig = {
    name: string
    favicon: string
    headericon: string
    copyright: string
}

type StyleConfig = {
    page: string list
    section: string list
    header: string list
    button: string list
}

type NanoConfig = {
    ``base``: string
    site: SiteConfig
    resources: ResourceConfig
    styles: StyleConfig
}

let loadNanoConfig = 
    System.IO.File.ReadAllText
    >> Deserialize<NanoConfig>
    >> List.exactlyOne
    >> function
    | Success x -> x.Data
    | Error e -> 
        failwithf "Can not load yaml, message: \n%A" e

let private getHtmlClass = List.map ((+) " ") >> List.fold (+) ""
let getButtonClass style = getHtmlClass style.button
let getSectionClass style = getHtmlClass style.section
let getHeaderClass style = getHtmlClass style.header
