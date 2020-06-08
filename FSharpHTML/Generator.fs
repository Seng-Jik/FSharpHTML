module private FSharpHTML.Generator
open System.Text

let private indent indentLevel (sb:StringBuilder) =
    for _ in 0..indentLevel*4-1 do
        sb.Append(' ') |> ignore

let private generateAttribute (sb:StringBuilder) (key:string) (value:string) =
    sb
        .Append(key)
        .Append('=')
        .Append('\"')
        .Append(value)
        .Append('\"')
    |> ignore


let private printNewLineWhenAttributesMoreThan = 2
let private generateAttributes indentLevel (sb:StringBuilder) attr =
    let attr =
        attr
        |> List.map (function
        | Attribute (key,value) -> key,value
        | _ -> invalidArg "attr" "The element type of attr must be Attributes.")
    if List.length attr <= printNewLineWhenAttributesMoreThan then
        attr
        |> List.iter (fun (key,value) ->
            sb.Append(' ') |> ignore
            generateAttribute sb key value)
    else
        attr
        |> List.iter (fun (key,value) ->
            sb.AppendLine() |> ignore
            indent indentLevel sb
            generateAttribute sb key value)

let private generateText (sb:StringBuilder) (text:string) =
    text
        .Replace("\"","&quot;")
        .Replace("&","&amp;")
        .Replace("<","&lt;")
        .Replace(">","&gt;")
        .Replace(" ","&nbsp;")
    |> sb.Append
    |> ignore

let rec generateContent indentLevel (sb:StringBuilder) = function
| Element e -> generateElement indentLevel sb e
| Meta metadata -> 
    indent indentLevel sb
    sb.Append("<meta ") |> ignore
    match metadata with
    | Charset x -> generateAttribute sb "charset" x
    | Property (key,value) -> 
        generateAttribute sb "name" key
        sb.Append(' ') |> ignore
        generateAttribute sb "value" value
    | HTTPEquiv (key,value) ->
        generateAttribute sb "http-equiv" key
        sb.Append(' ') |> ignore
        generateAttribute sb "value" value
    sb.AppendLine(">") |> ignore
| Text text -> 
    indent indentLevel sb
    generateText sb text
    sb.AppendLine() |> ignore
| _ -> invalidArg "HTMLContent" "HTMLContent must not be Attribute."

and private generateElement indentLevel (sb:StringBuilder) {Tag = tag; Children = children} =
    indent indentLevel sb
    sb.Append('<').Append(tag) |> ignore

    let attributes, content = 
        children
        |> List.partition (function
        | Attribute _ -> true
        | _ -> false)

    generateAttributes (indentLevel + 1) sb attributes
    if List.isEmpty content then
        sb.AppendLine(" />") |> ignore
    else
        let contentIndent =
            if List.length attributes <= printNewLineWhenAttributesMoreThan then indentLevel + 1
            else indentLevel + 2

        sb.Append('>') |> ignore

        match content with
        | Text x :: [] -> generateText sb x
        | content ->
            content 
            |> List.iter (
                sb.AppendLine() 
                |> generateContent contentIndent)
            indent indentLevel sb
        sb
            .Append("</")
            .Append(tag)
            .Append('>')
            .AppendLine()
        |> ignore
