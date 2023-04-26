using MonkeyCache.FileStore;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text;
using Toronto.Concerts.Data;

namespace Toronto.Concerts.Services
{
    public class ConcertDataService : IConcertDataService
    {
        HttpClient _client;
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
                if (selectedconcert != value)
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
            if (!Barrel.Current.IsExpired(key: url))
            {
                Concerts = Barrel.Current.Get<IEnumerable<Concert>>(key: url).ToList();
                return Concerts;
            }

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

            var concertsResponse = await _client.PostAsync("https://www.thewholenote.com/ludwig/listings/search.php", formContent);
            var json = await concertsResponse.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine(json);
            Concerts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Concert>>(json);

            //Saves the cache and pass it a timespan for expiration
            Barrel.Current.Add(key: url, data: concerts, expireIn: TimeSpan.FromDays(1));
            return concerts;
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
