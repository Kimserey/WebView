namespace WebView.Mobile

open System
open System.IO
open System.Reflection
open Xamarin.Forms

type IBaseUrl =
    abstract member Get: unit -> string
        
type View() =
    inherit ContentPage(Content = new WebView(Source = new HtmlWebViewSource(Html = (new StreamReader(typeof<View>.GetTypeInfo().Assembly.GetManifestResourceStream("WebView.Mobile.index.html"))).ReadToEnd(), BaseUrl = DependencyService.Get<IBaseUrl>().Get())))


type App() = 
    inherit Application(MainPage = new View())