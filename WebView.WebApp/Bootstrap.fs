namespace WebView.WebApp.Bootstrap

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Client
open WebSharper.Resources

[<JavaScript; AutoOpen>]
module Common =
    type attr
    with
        static member role x = Attr.Create "role" "presentation"
        static member dataToggle x = Attr.Create "data-toggle" x
        static member dataTarget x = Attr.Create "data-target" x
        static member ariaControls x = Attr.Create "aria-controls" x
        static member ariaLabel x = Attr.Create "aria-label" x
        static member ariaHidden x = Attr.Create "aria-hidden" x
        static member ariaExpanded x = Attr.Create "aria-expanded" x
