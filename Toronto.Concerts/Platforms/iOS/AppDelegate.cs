using Foundation;
using UIKit;

namespace Toronto.Concerts;

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
        catch(Exception ex)
        {
            UIAlertView alert = new UIAlertView("Error", $"{ex.Message}\r\n{ex.StackTrace}",null, "OK");
            alert.Show();
            throw;
        }
        
    }
    
}

