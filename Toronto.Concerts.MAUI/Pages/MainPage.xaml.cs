using Microsoft.Maui.Controls.Maps;
using Syncfusion.Maui.DataSource.Extensions;
using System.ComponentModel;
using System.Net.NetworkInformation;
using Toronto.Concerts.Data;
using Toronto.Concerts.MAUI.Pages;
using Toronto.Concerts.MAUI.ValueConverters;
using Toronto.Concerts.MAUI.ViewModels;

namespace Toronto.Concerts.MAUI
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private IServiceProvider serviceProvider;
        public MainPage(MainPageViewModel _vm, IServiceProvider _serviceProvider)
        {
            InitializeComponent();
            vm = _vm;
            serviceProvider = _serviceProvider;
            this.BindingContext = vm;
            vm.PropertyChanged += OnSelectedConcertChanged;
            this.Resources.Add("VenueToDistanceConverter", serviceProvider.GetRequiredService<VenueToDistanceConverter>());
            NavigationPage.SetHasBackButton(this, false);
            //this.Resources.Add("VenueToDistanceConverter", serviceProvider.GetRequiredService<VenueToDistanceConverter>());
        }
        private MainPageViewModel vm;
        public MainPageViewModel VM
        {
            get => vm;
            set
            {
                if (vm != value)
                {
                    vm = value;
                    OnPropertyChanged(nameof(VM));
                }
            }
        }
        private void OnSelectedConcertChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName=="ConcertVenue")
            {
                foreach (var item in cvConcerts.ItemsSource)
                {
                    var stackLayout = cvConcerts.FindByName<VerticalStackLayout>("itemStackLayout");
                    if (stackLayout != null)
                    {
                        var grid = stackLayout.FindByName<Grid>("itemGrid");
                        if (grid != null)
                        {
                            var map = grid.FindByName<Microsoft.Maui.Controls.Maps.Map>("map");
                            map?.MoveToRegion(new Microsoft.Maui.Maps.MapSpan(new Microsoft.Maui.Devices.Sensors.Location(vm.ConcertVenue[0].Location.Latitude, vm.ConcertVenue[0].Location.Longitude), 0.01, 0.01));
                        }
                    }
                }
                //(this.cvConcerts.FindByName<Microsoft.Maui.Controls.Maps.Map>("map") as Microsoft.Maui.Controls.Maps.Map)?.MoveToRegion(new Microsoft.Maui.Maps.MapSpan(new Microsoft.Maui.Devices.Sensors.Location(vm.ConcertVenue[0].Location.Latitude, vm.ConcertVenue[0].Location.Longitude), 0.01, 0.01));
            }
            if(e.PropertyName=="Concerts")
            {
                cvConcerts.ItemsSource = vm.Concerts;
            }
            if (e.PropertyName == "SelectedConcert")
            {
                try
                {
                    //Navigation.PushAsync(serviceProvider.GetRequiredService<ConcertDetailPage>());
                    //Shell.Current.GoToAsync("concertdetail", true);
                    //var index = vm.Concerts.IndexOf(vm.SelectedConcert);
                    //cvConcerts.ScrollTo(position: index);
                }
                catch 
                { 
                }
            }
        }
        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;
        }

        private void cvConcerts_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            
        }
        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            /*var stack = Shell.Current.Navigation.NavigationStack.ToArray();
            for (int i = stack.Length - 1; i > 0; i--)
            {
                if (!stack[i].GetType().Equals(typeof(MainPage)))
                {
                    Shell.Current.Navigation.RemovePage(stack[i]);
                }
            }*/
        }

        private void Pin_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

           
        }

        private void map_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ItemsSource")
            {
                var map = sender as Microsoft.Maui.Controls.Maps.Map;
                if (map != null)
                {
                    map.MoveToRegion(new Microsoft.Maui.Maps.MapSpan(new Microsoft.Maui.Devices.Sensors.Location(vm.ConcertVenue[0].Location.Latitude, vm.ConcertVenue[0].Location.Longitude), 0.01, 0.01));
                }
            }
        }

        private void map_BindingContextChanged(object sender, EventArgs e)
        {
            var map = sender as Microsoft.Maui.Controls.Maps.Map;
            if (map != null)
            {
                map.MoveToRegion(new Microsoft.Maui.Maps.MapSpan(new Microsoft.Maui.Devices.Sensors.Location(vm.ConcertVenue[0].Location.Latitude, vm.ConcertVenue[0].Location.Longitude), 0.01, 0.01));
            }
        }
    }
}
