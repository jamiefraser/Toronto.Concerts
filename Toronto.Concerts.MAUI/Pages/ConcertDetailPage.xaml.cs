using Toronto.Concerts.MAUI.ValueConverters;
using Toronto.Concerts.MAUI.ViewModels;

namespace Toronto.Concerts.MAUI.Pages;

public partial class ConcertDetailPage : ContentPage
{
	private readonly ConcertDetailViewModel vm;
	public ConcertDetailPage(ConcertDetailViewModel _vm, IServiceProvider serviceProvider)
	{
        vm = _vm;
        InitializeComponent();
        this.Resources.Add("VenueToDistanceConverter", serviceProvider.GetRequiredService<VenueToDistanceConverter>());
    }
    protected override void OnAppearing()
    {
        this.BindingContext = vm;
        base.OnAppearing();
        vm.UpdateConcertAndVenue();
        this.map.MoveToRegion(new Microsoft.Maui.Maps.MapSpan(new Location(vm.ConcertVenue[0].Location.Latitude, vm.ConcertVenue[0].Location.Longitude), 0.01, 0.01));
    }
}