using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
#if iOS
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
#endif
using Toronto.Concerts.Native.Pages;
using Toronto.Concerts.Services;

namespace Toronto.Concerts.Native
{
    public partial class App : Application
    {
        private IConcertDataService _concertDataService;
        private readonly ILogger<App> _logger;
        public App(AppShell shell, ILogger<App> logger)
        {
            InitializeComponent();
            _logger = logger;
#if iOS
            (Application.Current as IApplicationController)?.SetAppIndexingProvider(new IOSAppIndexingProvider());
                        this.AppLinks.RegisterLink(new AppLinkEntry() { AppLinkUri = new Uri("concerts://com.relevant.toronto.concerts.native") });

#endif
#if ANDROID
            //(Application.Current as IApplicationController)?.SetAppIndexingProvider(new AndroidAppIndexProvider(Microsoft.Maui.ApplicationModel.Platform.CurrentActivity));
#endif
            MainPage = shell;

        }

        protected override async void OnAppLinkRequestReceived(Uri uri)
        {
            try
            {
                _concertDataService = this.Handler.MauiContext.Services.GetRequiredService<IConcertDataService>();

                var segments = uri.Segments.ToList();
                var segmentCount = segments.Count();
                _concertDataService.SharedConcertTitle = segments.Last().Replace("%20", " ");
                _concertDataService.SharedConcertDate = segments[segments.Count - 2].Substring(0, segments[segments.Count - 2].Length - 1);
                if (_concertDataService.Concerts == null || _concertDataService.Concerts.Count() == 0) await _concertDataService.GetConcerts();
                try
                {
                    await MainPage.Navigation.PopToRootAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
                await MainPage.Navigation.PushAsync(this.Handler.MauiContext.Services.GetRequiredService<SharedConcertDetailPage>());
                //base.OnAppLinkRequestReceived(uri);
            }
            catch(Exception higherEx)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"{higherEx.Message}\r\n{higherEx.StackTrace}", "OK");

                _logger.LogError(higherEx,higherEx.Message);
            }
        }
    }
}