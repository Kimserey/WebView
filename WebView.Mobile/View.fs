﻿namespace WebView.Mobile

open System
open System.IO
open System.Reflection
open Xamarin.Forms

type IBaseUrl =
    abstract member Get: unit -> string
        
module View =
    let baseUrl =
        DependencyService.Get<IBaseUrl>().Get()

    let html = """
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <title>WebView.WebApp</title>
            <meta charset="utf-8" />
            <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            <link href="css/bootstrap.min.css" type="text/css" rel="stylesheet">
            <script src="js/jquery-1.12.4.min.js" type="text/javascript"></script>
            <script src="js/bootstrap.min.js" type="text/javascript"></script>
        </head>
        <body>
            <div id="main"></div>
            <script type="text/javascript" src="Content/WebView.WebApp.min.js"></script>
        </body>
        </html>
    """
    let webView = new WebView(Source = new HtmlWebViewSource(Html = html, BaseUrl = baseUrl))

type App() = 
    inherit Application(MainPage = new ContentPage(Content = View.webView))