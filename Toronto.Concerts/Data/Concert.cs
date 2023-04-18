using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toronto.Concerts.Data
{
    public class Concert
    {
        public string presenter { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string repertoire { get; set; }
        public string performers { get; set; }
        public string venue { get; set; }
        public string phone { get; set; }
        public string prices { get; set; }
        public string date { get; set; }
        public string spnotes { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public DateTime DateAndTime
        {
            get
            {
                return DateTimeOffset.FromUnixTimeSeconds(long.Parse(date)).LocalDateTime;
            }
        }
    }
}
