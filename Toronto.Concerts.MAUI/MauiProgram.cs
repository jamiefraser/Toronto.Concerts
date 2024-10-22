using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using Toronto.Concerts.MAUI.Pages;
using Toronto.Concerts.MAUI.ViewModels;
using Toronto.Concerts.Services;
using CommunityToolkit.Maui;
using Syncfusion.Maui.Core.Hosting;
using Toronto.Concerts.MAUI.Services;
using Toronto.Concerts.MAUI.ValueConverters;
using Plugin.Maui.CalendarStore;
namespace Toronto.Concerts.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzUzNzcxOEAzMjM3MmUzMDJlMzBOdXVOV2RGdzJWNmZ4K0YrZ1dkNnI3R0dsNjd5UFJWalFjUFFtckVSaHRzPQ==;MzUzNzcxOUAzMjM3MmUzMDJlMzBmVHNCTDBGbGVPb0J0VXgwNWxHYldOQjMzTGt2TnB3YkJmZ0JpdGZnRlFNPQ==;MzUzNzcyMEAzMjM3MmUzMDJlMzBNejc4Y1JSU015TUJ6L2NHc1Q0VU0yOURCOTI2NVpoaG53YUkzSGJUc1lNPQ==;MzUzNzcyMUAzMjM3MmUzMDJlMzBCSTA1ZnRqQWMySFllQkwxRURTbkszK25CcTNENDdDOVVHSHNwU00yb2NJPQ==;MzUzNzcyMkAzMjM3MmUzMDJlMzBOdXVOV2RGdzJWNmZ4K0YrZ1dkNnI3R0dsNjd5UFJWalFjUFFtckVSaHRzPQ==\r\n\r\n");
            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .UseMauiCompatibility()
                .UseMauiCommunityToolkit()
                .UseMauiMaps()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            
#if DEBUG
            builder.Logging.AddDebug();
#endif

#if ANDROID
        //
#endif
            //builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddSingleton<IConcertDataService, ConcertDataService>();
            builder.Services.AddSingleton<MainPageViewModel, MainPageViewModel>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<SOSPageViewModel, SOSPageViewModel>();
            builder.Services.AddSingleton<SOSPage>();
            builder.Services.AddSingleton<IGeolocation>(Microsoft.Maui.Devices.Sensors.Geolocation.Default);
            builder.Services.AddSingleton<IGeocoding>(Microsoft.Maui.Devices.Sensors.Geocoding.Default);
            builder.Services.AddTransient<ConcertDetailViewModel, ConcertDetailViewModel>();
            builder.Services.AddTransient<ConcertDetailPage>();
            builder.Services.AddSingleton<UserLocationService, UserLocationService>();
            builder.Services.AddSingleton<VenueToDistanceConverter, VenueToDistanceConverter>();
            builder.Services.AddSingleton<StartupViewModel, StartupViewModel>();
            builder.Services.AddSingleton<StartupPage>();
            builder.Services.AddSingleton<ICalendarStore>(CalendarStore.Default);

            return builder.Build();
        }
    }
}
