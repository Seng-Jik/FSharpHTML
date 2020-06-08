namespace FSharpHTML

type HTMLMetadata = 
| Charset of string
| HTTPEquiv of httpEquiv:string * content:string
| Property of name:string * content:string

type HTMLElement = {
    Tag : string
    Children : HTMLContent list
}

and HTMLContent = 
| Attribute of attribute:string * value:string
| Element of HTMLElement
| Meta of HTMLMetadata
| Text of string

