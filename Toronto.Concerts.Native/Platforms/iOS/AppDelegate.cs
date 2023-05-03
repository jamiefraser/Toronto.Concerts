using Foundation;
using System;
using UIKit;

namespace Toronto.Concerts.Native
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
        public override bool OpenUrl(UIApplication application, NSUrl url, NSDictionary options)
        {
            Microsoft.Maui.Controls.Application.Current.SendOnAppLinkRequestReceived(url);
            return true;
        }
    }
}