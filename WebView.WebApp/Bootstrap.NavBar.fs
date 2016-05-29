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

    type NavBar = {
        Brand: NavBrand
        LeftMenu: NavBarMenu
        RightMenu: NavBarMenu 
    } with
        static member Render (activeLink: View<string>) (x: NavBar) =
            navAttr [ attr.``class`` "navbar navbar-default navbar-fixed-top" ] 
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
                                    [ x.LeftMenu |> NavBarMenu.Render activeLink
                                      x.RightMenu |> NavBarMenu.Render activeLink ] ] ]
    
    and NavBrand = NavBrand of Doc
        with
            static member Render (NavBrand doc) = doc

    and NavBarMenu = {
        Links: Hyperlink list
        Side: NavBarMenuSide
    } with
        static member Render (activeLink: View<string>) (x: NavBarMenu) = 
            let links = 
                activeLink |> Doc.BindView(fun active -> x.Links |> List.map (fun link -> liAttr [ attr.style "display: none;"; attr.``class`` (if link.Id |> Option.isSome && link.Id.Value = active then "active" else "")  ] [ link |> Hyperlink.Render ] :> Doc) |> Doc.Concat) 
            
            ulAttr [ attr.``class`` (match x.Side with Left -> "nav navbar-nav" | Right -> "nav navbar-nav navbar-right") ] [ links ]
                

    and NavBarMenuSide = Left | Right
