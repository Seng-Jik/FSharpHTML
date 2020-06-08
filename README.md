# FSharpHTML
The DSL for generating HTML Document.

[![NuGet Badge](http://img.shields.io/nuget/v/FSharpHTML.svg?style=flat)](https://www.nuget.org/packages/FSharpHTML/)


### Example
```fsharp
open FSharpHTML
open FSharpHTML.Elements

html [
    head [
        Meta (Charset "UTF-8")
        title %"Hello, world!"
    ]
    body [
        div [
            Text "<This is a test page>"
            Text "???"
        ]
        
        img [
            "src" %= "demo.png"
            "width" %= 100
            "height" %= 50
        ]
        
        a [ 
            "href" %= "example.com"
            Text "Click to example.com"
        ]
    ]
]
|> HTMLDocument
|> string
|> printfn "%s"
```

```html
<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title>Hello,&nbsp;world!</title>
    </head>
    <body>
        <div>
            &lt;This&nbsp;is&nbsp;a&nbsp;test&nbsp;page&gt;
            ???
        </div>
        <img
            src="demo.png"
            width="100"
            height="50" />
        <a href="example.com">Click&nbsp;to&nbsp;example.com</a>
    </body>
</html>
```
