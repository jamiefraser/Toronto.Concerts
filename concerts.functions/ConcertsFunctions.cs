using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
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
        [FunctionName("FillCacheManually")]
        [StorageAccount("StorageConnection")]
        public async Task TestFetch([HttpTrigger] HttpRequest req, [Blob(blobPath: "concertscache/concerts.json", FileAccess.Write)] BlockBlobClient fileJson, ILogger log)
        {
            try
            {
                var json = await GetConcerts();
                byte[] byteArray = Encoding.UTF8.GetBytes(json);
                {
                    var accessCondition = new AccessCondition();
                    var blobRequestOptions = new BlockBlobOpenWriteOptions();

                    var operationContext = new OperationContext();

                    // ETag access condition is used so that it will not cause any issue due to ongoing concurrent modification on the same blob

                    accessCondition.IfMatchETag = "*";
                    MemoryStream ms = new MemoryStream(byteArray);
                    var result = await fileJson.UploadAsync(ms);
                    System.Diagnostics.Debug.WriteLine(result.GetRawResponse().Status);
                    //var s = await fileJson.OpenWriteAsync(true,blobRequestOptions);
                    //await s.WriteAsync(byteArray, 0, byteArray.Length);
                    //await s.FlushAsync();
                    ////await fileJson.FlushAsync();
                    //s.Close();
                }
                System.Diagnostics.Debug.WriteLine(json);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [FunctionName("FetchConcerts")]
        [StorageAccount("StorageConnection")]
        public async Task Run([TimerTrigger("* * */23 * * *")] TimerInfo myTimer, [Blob("concertscache/concerts.json", FileAccess.Write)] Stream fileJson, ILogger log)
        {
            try
            {
                var json = await GetConcerts();
                byte[] byteArray = Encoding.UTF8.GetBytes(json);
                fileJson.Write(byteArray, 0, byteArray.Length);
                System.Diagnostics.Debug.WriteLine(json);
            }
            catch (Exception ex)
            {
                //should log and report here
            }
        }
        [FunctionName("RetrieveConcerts")]
        [StorageAccount("StorageConnection")]
        public async Task<IEnumerable<Concert>>RetrieveConcerts([HttpTrigger] HttpRequest req, [Blob("concertscache/concerts.json", FileAccess.Read)] Stream fileJson, ILogger log)
        {
            fileJson.Position = 0;
            List<Concert> concerts;
            using (StreamReader reader = new StreamReader(fileJson, Encoding.UTF8))
            {
                concerts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Concert>>( reader.ReadToEnd());
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
            System.Diagnostics.Debug.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(concertQuery));
            //{"query":"","listingId":null,"enhancedOnly":false,"startDate":1681358400,"endDate":1683432000,"locations":[{"id":"1","name":"City+of+Toronto"},{"id":"2","name":"Halton-Peel+Regions"},{"id":"3","name":"York+Region"},{"id":"4","name":"Durham+Region"}],"tags":["Chamber","Choral","Early/Baroque","Musical+Theatre","New+Music","Organ","Orchestra","Piano","Solo+Voice","Strings","Religious+Service"]}
            var formContent = new FormUrlEncodedContent(new[]
                                                        {
                                                                new KeyValuePair<string, string>("query", Newtonsoft.Json.JsonConvert.SerializeObject(concertQuery))
                                                            });

            var concertsResponse = await _client.PostAsync("https://www.thewholenote.com/ludwig/listings/search.php", formContent);
            var json = await concertsResponse.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine(json);
            return json;
        }
    }
}
