﻿using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualBasic;
using Plugin.Maui.CalendarStore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toronto.Concerts.Data;
using Toronto.Concerts.MAUI.Services;
using Toronto.Concerts.Services;

namespace Toronto.Concerts.MAUI.ViewModels
{
    public partial class MainPageViewModel : ViewModelBase
    {
        private IConcertDataService _concertDataService;
        private UserLocationService userLocationService;
        private ICalendarStore calendarStore;
        private object lockThis = new object();
        public MainPageViewModel(IConcertDataService concertDataService, UserLocationService _userLocationService, ICalendarStore _calendarStore)
        {
            _concertDataService = concertDataService;
            userLocationService = _userLocationService;
            userLocationService.PropertyChanged += UserLocationService_PropertyChanged;
            _concertDataService.PropertyChanged += _concertDataService_PropertyChanged;
            _concertDataService.GetConcerts();
            this.calendarStore = _calendarStore;

            //_concertDataService.GetConcerts();
        }

        private void UserLocationService_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Latitude" || e.PropertyName == "Longitude")
            {
                OnPropertyChanged(nameof(Concerts));
            }
        }

        private Concert selectedconcert;
        public Concert SelectedConcert
        {
            get

            {
                return selectedconcert;
            }
            set
            {
                try
                {
                    if (value != null)
                    {
                        lock (lockThis)
                        {
                            _concertDataService.SelectedConcert = value;
                            selectedconcert = value;
                            //OnPropertyChanged(nameof(SelectedConcert));
                            concertVenue = new List<Place>();
                            Place place = new Place()


                            {
                                Address = _concertDataService.SelectedConcert?.address,
                                Description = _concertDataService.SelectedConcert?.venue,
                                Location = new Microsoft.Maui.Devices.Sensors.Location()
                                {
                                    Latitude = !string.IsNullOrEmpty(_concertDataService.SelectedConcert?.LatLong) ? double.Parse(_concertDataService.SelectedConcert?.LatLong?.Split(',')[0]) : 0d,
                                    Longitude = !string.IsNullOrEmpty(_concertDataService.SelectedConcert?.LatLong) ? double.Parse(_concertDataService.SelectedConcert?.LatLong?.Split(',')[1]) : 0d
                                }
                            };
                            ConcertVenue.Add(place);
                            OnPropertyChanged(nameof(ConcertVenue));
                            if (selecteddate.Date != selectedconcert.DateAndTime.Date)
                            {
                                selecteddate = selectedconcert.DateAndTime;
                                changingSelectedDateProgramatically = true;
                                OnPropertyChanged(nameof(SelectedDate));
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }
        private bool changingSelectedDateProgramatically = false;
        private List<Place> concertVenue;
        public List<Place> ConcertVenue
        {
            get
            {
                return concertVenue;
            }
        }
        public int SelectedConcertIndex
        {
            get
            {
                if (selectedconcert == null && Concerts.Count > 0) selectedconcert = Concerts[0];
                return selectedconcert != null ? concerts.IndexOf(selectedconcert) : 0;
            }
            set
            {
                if (value >= 0 && value < concerts.Count && SelectedConcert != concerts[value])
                {
                    SelectedConcert = concerts[value];
                    
                    OnPropertyChanged(nameof(SelectedConcertIndex));
                }
            }
        }
        public List<DateTime> Dates => concerts.Select(c => c.DateAndTime.Date).Distinct().ToList<DateTime>();// _concertDataService.Dates;

        private DateTime selecteddate;
        public DateTime SelectedDate
        {
            get
            {
                return selecteddate.Date;
            }
            set
            {
                {
                    lock (lockThis)
                    {
                        selecteddate = value;
                        OnPropertyChanged(nameof(SelectedDate));
                        OnPropertyChanged(nameof(Concerts));
                        var targetDate = concerts.OrderBy(c => c.DateAndTime).FirstOrDefault().DateAndTime.Date;
                        if (selectedconcert != concerts.FirstOrDefault(c => c.DateAndTime.Date.Equals(selecteddate.Date)) && !changingSelectedDateProgramatically)
                        {
                            System.Diagnostics.Debug.WriteLine("setting the selectedconcert to first concert of selected date");
                            selectedconcert = concerts.FirstOrDefault(c => c.DateAndTime.Date.Equals(selecteddate.Date));
                            OnPropertyChanged(nameof(SelectedConcert));
                        }
                        changingSelectedDateProgramatically = false;
                    }
                }
            }
        }
        public string Message => "Hello from ViewModel";
        private List<Concert> concerts = new List<Concert>();

        public List<Concert> Concerts
        {
            get
            {
                if(selecteddate.Equals(DateTime.MinValue))return concerts.Where(c => c.DateAndTime >= DateTime.Now).ToList<Concert>();
                else
                {
                    return concerts;
                }
            }
            set
            {
                concerts = value.OrderBy(c => c.DateAndTime).ToList<Concert>();
                OnPropertyChanged(nameof(Concerts));
            }
        }
        public IEnumerable<IGrouping<string, Concert>> GroupedConcerts => _concertDataService.GroupedConcerts;
        public int Count => concerts.Count;

        private async void _concertDataService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "GroupedConcerts" || e.PropertyName == "Concerts")
            {
                Concerts = _concertDataService.Concerts;
                SelectedConcert = concerts.Last();
                SelectedConcert = concerts.FirstOrDefault();
                selecteddate = (DateTime)(Concerts.First()?.DateAndTime);
                OnPropertyChanged(nameof(GroupedConcerts));
                // SelectedConcert = concerts.FirstOrDefault();
                OnPropertyChanged(nameof(Count));
                OnPropertyChanged(nameof(Dates));
                OnPropertyChanged(nameof(SelectedDate));
            }
        }
        [RelayCommand]
        public void NavigateToConcertCommand(Syncfusion.Maui.ListView.ItemTappedEventArgs eventArgs)
        {
            this._concertDataService.SelectedConcert = eventArgs.DataItem as Concert;
            Shell.Current.GoToAsync("concertdetail", true);
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
