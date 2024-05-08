using Toronto.Concerts.MAUI.Pages;

namespace Toronto.Concerts.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("sos", typeof(SOSPage));
        }
    }
}
