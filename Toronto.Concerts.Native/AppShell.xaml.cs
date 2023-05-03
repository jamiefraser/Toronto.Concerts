namespace Toronto.Concerts.Native
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }
        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);
        }
    }
}