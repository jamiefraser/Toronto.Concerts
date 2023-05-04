using MonkeyCache.FileStore;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using Toronto.Concerts.Data;

namespace Toronto.Concerts.Services
{
    public class ConcertDataService : IConcertDataService
    {
        HttpClient _client;
        public List<Concert> ConcertsOnSelectedDate
        {
            get
            {
                if (groupedConcerts != null && !string.IsNullOrEmpty(selectedDate))
                {
                    if (selectedDate.Equals("Today"))
                    {
                        return groupedConcerts.Where(gc => gc.Key.Equals(DateTime.Now.Date.ToString("MMM dd"))).ToList().FirstOrDefault().ToList();
                    }
                    else
                    {
                        if (selectedDate.Equals("Tomorrow"))
                        {
                            return groupedConcerts.Where(gc => gc.Key.Equals(DateTime.Now.AddDays(1).Date.ToString("MMM dd"))).ToList().FirstOrDefault().ToList();
                        }
                        else
                        {
                            return groupedConcerts.Where(gc => gc.Key.Equals(selectedDate)).ToList().FirstOrDefault().ToList();
                        }
                    }
                }
                return new List<Concert>();
            }
        }
        private string selectedDate;
        public string SelectedDate
        {
            get
            {
                RaisePropertyChanged(nameof(ConcertsOnSelectedDate));
                return selectedDate;
            }
            set
            {

                    selectedDate = value;
                    RaisePropertyChanged(nameof(SelectedDate));
                    RaisePropertyChanged(nameof(ConcertsOnSelectedDate));
                
            }
        }
        private IEnumerable<IGrouping<string, Concert>> groupedConcerts;
        public IEnumerable<IGrouping<string, Concert>> GroupedConcerts
        {
            get
            {
                return groupedConcerts;
            }
            private set
            {
                if (groupedConcerts != value)
                {
                    //List<Grouping<string, Concert>> tmp = new List<Grouping<string, Concert>>();
                    //foreach(var item in value)
                    //{
                    //    if(item.Key.Equals(DateTime.Now.Date.ToString("MMM dd")))
                    //    {
                    //        tmp.Add(new Grouping<string, Concert>("Today", item.ToList()));

                    //    }
                    //}
                    groupedConcerts = value;
                    RaisePropertyChanged(nameof(GroupedConcerts));
                    RaisePropertyChanged(nameof(Dates));
                }
            }
        }
        private async Task PopulateDates()
        {

        }
        public List<string> Dates
        {
            get
            {
                if (groupedConcerts != null)
                {
                    var dates = groupedConcerts.Select(gc => gc.Key).ToList();
                    var retDates = new List<string>();
                    foreach (string date in dates)
                    {
                        if (date.Equals(DateTime.Now.Date.ToString("MMM dd")))
                        {
                            retDates.Add("Today");
                        }
                        else
                        {
                            if (date.Equals(DateTime.Now.AddDays(1).Date.ToString("MMM dd")))
                            {
                                retDates.Add("Tomorrow");
                            }
                            else
                            {
                                retDates.Add(date);
                            }
                        }
                    }
                    SelectedDate = retDates.FirstOrDefault();
                    return retDates;
                }
                return new List<string>();
            }
        }
        private List<Concert> concerts = new List<Concert>();
        public ConcertDataService()
        {
            _client = new HttpClient();
        }
        private bool busy = true;

        public bool Busy
        {
            get { return busy; }
            set
            {
                if (busy != value)
                {
                    busy = value;
                    RaisePropertyChanged(nameof(Busy));
                }
            }
        }
        private Concert selectedconcert;

        public Concert SelectedConcert
        {
            get
            {
                try

                {
                    throw new Exception("Testing application insights");
                }
                catch (Exception ex)
                {

                }
                return selectedconcert;
            }
            set
            {
                if (value == null && selectedconcert != null)
                {
                    RaisePropertyChanged(nameof(SelectedConcert));
                    return;
                }
                if (selectedconcert == null || selectedconcert != value)
                {
                    selectedconcert = value;
                    RaisePropertyChanged(nameof(SelectedConcert));
                }
            }
        }

        public List<Concert> Concerts
        {
            get { return concerts; }
            set
            {
                if (concerts != value)
                {
                    concerts = value;
                    RaisePropertyChanged(nameof(Concerts));
                    RaisePropertyChanged(nameof(Dates));
                }
            }
        }

