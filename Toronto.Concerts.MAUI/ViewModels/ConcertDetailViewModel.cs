using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
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
        }
        public Concert SelectedConcert => _concertDataService.SelectedConcert;
    }
}
