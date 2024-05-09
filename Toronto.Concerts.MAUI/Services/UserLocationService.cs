using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toronto.Concerts.MAUI.Services
{
    public class UserLocationService : INotifyPropertyChanged
    {
        private readonly IGeolocation geoLocation;
        public UserLocationService(IGeolocation geolocation)
        {
            this.geoLocation = geolocation;
            this.geoLocation.LocationChanged += GeoLocation_LocationChanged;
            this.geoLocation.StartListeningForegroundAsync(new GeolocationListeningRequest(GeolocationAccuracy.Best,TimeSpan.FromMinutes(1)));
        }

        private void GeoLocation_LocationChanged(object? sender, GeolocationLocationChangedEventArgs e)
        {
            SetLocation(Latitude = e.Location.Latitude, Longitude = e.Location.Longitude);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public void SetLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Latitude)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Longitude)));
        }
    }
}
