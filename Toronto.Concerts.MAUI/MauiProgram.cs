using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using Toronto.Concerts.MAUI.Pages;
using Toronto.Concerts.MAUI.ViewModels;
using Toronto.Concerts.Services;
using CommunityToolkit.Maui;
using Syncfusion.Maui.Core.Hosting;
using Toronto.Concerts.MAUI.Services;
using Toronto.Concerts.MAUI.ValueConverters;
namespace Toronto.Concerts.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NBaF1cXmhMYVJwWmFZfVpgfV9EY1ZUQmYuP1ZhSXxXdkBjUH9XcndQQGdaWUQ=");
            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .UseMauiCommunityToolkit()
                .UseMauiMaps()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseMauiCompatibility();
            
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
            builder.Services.AddSingleton<ConcertDetailViewModel, ConcertDetailViewModel>();
            builder.Services.AddSingleton<ConcertDetailPage>();
            builder.Services.AddSingleton<UserLocationService, UserLocationService>();
            builder.Services.AddSingleton<VenueToDistanceConverter, VenueToDistanceConverter>();
            return builder.Build();
        }
    }
}
