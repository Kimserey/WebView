namespace WebView.WebApp.Bootstrap

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Client
open WebSharper.Resources

[<JavaScript; AutoOpen>]
module NavTabs =

    type NavTabs = {
        Tabs: NavTab list
        NavTabType: NavTabType
        IsJustified: bool
    } with
        static member Create(tabs) =
            { Tabs = tabs
              NavTabType = NavTabType.Normal
              IsJustified = false }
        
        static member Create(tabs, navTabType) =
            { Tabs = tabs
              NavTabType = navTabType
              IsJustified = false }
        
        static member RenderNav x =
            ulAttr [ attr.``class`` ("nav "
                                     + (if x.IsJustified then "nav-justified " else "")
                                     + (match x.NavTabType with 
                                        | Normal -> "nav-tabs" 
                                        | Pill Horizontal -> "nav-pills"
                                        | Pill Vertical -> "nav-pills nav-stacked")) ]
                   (x.Tabs |> List.map NavTab.RenderNavItem |> Seq.cast)
        
        static member RenderContent x =
            divAttr [ attr.``class`` "tab-content" ] (x.Tabs |> List.map NavTab.RenderContent |> Seq.cast)
        
        static member Render x =
            NavTabs.RenderNav x, NavTabs.RenderContent x

    and NavTab = {
        Id: string
        Title: string
        Content: Doc
        NavTabState: NavTabState 
    } with
        static member Create(id, title) =
            { Id = id
              Title = title
              Content = Doc.Empty
              NavTabState = NavTabState.Normal }
        
        static member Create(id, title, state) =
            { Id = id
              Title = title
              Content = Doc.Empty
              NavTabState = state }
        
        static member Create(id, title, content) =
            { Id = id
              Title = title
              Content = content
              NavTabState = NavTabState.Normal }
        
        static member Create(id, title, content, state) =
            { Id = id
              Title = title
              Content = content
              NavTabState = state }

        static member AddContent content (x: NavTab) =
            { x with  Content = content }

        static member SetState state (x: NavTab) =
            { x with NavTabState = state }
        
        static member RenderNavItem x =
            liAttr [ attr.role "presentation"
                     attr.``class`` (match x.NavTabState with
                                     | NavTabState.Normal -> ""
                                     | NavTabState.Active -> "active"
                                     | NavTabState.Disabled -> "disabled") ] 
                   [ (match x.NavTabState with
                      | NavTabState.Disabled -> Hyperlink.Create(Href "#", x.Title)
                      | _ ->  Hyperlink.Create(Href <| "#" + x.Id, x.Title)
                              |> Hyperlink.SetRole("tab")
                              |> Hyperlink.SetDataToggle("tab"))
                      |> Hyperlink.Render ]

        member x.RenderNavItem() = 
            NavTab.RenderNavItem x
        
        static member RenderContent x =
            divAttr [ attr.role "tabpanel"
                      attr.id x.Id
                      attr.``class`` (match x.NavTabState with NavTabState.Active -> "tab-pane fade in active" | _ -> "tab-content tab-pane fade") ]
                    [ x.Content ]
        
        member x.WithContent doc = 
            { x with Content = doc }
        
        member x.RenderContent() = 
            NavTab.RenderContent x
    
    and NavTabState =
        | Normal
        | Active
        | Disabled
    
    and NavTabType =
        | Normal
        | Pill of PillStack
    
    and PillStack =
        | Horizontal
        | Vertical