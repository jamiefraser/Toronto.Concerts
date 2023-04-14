using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toronto.Concerts.Data
{

    public class ConcertQuery
    {
        public string query { get; set; }
        public object listingId { get; set; }
        public bool enhancedOnly { get; set; }
        public int startDate { get; set; }
        public int endDate { get; set; }
        public Location[] locations { get; set; }
        public string[] tags { get; set; }
    }

    public class Location
    {
        public string id { get; set; }
        public string name { get; set; }
    }

}
