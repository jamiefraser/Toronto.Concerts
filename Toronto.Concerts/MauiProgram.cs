using Microsoft.Extensions.Logging;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Toronto.Concerts.Data;
using Toronto.Concerts.Services;
using MauiInsights;

namespace Toronto.Concerts;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .AddApplicationInsights("InstrumentationKey=a2fc8a11-314b-42a4-9255-13f7e62cdac8;IngestionEndpoint=https://canadacentral-1.in.applicationinsights.azure.com/;LiveEndpoint=https://canadacentral.livediagnostics.monitor.azure.com/")
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });
        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddFluentUIComponents();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
#if IOS
        builder.Services.AddSingleton<ICalendarService, Toronto.Concerts.Platforms.iOS.CalendarService>();
#elif ANDROID
        builder.Services.AddSingleton<ICalendarService, Toronto.Concerts.Platforms.Android.CalendarService>();
#endif
#if ANDROID
        //
#endif
        //builder.Services.AddSingleton<WeatherForecastService>();
        builder.Services.AddSingleton<IConcertDataService, ConcertDataService>();

        return builder.Build();
    }
}
