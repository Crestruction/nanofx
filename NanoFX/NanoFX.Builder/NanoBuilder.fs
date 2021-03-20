module NanoFX.Builder.Builder

open System.IO
open NanoFX.Builder.Files
open NanoFX.Configure
open NanoFX

let build configPath outPath =
    NanoGraph.drawLogo ()
    let config: NanoConfig = loadNanoConfig configPath

    let keepDir dir = 
        let dir = DirectoryInfo dir
        if not dir.Exists then dir.Create ()
        dir

    let outputDir = keepDir outPath
    let getSubDir name = keepDir <| Path.Combine (outputDir.FullName, name)
    let audioDir = getSubDir "audio"
    let sourceDir = getSubDir "src"

    let baseDir =
        let dir = config.``base``
        if Path.IsPathRooted(dir) then dir
        else Path.Combine(Path.GetDirectoryName(configPath), dir)
    
    ///Audio files in directories.
    let audioBlocks =
        NanoLog.logBlock $"Start exporting audios..."
        config.resources.audiosources
        |> List.map (fun res ->
            let path = DirectoryInfo <| Path.Combine(baseDir, res.path)
            NanoLog.log $"Start exporting {res.title}..."
            NanoLog.log $"Audio directory at {path.FullName}..."
                    
            path.GetFiles()
            |> Seq.map (fun fi -> 
                NanoLog.logSuccess $"Audio found: {fi.FullName}..."
                NanoLog.log "|---Collecting File info..."
                        
                let audioFile = NanoFile.from fi
                        
                NanoLog.log $"|---Copy file to destination path..."
                NanoFile.organize audioDir audioFile
                NanoLog.logSuccess "|---File exporting finish..."
                audioFile)
            |> Seq.toList
            |> NanoAudioCatlog.from res)

    let sources =
        let recordFile fi fileType =     
            let src = {
                file = NanoFile.from fi
                srcType = fileType
            }
            NanoFile.organize sourceDir src.file
            src

        let org info filetype =
            List.choose (fun path ->
                let path = Path.Combine(baseDir, path)
                if path <> null then
                    let fi = FileInfo path
                    NanoLog.logSuccess $"{info} found: {fi.FullName}"
                    Some <| recordFile fi filetype
                else None)

        NanoLog.logBlock $"Start exporting javascripts..."
        let js = org "Javascript" JavaScript config.resources.javascripts
        NanoLog.logBlock $"Start exporting stylesheets..."
        let css = org "Stylesheet" StyleSheet config.resources.stylesheets
        NanoLog.logBlock $"Start exporting icons..."

        if config.site.favicon <> null then
            let favicon = Path.Combine(baseDir, config.site.favicon)
            File.Copy(favicon, Path.Combine(outPath, "favicon.ico"), true)
            NanoLog.logSuccess $"Favicon found: {favicon}"
            
        if config.site.name <> null then
            let headericon = Path.Combine(baseDir, config.site.headericon)
            File.Copy(headericon, Path.Combine(outPath, "icon.png"), true)
            NanoLog.logSuccess $"Header icon found: {headericon}"

        js @ css

    let html =        
        NanoLog.logBlock $"Start exporting HTML page..."
        
        let result =
            audioBlocks
            |> Seq.map (fun audioBlocks ->
                NanoFX.Builder.Html.ButtonSection.generate config audioBlocks ())
            |> NanoFX.Builder.NanoPage.build config sources

        NanoLog.logSuccess $"HTML build finished..."
        result
    
        
    File.WriteAllText(Path.Combine(outPath, "index.html"), html)
    NanoLog.logBlock $"Finished. Exit now..."
