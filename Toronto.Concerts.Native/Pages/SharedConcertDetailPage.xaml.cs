using Toronto.Concerts.Data;
using Toronto.Concerts.Services;

namespace Toronto.Concerts.Native.Pages;

public partial class SharedConcertDetailPage : ContentPage
{
	private readonly IConcertDataService _concertDataService;
    private readonly ICalendarService _calendarService;
	public SharedConcertDetailPage(IConcertDataService cs, ICalendarService calendarService)
	{
		InitializeComponent();
        this.BindingContext = this;
		_concertDataService = cs;
        _calendarService = calendarService;
        var requestedConcert = from c in _concertDataService.Concerts
                               where c.date == _concertDataService.SharedConcertDate
                               && c.title == _concertDataService.SharedConcertTitle.Replace("%20", " ")
                               select c;

        var groupWithRequestedConcert = from gc in _concertDataService.GroupedConcerts
                                        where
                                        gc.Contains(requestedConcert.FirstOrDefault())
                                        select gc;
        this.Concert = requestedConcert.FirstOrDefault();
    }


    public static BindableProperty ConcertProperty = BindableProperty.Create(nameof(Concert), typeof(Concert), typeof(SharedConcertDetailPage), default(Concert), BindingMode.Default);

    public Concert Concert
    {
        get => (Concert)GetValue(ConcertProperty);
        set => SetValue(ConcertProperty, value);
    }


    private async void AddToCalendarClicked(object sender, EventArgs e)
    {
        var result = await _calendarService.AddEventToCalendar(Concert.DateAndTime, Concert.DateAndTime.AddHours(2), Concert.title, $"{Concert.performers}\r\n{Concert.repertoire}", Concert.address);
        if (result.isAdded)
        {
            await App.Current.MainPage.DisplayAlert("Success", "Event added to your calendar.  Enjoy!", "OK");
        }
        else
        {
            await App.Current.MainPage.DisplayAlert("Failure", $"Couldn't add the event to your calendar.  The error message was: \r\n{result.message}", "OK");
        }
    }
}