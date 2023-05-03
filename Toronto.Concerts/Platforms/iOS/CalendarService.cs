#if IOS
using EventKit;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toronto.Concerts.Platforms.iOS;
using Toronto.Concerts.Services;
namespace Toronto.Concerts.Platforms.iOS
{
    public class CalendarService : ICalendarService
    {
        public async Task<(bool isAdded, string message)> AddEventToCalendar(DateTime startDate, DateTime endDate, string title, string description, string location)
        {
            bool isEventAdded = false;
            string message = string.Empty;
            try
            {
                var requestAccessResponse = await AppConstant.App.Current.EventStore.RequestAccessAsync(EKEntityType.Event);

                if (requestAccessResponse.Item1)
                {
                    NSDate nsStartDate = AppConstant.DateTimeToNSDate(startDate);
                    NSDate nsEndDate = AppConstant.DateTimeToNSDate(endDate);
                    NSPredicate query = AppConstant.App.Current.EventStore.PredicateForEvents(nsStartDate, nsEndDate, null);

                    EKEvent newEvent = EKEvent.FromStore(AppConstant.App.Current.EventStore);
                    var events = AppConstant.App.Current.EventStore.EventsMatching(query).ToList();
                    if (events?.Count > 0)
                    {
                        bool isExist = events.Any(f => f.Title == title && f.Notes == description && f.Location == location);

                        if (isExist)
                        {
                            message = "This Event already exist in your calendar.";
                            isEventAdded = true;
                        }
                    }

                    if (!isEventAdded)
                    {
                        newEvent.Calendar = AppConstant.App.Current.EventStore.DefaultCalendarForNewEvents;
                        newEvent.StartDate = nsStartDate;
                        newEvent.EndDate = nsEndDate;
                        newEvent.Title = title;
                        newEvent.Location = location;
                        newEvent.Notes = description;

                        newEvent.AddAlarm(EKAlarm.FromDate(AppConstant.DateTimeToNSDate(startDate.AddHours(-2))));
                        NSError et;
                        bool isSaved = AppConstant.App.Current.EventStore.SaveEvent(newEvent, EKSpan.ThisEvent, true, out et);
                        if (isSaved)
                        {
                            message = "Event Added to your calendar.";
                            isEventAdded = true;
                        }
                        else
                        {
                            if (et != null)
                            {
                                message = et.Description;
                            }
                        }
                    }
                }
                else
                {
                    message = "Calendar Permission required to save event";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return (isEventAdded, message);
        }
    }
}
#endif