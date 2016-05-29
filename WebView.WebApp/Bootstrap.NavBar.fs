namespace WebView.WebApp.Bootstrap

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Client
open WebSharper.Resources

[<JavaScript; AutoOpen>]
module NavBar =
    
    [<Direct "$('.navbar-collapse').collapse('hide');">]
    let collapse() = X<unit> 

    type NavBar<'Endpoint> = {
        Brand: NavBrand
        LeftMenu: NavBarMenu<'Endpoint>
        RightMenu: NavBarMenu<'Endpoint>
    } with
        static member Create brand =
            { Brand = brand
              LeftMenu = NavBarMenu<'Endpoint>.Create Left
              RightMenu = NavBarMenu<'Endpoint>.Create Right }

        static member SetLeftMenu menu x =
            { x with LeftMenu = menu }

        static member SetRightMenu menu x =
            { x with RightMenu = menu }

        static member Render activeLink x =
            navAttr [ attr.``class`` "navbar navbar-default" ] 
                    [ divAttr [ attr.``class`` "container-fluid" ] 
                              [ divAttr 
                                    [ attr.``class`` "navbar-header" ] 
                                    [ buttonAttr [ attr.``class`` "navbar-toggle collapsed"
                                                   attr.dataToggle "collapse"
                                                   attr.dataTarget "#menu"
                                                   attr.ariaExpanded "false" ] 
                                                 [ spanAttr [ attr.``class`` "sr-only" ]  []
                                                   spanAttr [ attr.``class`` "icon-bar" ] []
                                                   spanAttr [ attr.``class`` "icon-bar" ] []
                                                   spanAttr [ attr.``class`` "icon-bar" ] [] ]
                                      x.Brand |> NavBrand.Render ] 
                                divAttr 
                                    [ attr.``class`` "collapse navbar-collapse"; attr.id "menu" ]
                                    [ x.LeftMenu |> NavBarMenu<'Endpoint>.Render activeLink
                                      x.RightMenu |> NavBarMenu<'Endpoint>.Render activeLink ] ] ]
    
    and NavBrand = {
        Action: unit -> unit
        Content: Doc
    } with
        static member Create action content =
            { Action = action; Content = content }
        
        static member Render x =
            Hyperlink.Create (Action (fun () -> x.Action(); collapse()))
            |> Hyperlink.SetClasses [ "navbar-brand" ]
            |> Hyperlink.SetContent x.Content
            |> Hyperlink.Render

    and NavBarMenu<'Endpoint> = {
        Links: Hyperlink list
        Side: NavBarMenuSide
        IsActiveLink: Hyperlink -> 'Endpoint -> bool
    } with
        static member Create side = 
            { Links = []
              Side = side
              IsActiveLink = fun _ _ -> false }

        static member AddLink (link: Hyperlink) x =
            { x with Links = x.Links @ [ (match link.Action with Action act -> { link with Action = Action (fun () -> act(); collapse()) } | _ -> link) ] }
            
        static member AddLinks links x =
            (x, links) ||> List.fold (fun x link -> x |> NavBarMenu<_>.AddLink link)

        static member IsActive predicate x =
            { x with IsActiveLink = predicate }

        static member Render activeLink x = 
            let links = 
                activeLink |> Doc.BindView(fun active -> x.Links |> List.map (fun link -> liAttr [ attr.``class`` (if x.IsActiveLink link active then "active" else "")   ] [ link |> Hyperlink.Render ] :> Doc) |> Doc.Concat) 
            
            ulAttr [ attr.``class`` (match x.Side with Left -> "nav navbar-nav" | Right -> "nav navbar-nav navbar-right") ] [ links ]
                

    and NavBarMenuSide = Left | Right
