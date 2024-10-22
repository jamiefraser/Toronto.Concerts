using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Toronto.Concerts.Data;
using Toronto.Concerts.Services;
using Plugin.Maui.AddToCalendar;
using Plugin.Maui.CalendarStore;
using System.Linq;
using CoreData;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
namespace Toronto.Concerts.MAUI.ViewModels
{
    public partial class ConcertDetailViewModel : ViewModelBase
    {
        private IConcertDataService _concertDataService;
        private ICalendarStore calendarStore;
        public ConcertDetailViewModel(IConcertDataService concertDataService, ICalendarStore _calendarStore)
        {
            _concertDataService = concertDataService;
            this.calendarStore = _calendarStore;
            //addToCalendar = _addToCalendar;
            concertVenue = new List<Place>();
            Place place = new Place()
            {
                Address = SelectedConcert.address,
                Description = SelectedConcert.venue,
                Location = new Microsoft.Maui.Devices.Sensors.Location()
                {
                    Latitude = double.Parse(SelectedConcert.LatLong.Split(',')[0]),
                    Longitude = double.Parse(SelectedConcert.LatLong.Split(',')[1])
                }
            };
            ConcertVenue.Add(place);
        }
        public void UpdateConcertAndVenue()
        {
            concertVenue.Clear();
            Place place = new Place()
            {
                Address = SelectedConcert.address,
                Description = SelectedConcert.venue,
                Location = new Microsoft.Maui.Devices.Sensors.Location()
                {
                    Latitude = double.Parse(SelectedConcert.LatLong.Split(',')[0]),
                    Longitude = double.Parse(SelectedConcert.LatLong.Split(',')[1])
                }
            };
            concertVenue.Add(place);
            OnPropertyChanged(nameof(SelectedConcert));
            OnPropertyChanged(nameof(ConcertVenue));
        }
        public Concert SelectedConcert => _concertDataService.SelectedConcert;
        private List<Place> concertVenue;
        public List<Place> ConcertVenue
        {
            get
            {
                return concertVenue;
            }
        }
        private Command getDirectionsCommand;
        public Command GetDirectionsCommand
        {
            get
            {
                return getDirectionsCommand ??= new Command(OnGetDirections);
            }
        }
        private async void OnGetDirections()
        {
            var location = new Microsoft.Maui.Devices.Sensors.Location(double.Parse(SelectedConcert.LatLong.Split(',')[0]), double.Parse(SelectedConcert.LatLong.Split(',')[1]));
            await Map.OpenAsync(location, new MapLaunchOptions
            {
                Name = SelectedConcert.venue,
                NavigationMode = NavigationMode.None
            });
        }
        private Command addToCalendarCommand;
        public Command AddToCalendarCommand
        {
            get
            {
                return addToCalendarCommand ??= new Command(OnAddToCalendar);
            }
        }

        private async void OnAddToCalendar(object obj)
        {
            var startDate = SelectedConcert.DateAndTime;
            var endDate = SelectedConcert.DateAndTime.AddHours(2);
            var calendars = await calendarStore.GetCalendars();

            var calendar = calendars.FirstOrDefault();
            var calendarId = calendar.Id;
            var calendarEvent = new CalendarEvent(Guid.NewGuid().ToString(), calendarId, SelectedConcert.title);
            var item = await calendarStore.CreateEvent(calendarEvent);
            System.Diagnostics.Debug.WriteLine(item);
            await calendarStore.UpdateEvent(item, calendarEvent.Title, SelectedConcert.description, $"{SelectedConcert.address} {SelectedConcert.city}", startDate, endDate, false);
        }
    }
}
