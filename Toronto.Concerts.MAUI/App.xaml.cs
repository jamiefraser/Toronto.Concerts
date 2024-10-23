using Toronto.Concerts.MAUI.Pages;

namespace Toronto.Concerts.MAUI
{
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider;
            MainPage = new NavigationPage();
            MainPage.Navigation.PushAsync(_serviceProvider.GetRequiredService<StartupPage>());
        }
    }
}
