using Toronto.Concerts.Services;

namespace Toronto.Concerts.Native.Pages;

public partial class MainPage : ContentPage
{
	public IConcertDataService ConcertService { get;private set; }
	public MainPage(IConcertDataService cs)
	{
		InitializeComponent();
		ConcertService = cs;
        this.BindingContext = ConcertService;
        if (DeviceInfo.Current.DeviceType.Equals(DeviceIdiom.Phone))
        {
            //gridLayout.SpanCount = 1;
        }
        else
        {
            //gridLayout.SpanCount = 2;
        }
        cs.GetConcerts();

	}

}