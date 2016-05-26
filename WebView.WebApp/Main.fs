namespace WebView.WebApp

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Client
open WebSharper.Resources

module Resources = 

    [<assembly:System.Web.UI.WebResource("myfile.js", "text/javascript");
      assembly:System.Web.UI.WebResource("jquery-1.12.4.min.js", "text/javascript");
      assembly:System.Web.UI.WebResource("test.css", "text/css")>]
    do ()
    
[<JavaScript>]
module Main =

    let main =
        text "Hello world"
        |> Doc.RunById "main"
