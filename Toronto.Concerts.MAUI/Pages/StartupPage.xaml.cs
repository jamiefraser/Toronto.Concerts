using System.ComponentModel;
using Toronto.Concerts.MAUI.ViewModels;

namespace Toronto.Concerts.MAUI.Pages;

public partial class StartupPage : ContentPage
{
    private StartupViewModel vm;
    public StartupPage(StartupViewModel _vm)
    {
        this.vm = _vm;
        if (vm.ConcertsRetrieved)
        {
            Shell.Current.GoToAsync("mainpage", true);
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
            await Shell.Current.GoToAsync("mainpage", true);
        }
    }
    private async void vm_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "Busy" && !vm.Busy)
        {
            await Shell.Current.GoToAsync("mainpage", true);
        }
    }
}