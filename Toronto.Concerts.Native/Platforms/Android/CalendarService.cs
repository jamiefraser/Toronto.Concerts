using Android.Content;
using Android.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toronto.Concerts.Services;

namespace Toronto.Concerts.Platforms.Android
{
    public class CalendarService : ICalendarService
    {
        public async Task<(bool isAdded, string message)> AddEventToCalendar(DateTime startDate, DateTime endDate, string title, string description, string location)
        {
            bool isEventAdded = true;
            string message = string.Empty;
            try
            {
                Intent eventValues = new Intent(Intent.ActionInsert);
                eventValues.SetData(CalendarContract.Events.ContentUri);
                eventValues.AddFlags(ActivityFlags.NewTask);
                eventValues.PutExtra(CalendarContract.Events.InterfaceConsts.CalendarId, 2);
                eventValues.PutExtra(CalendarContract.Events.InterfaceConsts.Title, title);
                eventValues.PutExtra(CalendarContract.Events.InterfaceConsts.Description, description);
                eventValues.PutExtra(CalendarContract.ExtraEventBeginTime, AppConstant.GetDateTimeMS(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute));
                eventValues.PutExtra(CalendarContract.ExtraEventEndTime, AppConstant.GetDateTimeMS(endDate.Year, endDate.Month, endDate.Day, endDate.Hour, endDate.Minute));
                eventValues.PutExtra(CalendarContract.Events.InterfaceConsts.EventTimezone, "UTC");
                eventValues.PutExtra(CalendarContract.Events.InterfaceConsts.EventEndTimezone, "UTC");
                eventValues.PutExtra(CalendarContract.Events.InterfaceConsts.EventLocation, location);

                Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.StartActivity(eventValues);
            }
            catch (Exception ex)
            {
                isEventAdded = false;
            }
            return (isEventAdded, message);
        }
    }
}
