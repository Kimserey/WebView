using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace WebView.CSharp.Test
{
    /*
    /   Code taken from Xamarin form samples
    //new WebView(Source = new HtmlWebViewSource(Html = (new StreamReader(typeof<View>.GetTypeInfo().Assembly.GetManifestResourceStream("WebView.Mobile.index.html"))).ReadToEnd(), BaseUrl = DependencyService.Get<IBaseUrl>().Get())))
    */
    class View : ContentPage
    {
        Label label = new Label();

        public View()
        {
            var assembly = typeof(View).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("WebView.CSharp.Test.index.html");

            string text = "";
            using (var reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }

            Content = new Xamarin.Forms.WebView {
                Source = new HtmlWebViewSource {
                    Html = text,
                    BaseUrl = DependencyService.Get<IBaseUrl>().Get()
                }
            };
        }
    }
}