        private DateTime lasttimesomethingwasset;
        public DateTime LastTimeSomethingWasSet
        {
            get
            {
                return lasttimesomethingwasset;
            }
            set
            {
                if (lasttimesomethingwasset != value)
                {
                    lasttimesomethingwasset = value;
                    RaisePropertyChanged(nameof(LastTimeSomethingWasSet));
                }
            }
        }
        private string sharedconcertdate;
        public string SharedConcertDate
        {
            get
            {
                return sharedconcertdate;
            }
            set
            {
                if (sharedconcertdate != value)
                {
                    sharedconcertdate = value;
                    RaisePropertyChanged(nameof(SharedConcertDate));
                }
            }
        }
        private string sharedconcerttitle;
        public string SharedConcertTitle
        {
            get
            {
                return sharedconcerttitle;
            }
            set
            {
                if(sharedconcerttitle != value)
                {
                    sharedconcerttitle = value;
                    RaisePropertyChanged(nameof(SharedConcertTitle));
                }
            }
        }

        async Task<IEnumerable<Concert>> GetConcertsAsync()
        {
            var url = "https://www.thewholenote.com/ludwig/listings/search.php";
            Barrel.ApplicationId = "Toronto.Concerts";
            //Dev handle online/offline scenario
            //if (!CrossConnectivity.Current.IsConnected)
            //{
            //    return Barrel.Current.Get<IEnumerable<Concert>>(key: url);
            //}

            //Dev handles checking if cache is expired
            System.Diagnostics.Debug.WriteLine(Barrel.Current.Get<string>(key: url));
            if (!Barrel.Current.IsExpired(key: url))
            {
                Concerts = Barrel.Current.Get<IEnumerable<Concert>>(key: url).ToList();
                GroupedConcerts = Concerts.GroupBy(c => c.DateAndTime.Date.ToString("MMM dd"));
                return Concerts;
            }
            try
            {
                var location = new Data.Location() { id = "1", name = "City of Toronto" };
                var today = DateTime.Now.Date.ToUniversalTime();
                var concertQuery = new ConcertQuery()
                {
                    startDate = (int)DateTimeOffset.Now.ToUnixTimeSeconds(),
                    endDate = (int)DateTimeOffset.Now.AddDays(30).ToUnixTimeSeconds(),
                    tags = new List<string>().ToArray(),
                    enhancedOnly = false,
                    locations = new Data.Location[] { location, new Data.Location() { id = "2", name = "Halton-Peel Regions" }, new Data.Location() { id = "3", name = "York Region" }, new Data.Location() { id = "4", name = "Durham Region" } }
                };
                System.Diagnostics.Debug.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(concertQuery));
                //{"query":"","listingId":null,"enhancedOnly":false,"startDate":1681358400,"endDate":1683432000,"locations":[{"id":"1","name":"City+of+Toronto"},{"id":"2","name":"Halton-Peel+Regions"},{"id":"3","name":"York+Region"},{"id":"4","name":"Durham+Region"}],"tags":["Chamber","Choral","Early/Baroque","Musical+Theatre","New+Music","Organ","Orchestra","Piano","Solo+Voice","Strings","Religious+Service"]}
                var formContent = new FormUrlEncodedContent(new[]
                                                            {
                                                                new KeyValuePair<string, string>("query", Newtonsoft.Json.JsonConvert.SerializeObject(concertQuery))
                                                            });

                var concertsResponse = await _client.GetAsync("https://concertsfunctions.azurewebsites.net/api/RetrieveConcerts?code=ZrCRZVpk1MKhcSEBRTDe86NAXl9X7HivlLVZDUxSqCOOAzFuS2TNZw==");
                var json = await concertsResponse.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(json);
                Concerts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Concert>>(json);
                GroupedConcerts = Concerts.GroupBy(c => c.DateAndTime.Date.ToString("MMM dd"));
                //Saves the cache and pass it a timespan for expiration
                Barrel.Current.Add(key: url, data: concerts, expireIn: TimeSpan.FromDays(1));
                return concerts;
            }
            catch(Exception ex)
            {
                Concerts = Barrel.Current.Get<IEnumerable<Concert>>(key: url).ToList();
                GroupedConcerts = Concerts.GroupBy(c => c.DateAndTime.Date.ToString("MMM dd"));
                return Concerts;
            }
        }
        public async Task<bool> GetConcerts()
        {
            Busy = true;
            var ret = false;
            try
            {
                await GetConcertsAsync();
                var todaysConcerts = Concerts.Where(concert => concert.DateAndTime.Date.Equals(DateTime.Now.Date)).ToList();
                ret = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
            }
            finally
            {
                Busy = false;
            }
            return ret;
        }
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
