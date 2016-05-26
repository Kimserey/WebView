namespace WebView.Mobile.Core

open Xamarin.Forms

module WebView =
    
    let content =
        new ContentPage(Content = new Label(Text = "Hello world"))
    
    type App() = 
        inherit Application(MainPage = content)