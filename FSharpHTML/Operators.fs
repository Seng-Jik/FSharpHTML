[<AutoOpen>]
module FSharpHTML.Operators

let (~%) text = [Text (string text)]

let (%=) attribute value = Attribute (attribute, (string value))

