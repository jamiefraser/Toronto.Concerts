namespace Toronto.Concerts.Native
{
    public partial class App : Application
    {
        public App(AppShell shell)
        {
            InitializeComponent();

            MainPage = shell;
        }
    }
}