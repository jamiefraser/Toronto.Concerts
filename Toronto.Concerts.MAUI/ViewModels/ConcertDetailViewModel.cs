using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Toronto.Concerts.Data;
using Toronto.Concerts.Services;

namespace Toronto.Concerts.MAUI.ViewModels
{
    public partial class ConcertDetailViewModel : ViewModelBase
    {
        private IConcertDataService _concertDataService;
        public ConcertDetailViewModel(IConcertDataService concertDataService)
        {
            _concertDataService = concertDataService;
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
            OnPropertyChanged(nameof (ConcertVenue));
        }
        public Concert SelectedConcert =>   _concertDataService.SelectedConcert;
        private List<Place> concertVenue;
        public List<Place> ConcertVenue
        {
            get
            {
                return concertVenue;
            }
        }
    }
}
