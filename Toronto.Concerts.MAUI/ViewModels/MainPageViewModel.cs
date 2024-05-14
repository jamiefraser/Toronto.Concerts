using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualBasic;
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
        public MainPageViewModel(IConcertDataService concertDataService, UserLocationService _userLocationService)
        {
            _concertDataService = concertDataService;
            userLocationService = _userLocationService;
            userLocationService.PropertyChanged += UserLocationService_PropertyChanged;
            _concertDataService.PropertyChanged += _concertDataService_PropertyChanged;
            _concertDataService.GetConcerts();
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
                if (value != null)
                {
                    _concertDataService.SelectedConcert = value;
                    OnPropertyChanged(nameof(SelectedConcert));
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
                if (selecteddate != value)
                {
                    selecteddate = value;
                    OnPropertyChanged(nameof(SelectedDate));
                    OnPropertyChanged(nameof(Concerts));
                    //SelectedConcert = concerts.FirstOrDefault(c => c.DateAndTime.Date.Equals(selecteddate.Date));
                }
            }
        }
        public string Message => "Hello from ViewModel";
        private List<Concert> concerts = new List<Concert>();

        public List<Concert> Concerts
        {
            get
            {
                if(selecteddate.Equals(DateTime.MinValue))return concerts;
                else
                {
                    return concerts.Where(c => c.DateAndTime >= selecteddate).ToList<Concert>();
                }
            }
            set
            {
                concerts = value;
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
    }
}
