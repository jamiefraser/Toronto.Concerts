using System.ComponentModel;
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
            vm.PropertyChanged += OnSelectedConcertChanged;
        }
        private void OnSelectedConcertChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName=="GroupedConcerts" && cvConcerts.ItemsSource==null)
            {
                cvConcerts.ItemsSource = vm.GroupedConcerts;
            }
            if (e.PropertyName == "SelectedConcert")
                try
                {
                    cvConcerts.ScrollTo(vm.SelectedConcert, position: ScrollToPosition.Start, animate: true);
                }
                catch { }
        }
        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;
        }

        private void cvConcerts_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            
        }
    }

}
