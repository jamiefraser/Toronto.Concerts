using System.ComponentModel;
using Toronto.Concerts.Data;

namespace Toronto.Concerts.Services
{
    public class ConcertDataService : IConcertDataService
    {
        HttpClient _client;
        private List<Concert> concerts;
        public ConcertDataService()
        {
            _client = new HttpClient();
        }
        private bool busy;

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
            get { return selectedconcert; }
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
        public async Task<bool> GetConcerts()
        {
            var ret = false;
            Busy = true;
            try
            {
                var location = new Data.Location() { id = "1", name = "City of Toronto" };
                var concertQuery = new ConcertQuery()
                {
                    startDate = (int)DateTimeOffset.Now.ToUnixTimeSeconds(),
                    endDate = (int)DateTimeOffset.Now.AddDays(30).ToUnixTimeSeconds(),
                    tags = new string[] { "Chamber", "Choral", "Early/Baroque", "Musical+Theatre", "New+Music", "Organ", "Orchestra", "Piano", "Solo+Voice", "Strings", "Religious+Service" },
                    enhancedOnly = false,
                    locations = new Data.Location[] { location }
                };
                //{"query":"","listingId":null,"enhancedOnly":false,"startDate":1681358400,"endDate":1683432000,"locations":[{"id":"1","name":"City+of+Toronto"},{"id":"2","name":"Halton-Peel+Regions"},{"id":"3","name":"York+Region"},{"id":"4","name":"Durham+Region"}],"tags":["Chamber","Choral","Early/Baroque","Musical+Theatre","New+Music","Organ","Orchestra","Piano","Solo+Voice","Strings","Religious+Service"]}
                var formContent = new FormUrlEncodedContent(new[]
                                                            {
                                                                new KeyValuePair<string, string>("query", Newtonsoft.Json.JsonConvert.SerializeObject(concertQuery))
                                                            });

                var concertsResponse = await _client.PostAsync("https://www.thewholenote.com/ludwig/listings/search.php", formContent);
                Concerts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Concert>>(await concertsResponse.Content.ReadAsStringAsync());
                ret = true;
            }
            catch (Exception ex)
            {
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
