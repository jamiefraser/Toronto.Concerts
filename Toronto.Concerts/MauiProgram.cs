using Microsoft.Extensions.Logging;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Toronto.Concerts.Data;
using Toronto.Concerts.Services;

namespace Toronto.Concerts;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
#if IOS
        if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
        {
            builder.Services.AddSingleton<ICalendarService, Toronto.Concerts.Platforms.iOS.CalendarService>();
        }
#endif

#if ANDROID
    builder.Services.AddSingleton<ICalendarService, Toronto.Concerts.Platforms.Android.CalendarService>();
#endif
        builder.Services.AddSingleton<WeatherForecastService>();
        builder.Services.AddSingleton<IConcertDataService, ConcertDataService>();
        builder.Services.AddFluentUIComponents();

        return builder.Build();
    }
}
