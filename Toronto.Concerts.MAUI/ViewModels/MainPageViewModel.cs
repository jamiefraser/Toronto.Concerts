﻿using Microsoft.VisualBasic;
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
                    selectedconcert = value;
                    OnPropertyChanged(nameof(SelectedConcert));
                    SelectedDate = selectedconcert.DateAndTime.Date;
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
                    SelectedConcert = concerts.FirstOrDefault(c => c.DateAndTime.Date.Equals(selecteddate.Date));
                }
            }
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
            if (e.PropertyName == "GroupedConcerts" || e.PropertyName=="Concerts")
            {
                Concerts = _concertDataService.Concerts;
                OnPropertyChanged(nameof(GroupedConcerts));
                SelectedConcert = concerts.FirstOrDefault();
                OnPropertyChanged(nameof(Count));
                OnPropertyChanged(nameof(Dates));
                OnPropertyChanged(nameof(SelectedDate));
            }
        }
    }
}
