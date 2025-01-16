using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Toronto.Concerts.Data;

namespace concerts.functions
{
    public class ConcertsFunctions
    {
        #region Test
        [FunctionName("FillCacheManually")]
        [StorageAccount("StorageConnection")]
        public async Task TestFetch([HttpTrigger] HttpRequest req, [Blob(blobPath: "concertscache/concerts.json", FileAccess.Write)] BlockBlobClient fileJson, ILogger log)
        {
            try
            {
                var json = await GetConcerts();
                var concerts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Concert>>(json);
                try
                {
                    concerts = await ProcessVenueAddresses(concerts);
                    json = Newtonsoft.Json.JsonConvert.SerializeObject(concerts);
                    byte[] byteArray = Encoding.UTF8.GetBytes(json);
                    {
                        var accessCondition = new AccessCondition();
                        var blobRequestOptions = new BlockBlobOpenWriteOptions();

                        var operationContext = new OperationContext();

                        // ETag access condition is used so that it will not cause any issue due to ongoing concurrent modification on the same blob

                        accessCondition.IfMatchETag = "*";
                        MemoryStream ms = new MemoryStream(byteArray);
                        var result = await fileJson.UploadAsync(ms);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        [FunctionName("ProcessVenueAddresses")]
        [StorageAccount("StorageConnection")]
        private async Task<List<Concert>> ProcessVenueAddresses(List<Concert> concerts)
        {

            foreach (var c in concerts.Where(c=>string.IsNullOrEmpty(c.LatLong)))
            {
                if(!string.IsNullOrEmpty(c.venue))
                {
                    var place = await GeocodePlaceNamesAsync(c.venue);
                    if (place != null && !string.IsNullOrEmpty(place.LatLong))
                    {
                        c.LatLong = place.LatLong;
                    }
                    else
                    {
                        place = await GeocodePlaceNamesAsync(c.address + " ON");
                        if(place != null)
                        {
                            c.LatLong = place.LatLong;
                        }
                    }
                }
                if(string.IsNullOrEmpty(c.LatLong))
                {
                    c.LatLong = "0,0";
                }
            }
            return concerts;
        }
        [FunctionName("FetchConcerts")]
        [StorageAccount("StorageConnection")]
        public async Task Run([TimerTrigger("59 59 23 1 * *")] TimerInfo myTimer, [Blob("concertscache/concerts.json", FileAccess.Write)] Stream fileJson, ILogger log)
        {
            try
            {
                var json = await GetConcerts();
                var concerts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Concert>>(json);
                try
                {
                    concerts = await ProcessVenueAddresses(concerts);
                    json = Newtonsoft.Json.JsonConvert.SerializeObject(concerts);
                }
                catch (Exception ex)
                {
                    //should log and report here
                }
                byte[] byteArray = Encoding.UTF8.GetBytes(json);
                fileJson.Write(byteArray, 0, byteArray.Length);
            }
            catch (Exception ex)
            {
                //should log and report here
            }
        }
        private static async Task<GeocodedEntry> GeocodePlaceNamesAsync(string placename)
        {

            var apiKey = "AIzaSyBgPVXWlSWmvsyTkvObWAmeGoTTKPDpJVk"; // Replace with your actual API key
            var baseUri = new Uri("https://maps.googleapis.com/maps/api/geocode/json");
            GeocodedEntry geocodedEntry = new GeocodedEntry();
            var geocodedData = new List<GeocodedEntry>();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var queryParams = $"address={Uri.EscapeDataString(placename)}&key={apiKey}";
                    var response = await httpClient.GetAsync($"{baseUri}?{queryParams}");
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    // Parse the JSON response
                    // You can use a JSON library like Newtonsoft.Json to deserialize the data
                    // Example: var result = JsonConvert.DeserializeObjecet<GeocodingResult>(content);

                    // For demonstration purposes, let's assume the data is already parsed
                    if (!string.IsNullOrEmpty(content))
                    {
                        var geocodedEntries = Newtonsoft.Json.JsonConvert.DeserializeObject<GeocodedEntries>(content);
                        if (geocodedEntries != null && geocodedEntries.results != null && geocodedEntries.results.Count() > 0)
                        {
                            geocodedEntry = new GeocodedEntry()
                            {
                                Address = geocodedEntries.results.First().formatted_address,
                                LatLong = geocodedEntries.results.First().geometry.location.lat + "," + geocodedEntries.results.First().geometry.location.lng,
                                Name = placename
                            };
                        }
                        else
                        {
                            Console.WriteLine($"Couldn't get an address for location: {placename}");
                        }
                    }
                    return geocodedEntry;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new GeocodedEntry();
                }
            }

        }
        [FunctionName("RetrieveConcerts")]
        [StorageAccount("StorageConnection")]
        public async Task<IEnumerable<Concert>> RetrieveConcerts([HttpTrigger] HttpRequest req, [Blob("concertscache/concerts.json", FileAccess.Read)] Stream fileJson, ILogger log)
        {
            fileJson.Position = 0;
            List<Concert> concerts;
            using (StreamReader reader = new StreamReader(fileJson, Encoding.UTF8))
            {
                concerts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Concert>>(reader.ReadToEnd());
            }
            return concerts;
        }
        private async Task<string> GetConcerts()
        {
            var _client = new HttpClient();
            var url = "https://www.thewholenote.com/ludwig/listings/search.php";
            var location = new Toronto.Concerts.Data.Location() { id = "1", name = "City of Toronto" };
            var today = DateTime.Now.Date.ToUniversalTime();
            var concertQuery = new ConcertQuery()
            {
                startDate = (int)DateTimeOffset.Now.ToUnixTimeSeconds(),
                endDate = (int)DateTimeOffset.Now.AddDays(30).ToUnixTimeSeconds(),
                tags = new List<string>().ToArray(),
                enhancedOnly = false,
                locations = new Toronto.Concerts.Data.Location[] { location, new Toronto.Concerts.Data.Location() { id = "2", name = "Halton-Peel Regions" }, new Toronto.Concerts.Data.Location() { id = "3", name = "York Region" }, new Toronto.Concerts.Data.Location() { id = "4", name = "Durham Region" } }
            };
            var formContent = new FormUrlEncodedContent(new[]
                                                        {
                                                                new KeyValuePair<string, string>("query", Newtonsoft.Json.JsonConvert.SerializeObject(concertQuery))
                                                            });

            var concertsResponse = await _client.PostAsync("https://www.thewholenote.com/ludwig/listings/search.php", formContent);
            var json = await concertsResponse.Content.ReadAsStringAsync();
            return json;
        }
    }
}
