using System.ComponentModel;
using System.Text;
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
        public async Task<bool> GetConcerts()
        {
            var ret = false;
            Busy = true;
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

                var concertsResponse = await _client.PostAsync("https://www.thewholenote.com/ludwig/listings/search.php", formContent);
                var json = await concertsResponse.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(json);
                Concerts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Concert>>(json);
                var todaysConcerts = Concerts.Where(concert => concert.DateAndTime.Date.Equals(DateTime.Now.Date)).ToList();
                if (DeviceInfo.Current.Platform.Equals(DevicePlatform.WinUI))
                {
                    System.IO.Directory.SetCurrentDirectory("c:\\git\\toronto.concerts");
                    using (var s = System.IO.File.OpenWrite("concerts.json"))
                    {

                        await (s.WriteAsync(Encoding.ASCII.GetBytes(json)));
                    }
                    var addresses = Concerts.Select(c => c.address);
                    var uniqueAddresses = addresses.Distinct();
                    using (var sa = System.IO.File.OpenWrite("addresses.json"))
                    {
                        await (sa.WriteAsync(Encoding.ASCII.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(uniqueAddresses))));
                    }
                }
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
