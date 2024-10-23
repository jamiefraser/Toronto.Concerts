using Toronto.Concerts.MAUI.Pages;

namespace Toronto.Concerts.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute("sos", typeof(SOSPage));
            Routing.RegisterRoute("concertlocation", typeof(MapPage));
            Routing.RegisterRoute("concertdetail", typeof(ConcertDetailPage));
            Routing.RegisterRoute("mainpage", typeof(MainPage));
            Routing.RegisterRoute("startuppage", typeof(StartupPage));
        }
    }
}
