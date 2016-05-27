using System;
using Xamarin.Forms;
using WebView.Mobile.AndroidApp;

[assembly: Dependency(typeof(BaseUrl_Android))]
namespace WebView.Mobile.AndroidApp
{
    public class BaseUrl_Android : WebView.Mobile.Core.IBaseUrl
    {
        public string Get()
        {
            return "file:///android_asset/";
        }
    }
}
