namespace WebView.Mobile

open System
open System.IO
open System.Reflection
open Xamarin.Forms

type IBaseUrl =
    abstract member Get: unit -> string
        
module View =
    let baseUrl =
        DependencyService.Get<IBaseUrl>().Get()
        
    let htmlSource =
        new HtmlWebViewSource(Html = """ <html><body>hello world</body></html> """, BaseUrl = baseUrl)

    let view = 
        new WebView(Source = htmlSource)

    let stackLayout =
        let l = new StackLayout()
        l.Children.Add view
        l.Children.Add (new Label(Text = (typeof<WebView>.GetTypeInfo().Assembly.GetManifestResourceNames() |> String.concat " ")))
        l

    let content =
        new ContentPage(Content = new Label(Text = (typeof<WebView>.GetTypeInfo().Assembly.GetManifestResourceNames() |> Array.append [| "hehe" |] |> String.concat " ")))
    
    type App() = 
        inherit Application(MainPage = content)