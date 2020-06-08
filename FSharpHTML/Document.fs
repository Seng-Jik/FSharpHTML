namespace FSharpHTML

type HTMLDocument = HTMLDocument of HTMLContent with
override x.ToString () =
    let sb = System.Text.StringBuilder ()
    let (HTMLDocument rootElement) = x
    sb.AppendLine("<!DOCTYPE html>") |> ignore
    Generator.generateContent 0 sb rootElement
    string sb

