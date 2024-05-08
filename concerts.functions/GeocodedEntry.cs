using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace concerts.functions
{
    class GeocodedEntry
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string LatLong { get; set; }
    }
    class Location
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
