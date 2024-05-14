using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toronto.Concerts.Services;

namespace Toronto.Concerts.MAUI.ViewModels
{
    public partial class StartupViewModel : ViewModelBase
    {
        private IConcertDataService concertDataService;
        public StartupViewModel(IConcertDataService _concertDataService)
        {
            concertDataService = _concertDataService;
        }
        public bool ConcertsRetrieved
        {
            get
            {
                concertDataService.GetConcerts();
                return concertDataService.Concerts != null && concertDataService.Concerts.Count > 0;
            }
        }
        public async Task GetConcerts()
        {
            Busy = true;
            var result = await concertDataService.GetConcerts();
            Busy = false;
            OnPropertyChanged(nameof(Busy));
        }
        [ObservableProperty]
        public bool busy;
    }
}
