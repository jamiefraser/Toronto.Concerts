using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toronto.Concerts.Data;
using Toronto.Concerts.Services;

namespace Toronto.Concerts.MAUI.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private IConcertDataService _concertDataService;
        public MainPageViewModel(IConcertDataService concertDataService)
        {
            _concertDataService = concertDataService;
            _concertDataService.PropertyChanged += _concertDataService_PropertyChanged;
            _concertDataService.GetConcerts();
            //_concertDataService.GetConcerts();
        }
        public string Message => "Hello from ViewModel";
        private List<Concert> concerts = new List<Concert>();

        public List<Concert> Concerts 
        {
            get { return concerts; }
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
            if (e.PropertyName == nameof(_concertDataService.Concerts))
            {
                Concerts = _concertDataService.Concerts;
                OnPropertyChanged(nameof(GroupedConcerts));
                OnPropertyChanged(nameof(Count));
            }
        }

    }
}
