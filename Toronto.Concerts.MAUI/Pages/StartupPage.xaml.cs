using Microsoft.Maui.Controls.PlatformConfiguration.GTKSpecific;
using System.ComponentModel;
using Toronto.Concerts.MAUI.ViewModels;

namespace Toronto.Concerts.MAUI.Pages;

public partial class StartupPage : ContentPage
{
    private StartupViewModel vm;
    private IServiceProvider serviceProvider;
    public StartupPage(StartupViewModel _vm, IServiceProvider _serviceProvider)
    {
        this.vm = _vm;
        serviceProvider = _serviceProvider;
        if (vm.ConcertsRetrieved)
        {
            try
            {
                Navigation.RemovePage(serviceProvider.GetRequiredService<MainPage>());
            }
            catch { }
            Navigation.PushAsync(serviceProvider.GetRequiredService<MainPage>(),true);
            //Shell.Current.GoToAsync("mainpage", true);
            return;
        }
        vm.PropertyChanged += vm_PropertyChanged;
        InitializeComponent();
        vm.GetConcerts();

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (!vm.Busy)
        {
            try
            {
                Navigation.RemovePage(serviceProvider.GetRequiredService<MainPage>());
            }
            catch { }
            await Navigation.PushAsync(serviceProvider.GetRequiredService<MainPage>());
            //await Shell.Current.GoToAsync("mainpage", true);
        }
    }
    private async void vm_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "Busy" && !vm.Busy)
        {
            try
            {
                Navigation.RemovePage(serviceProvider.GetRequiredService<MainPage>());
            }
            catch { }
            await Navigation.PushAsync(serviceProvider.GetRequiredService<MainPage>());
            //await Shell.Current.GoToAsync("mainpage", true);
        }
    }
}