using Microsoft.Extensions.Logging;
using PanCardView;
using Syncfusion.Maui;
using Syncfusion.Maui.Core.Hosting;
using Toronto.Concerts.Native.Pages;
using Toronto.Concerts.Services;

namespace Toronto.Concerts.Native
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt+QHFqVkNrXVNbdV5dVGpAd0N3RGlcdlR1fUUmHVdTRHRcQllhTn5ackJiUX5acnw=;Mgo+DSMBPh8sVXJ1S0d+X1RPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9gSXpRcUVqXXhfeHVTQ2g=;ORg4AjUWIQA/Gnt2VFhhQlJBfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5XdEJjUHtZcXxUQWlU;MTg1OTQ4OUAzMjMxMmUzMTJlMzMzNUF4VUlwNk1sSGRtTG5mU3EyUTZ3cU4xaUV0V0I1NG1KY05ocmZNZkpJem89;MTg1OTQ5MEAzMjMxMmUzMTJlMzMzNUpXMlhObkYwUmdUNCtraThrOXBuMDgzWHpDdUg1WVZLYVluZCtiRVI2VGc9;NRAiBiAaIQQuGjN/V0d+XU9Hc1RDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS31TckdkWXZad3VdRGRVWQ==;MTg1OTQ5MkAzMjMxMmUzMTJlMzMzNVFkZ3VHYTRnZ0ozWTRBdkVqeitiRnpUczZ5Q0lTTXFwWmsrZTlMdFNXTlE9;MTg1OTQ5M0AzMjMxMmUzMTJlMzMzNUMrcGQwN0xzajIwZ1RydGlhUExhamRzd2Y0blFpM3FYUGIyYTZ1NFVvRjg9;Mgo+DSMBMAY9C3t2VFhhQlJBfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5XdEJjUHtZcXxWRGRU;MTg1OTQ5NUAzMjMxMmUzMTJlMzMzNUMwM0tFSXVWTUFpY2xYYjEzNFpDWCtaT1JOUkp3WEpxOCtxYllrR1M3V3M9;MTg1OTQ5NkAzMjMxMmUzMTJlMzMzNUZZeVVwajhqN2RoTDVWdGtOT2tWcS96UUVyMjdVdGVoMzJ2d0NSMWQ1Tmc9;MTg1OTQ5N0AzMjMxMmUzMTJlMzMzNVFkZ3VHYTRnZ0ozWTRBdkVqeitiRnpUczZ5Q0lTTXFwWmsrZTlMdFNXTlE9");
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseCardsView()
                .UseMauiMaps()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
		builder.Logging.AddDebug();
#endif
#if IOS
            builder.Services.AddSingleton<ICalendarService, Toronto.Concerts.Platforms.iOS.CalendarService>();
#elif ANDROID
        builder.Services.AddSingleton<ICalendarService, Toronto.Concerts.Platforms.Android.CalendarService>();
#elif WINDOWS
            builder.Services.AddSingleton<ICalendarService, Toronto.Concerts.Native.Platforms.Windows.CalendarService>();
#endif
            //builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<IConcertDataService, ConcertDataService>();
            return builder.Build();
        }
    }
}