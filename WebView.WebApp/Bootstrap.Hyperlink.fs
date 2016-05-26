namespace WebView.WebApp.Bootstrap

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Client
open WebSharper.Resources

[<JavaScript; AutoOpen>]
module Hyperlink =

    type Hyperlink = {
        Action: HyperlinkAction
        Content: HyperlinkContent
        AriaLabel: string option
        Role: string option
        DataToggle: string option
        Id: string option
        CssClass: string list
        Target: string option
    } with
        static member Create action =
            { Action = action
              Content = Content Doc.Empty
              AriaLabel = None
              Role = None
              DataToggle = None
              Id = None
              CssClass = []
              Target = None }

        static member Create(action, content) =
            Hyperlink.Create action
            |> Hyperlink.AddContent content

        static member Create(action, content) =
            Hyperlink.Create action
            |> Hyperlink.AddTextContent content

        static member Render x =
            aAttr [ yield! (match x.Action with 
                            | Href href -> [ attr.href href ] 
                            | Action action -> [ attr.href "#"
                                                 on.click(fun _ ev ->
                                                       ev.PreventDefault()
                                                       action()) ]
                            | Mailto (email, subject) -> [ attr.href (sprintf "mailto:%s?subject=%s" email subject) ])
                    if x.CssClass <> [] then yield attr.``class`` (x.CssClass |> String.concat " ")

                    let getOrEmpty (opt: Attr option) = match opt with Some x -> x | None -> Attr.Empty
                    yield x.AriaLabel  |> Option.map (fun label -> attr.ariaLabel label ) |> getOrEmpty
                    yield x.Role       |> Option.map (fun role ->  attr.role role       ) |> getOrEmpty
                    yield x.DataToggle |> Option.map (fun tog ->   attr.dataToggle tog  ) |> getOrEmpty
                    yield x.Id         |> Option.map (fun i ->     attr.id i            ) |> getOrEmpty
                    yield x.Target     |> Option.map (fun i ->     attr.target i        ) |> getOrEmpty ] 
                  [ (match x.Content with
                     | Content doc -> doc
                     | Text txt -> text txt) ]

        member x.Render() = 
            Hyperlink.Render x
        
        member x.WithAriaLabel label = 
            { x with AriaLabel = Some label }
        
        member x.WithRole role = 
            { x with Role = Some role }
        
        member x.WithDataToggle toggle = 
            { x with DataToggle = Some toggle }
        
        static member AddId id (x: Hyperlink) = 
            { x with Id = Some id }
        
        static member AddContent content (x: Hyperlink) = 
            { x with Content = Content content }
        
        static member AddTextContent content (x: Hyperlink) = 
            { x with Content = Text content }
        
        static member AddClasses cls x = 
            { x with CssClass = x.CssClass @ cls |> List.distinct }

        static member AddTarget target x =
            { x with Target = Some target }
    
    and HyperlinkAction =
        | Href of string
        | Mailto of email:string * subject:string
        | Action of (unit -> unit)
    
    and HyperlinkContent =
        | Content of Doc
        | Text of string