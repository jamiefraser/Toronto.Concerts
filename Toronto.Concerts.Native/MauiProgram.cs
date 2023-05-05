using Microsoft.Extensions.Logging;
using PanCardView;

using Toronto.Concerts.Native.Pages;
using Toronto.Concerts.Services;

namespace Toronto.Concerts.Native
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseCardsView()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            MauiExceptions.UnhandledException += MauiExceptions_UnhandledException;

#if DEBUG
		builder.Logging.AddDebug();
#endif
#if IOS
            builder.Services.AddSingleton<ICalendarService, Toronto.Concerts.Platforms.iOS.CalendarService>();
#elif ANDROID
        builder.Services.AddSingleton<ICalendarService, Toronto.Concerts.Platforms.Android.CalendarService>();
#elif WINDOWS
            builder.Services.AddSingleton<ICalendarService, Toronto.Concerts.Native.Platforms.Windows.CalendarService>();
#elif MACCATALYST
            builder.Services.AddSingleton<ICalendarService, Toronto.Concerts.Native.Platforms.MacCatalyst.CalendarService>();
#endif
            //builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<SharedConcertDetailPage>();
            builder.Services.AddSingleton<IConcertDataService, ConcertDataService>();
            var app = builder.Build();
            return app;
        }

        private static void MauiExceptions_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Microsoft.AppCenter.Crashes.Crashes.TrackError((Exception)e.ExceptionObject);
        }
    }
}