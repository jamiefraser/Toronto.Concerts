using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toronto.Concerts.Data;

namespace Toronto.Concerts.Services
{
    public interface IConcertDataService: INotifyPropertyChanged
    {
        public List<Concert> Concerts { get; set; }
        Task<bool> GetConcerts();
        public bool Busy { get; set; }
        public Concert SelectedConcert { get; set; }
    }
}
