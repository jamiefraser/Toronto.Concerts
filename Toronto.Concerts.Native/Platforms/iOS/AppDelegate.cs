using Foundation;
using System;
using UIKit;

namespace Toronto.Concerts.Native
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp()
        {
            try
            {
                var app = MauiProgram.CreateMauiApp();
                
                return app;
            }
            catch (Exception ex)
            {
                
                throw;
            }

        }
        public override bool OpenUrl(UIApplication application, NSUrl url, NSDictionary options)
        {
            Microsoft.Maui.Controls.Application.Current.SendOnAppLinkRequestReceived(url);
            return true;
        }
        [Export("application:didFinishLaunchingWithOptions:")]
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Microsoft.AppCenter.AppCenter.Start("5dcdf7d2-800d-48b7-a629-be2030f70b65",
                              typeof(Microsoft.AppCenter.Analytics.Analytics), typeof(Microsoft.AppCenter.Crashes.Crashes));
            return base.FinishedLaunching(application, launchOptions);
        }
       
    }
}