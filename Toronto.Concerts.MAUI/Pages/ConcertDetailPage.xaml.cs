using Toronto.Concerts.MAUI.ViewModels;

namespace Toronto.Concerts.MAUI.Pages;

public partial class ConcertDetailPage : ContentPage
{
	private readonly ConcertDetailViewModel vm;
	public ConcertDetailPage(ConcertDetailViewModel _vm)
	{
        vm = _vm;
        InitializeComponent();

	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        this.BindingContext = vm.SelectedConcert;
    }
}