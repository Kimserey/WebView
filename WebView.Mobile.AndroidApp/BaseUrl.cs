using System;
using Xamarin.Forms;
using WebView.Mobile.AndroidApp;

[assembly: Dependency(typeof(BaseUrl))]
namespace WebView.Mobile.AndroidApp
{
    public class BaseUrl : WebView.CSharp.Test.IBaseUrl
    {
        public string Get()
        {
            return "file:///android_asset/";
        }
    }
}
