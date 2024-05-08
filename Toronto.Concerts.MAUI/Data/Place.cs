using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toronto.Concerts.Data
{
    public class Place
    {
        public Microsoft.Maui.Devices.Sensors.Location Location { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
    }
}
