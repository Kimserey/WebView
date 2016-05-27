namespace WebView.Mobile.Core

open Xamarin.Forms

module WebView =
    
    let view = 
        new WebView(Source = 
            new HtmlWebViewSource(Html = 
                """<html><body>
                   <h1>Xamarin.Forms</h1>
                   <p>Welcome to WebView.</p>
                   </body></html>"""))
    
    type App() = 
        inherit Application(MainPage = new ContentPage(Content = view))