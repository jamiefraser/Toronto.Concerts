using Microsoft.Maui.ApplicationModel.DataTransfer;
using System.Web;
using Toronto.Concerts.Services;

namespace Toronto.Concerts.Native.Pages;

public partial class MainPage : ContentPage
{
	public IConcertDataService ConcertService { get;private set; }
    private ICalendarService _calendarService;
	public MainPage(IConcertDataService cs, ICalendarService calendarService)
	{
		InitializeComponent();
        _calendarService = calendarService;
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
        ConcertService.SelectedDate = null;
        if (ConcertService.Concerts == null || ConcertService.Concerts.Count() == 0)
        {
            try
            {
                ConcertService.GetConcerts();
            }
            catch (Exception ex)
            {
                DisplayAlert("error!", $"{ex.Message}\r\n{ex.StackTrace}", "OK");
            }
        }
	}

    private async void AddToCalendar_Clicked(object sender, EventArgs e)
    {
        var result = await _calendarService.AddEventToCalendar(ConcertService.SelectedConcert.DateAndTime, ConcertService.SelectedConcert.DateAndTime.AddHours(2), ConcertService.SelectedConcert.title, $"{ConcertService.SelectedConcert.performers}\r\n{ConcertService.SelectedConcert.repertoire}", ConcertService.SelectedConcert.address);
        if (result.isAdded)
        {
            await App.Current.MainPage.DisplayAlert("Success", "Event added to your calendar.  Enjoy!", "OK");
        }
        else
        {
            await App.Current.MainPage.DisplayAlert("Failure", $"Couldn't add the event to your calendar.  The error message was: \r\n{result.message}", "OK");
        }
    }
    private async Task Share(string uri)
    {
        var share = Microsoft.Maui.ApplicationModel.DataTransfer.Share.Default;
        await share.RequestAsync(new ShareTextRequest
        {
            Uri = uri,
            Title = "Share Web Link"
        });
    }
    public async void ShareConcert(System.Object sender, System.EventArgs e)
    {
        await Share($"concerts://com.relevant.toronto.concerts.native/{ConcertService.SelectedConcert.date}/{HttpUtility.UrlEncode(ConcertService.SelectedConcert.title)}"); 
    }
}