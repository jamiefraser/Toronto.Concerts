using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using Toronto.Concerts.MAUI.Pages;
using Toronto.Concerts.MAUI.ViewModels;
using Toronto.Concerts.Services;
using CommunityToolkit.Maui;
using Syncfusion.Maui.Core.Hosting;
namespace Toronto.Concerts.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF5cWWdCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXpeeHRcQ2lcUEd+WkE=");
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
            builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
            builder.Services.AddSingleton<IGeocoding>(Geocoding.Default);
            return builder.Build();
        }
    }
}
