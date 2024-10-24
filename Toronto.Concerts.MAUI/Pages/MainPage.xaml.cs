﻿using Syncfusion.Maui.DataSource.Extensions;
using System.ComponentModel;
using Toronto.Concerts.Data;
using Toronto.Concerts.MAUI.Pages;
using Toronto.Concerts.MAUI.ValueConverters;
using Toronto.Concerts.MAUI.ViewModels;

namespace Toronto.Concerts.MAUI
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private MainPageViewModel vm;
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
        }

        private void OnSelectedConcertChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName=="Concerts")
            {
                cvConcerts.ItemsSource = vm.Concerts;
                cvConcerts.RefreshView();
            }
            if (e.PropertyName == "SelectedConcert")
            {
                try
                {
                    Navigation.PushAsync(serviceProvider.GetRequiredService<ConcertDetailPage>());
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
    }
}
