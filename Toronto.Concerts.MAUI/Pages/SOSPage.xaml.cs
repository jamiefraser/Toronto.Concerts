using System.ComponentModel;
using Toronto.Concerts.MAUI.ViewModels;

namespace Toronto.Concerts.MAUI.Pages;

public partial class SOSPage : ContentPage
{
	SOSPageViewModel vm;
	public SOSPage(SOSPageViewModel _vm)
	{
		InitializeComponent();
        this.vm = _vm;
        this.BindingContext = vm;
		vm.PropertyChanged += Vm_PropertyChanged;
	}

    private void Vm_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if(e.PropertyName=="IsReady")
        {
            map.MoveToRegion(new Microsoft.Maui.Maps.MapSpan(new Location(vm.Places[0].Location.Latitude, vm.Places[0].Location.Longitude), 0.01, 0.01));
        }
    }

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
		vm.GetCurrentLocationCommand.Execute(null);
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        vm.PropertyChanged -= Vm_PropertyChanged;
    }
}