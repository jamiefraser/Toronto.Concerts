using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toronto.Concerts.Services;

namespace Toronto.Concerts.Platforms.Windows
{
    internal class CalendarService : ICalendarService
    {
        public Task<(bool isAdded, string message)> AddEventToCalendar(DateTime startDate, DateTime endDate, string title, string description, string location)
        {
            throw new NotImplementedException();
        }
    }
}
