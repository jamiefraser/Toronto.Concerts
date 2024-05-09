using Syncfusion.Maui.DataSource.Extensions;
using System.ComponentModel;
using Toronto.Concerts.Data;
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
            {
                try
                {
                    Shell.Current.GoToAsync("concertdetail", true);
                    //var index = vm.Concerts.IndexOf(vm.SelectedConcert);
                    //cvConcerts.ScrollTo(position: index);
                }
                catch { }
            }
        }
        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;
        }

        private void cvConcerts_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("sos",true);
        }
    }
}
