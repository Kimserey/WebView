namespace WebView.Mobile

open System
open System.IO
open System.Reflection
open Xamarin.Forms

type IBaseUrl =
    abstract member Get: unit -> string
        
module Core =
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

    let grid =
        let backButton = new Button(Text = "Back")
        backButton.Clicked.AddHandler(fun _ _ -> if webView.CanGoBack then webView.GoBack())
        let nextButton = new Button(Text = "Next")
        nextButton.Clicked.AddHandler(fun _ _ -> if webView.CanGoForward then webView.GoForward())

        let g = new Grid()
        g.RowDefinitions.Add(new RowDefinition(Height = GridLength.Auto))
        g.RowDefinitions.Add(new RowDefinition(Height = new GridLength(300.)))
        g.ColumnDefinitions.Add(new ColumnDefinition(Width = new GridLength(1., GridUnitType.Star)))
        g.ColumnDefinitions.Add(new ColumnDefinition(Width = new GridLength(1., GridUnitType.Star)))
        g.Children.Add(backButton, 0, 0)
        g.Children.Add(nextButton, 1, 0)

        g.Children.Add(webView, 0, 2, 1, 2)
        g

type App() = 
    inherit Application(MainPage = new ContentPage(Content = Core.grid))