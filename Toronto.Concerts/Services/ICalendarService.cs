using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toronto.Concerts.Services
{
    public interface ICalendarService
    {
        Task<(bool isAdded, string message)> AddEventToCalendar(DateTime startDate, DateTime endDate, string title, string description, string location);
    }
}
