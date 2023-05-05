using System;
using Toronto.Concerts.Services;

namespace Toronto.Concerts.Native.Platforms.MacCatalyst
{
	public class CalendarService : ICalendarService
	{
		public CalendarService()
		{
		}

        public Task<(bool isAdded, string message)> AddEventToCalendar(DateTime startDate, DateTime endDate, string title, string description, string location)
        {
            throw new NotImplementedException();
        }
    }
}

