using Toronto.Concerts.MAUI.ViewModels;

namespace Toronto.Concerts.MAUI
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private MainPageViewModel vm;
        public MainPage(MainPageViewModel _vm)
        {
            InitializeComponent();
            vm = _vm;
            this.BindingContext = vm;
            
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;
            
        }
    }

}
