using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Toronto.Concerts.Data;

namespace Toronto.Concerts.MAUI.ViewModels
{
    public partial class SOSPageViewModel : ViewModelBase
    {
        public SOSPageViewModel(IGeolocation geolocation, IGeocoding geocoding)
        {
            this.geocoding = geocoding;
            this.geolocation = geolocation;
        }
        public string HelloMessage => "Hello from the SOS ViewModel!";
        [ObservableProperty]
        bool ready;
        private bool isready = false;
        public bool IsReady
        {
            get
            {
                return isready;
            }
            set
            {
                if(isready != value)
                {
                    isready = value;
                    GetHelpNowCommand.NotifyCanExecuteChanged();
                    OnPropertyChanged(nameof(IsReady));
                }
            }
        }
        [ObservableProperty]
        ObservableCollection<Place> bindablePlaces;

        public ObservableCollection<Place> Places { get; } = new();

        private CancellationTokenSource cts;
        private IGeolocation geolocation;
        private IGeocoding geocoding;
        [RelayCommand(AllowConcurrentExecutions =false,CanExecute ="IsReady")]
        private async Task GetHelpNowAsync()
        {
            System.Diagnostics.Debug.WriteLine("Help is on the way!");
        }

        [RelayCommand]
        private async Task GetCurrentLocationAsync()
        {
            try
            {
                cts = new CancellationTokenSource();

                var request = new GeolocationRequest(
                    GeolocationAccuracy.Medium,
                    TimeSpan.FromSeconds(10));

                var location = await geolocation.GetLocationAsync(request, cts.Token);
                var placemarks = await geocoding.GetPlacemarksAsync(location);
                var address = string.Empty;// placemarks?.FirstOrDefault()?.AdminArea;
                var firstPlace = placemarks?.FirstOrDefault();
                if(firstPlace?.SubThoroughfare != null)
                {
                    address += " " + firstPlace.SubThoroughfare;
                }
                if(firstPlace?.Thoroughfare != null)
                {
                    address += " " + firstPlace.Thoroughfare;
                }
                if(firstPlace?.Locality != null)
                {
                    address += ", " + firstPlace?.Locality;
                }
                if(string.IsNullOrEmpty(address))
                {
                    address = firstPlace?.AdminArea;
                }
                Places.Clear();

                var place = new Place()
                {
                    Location = location,
                    Address = address,
                    Description = "Current Location"
                };

                Places.Add(place);

                var placeList = new List<Place>() { place };
                this.BindablePlaces = new ObservableCollection<Place>(placeList);
                IsReady = true;
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }
    }
}
