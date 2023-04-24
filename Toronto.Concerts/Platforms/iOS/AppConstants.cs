using EventKit;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toronto.Concerts.Platforms.iOS
{
    public static class AppConstant
    {
        public static NSDate DateTimeToNSDate(this DateTime date)
        {
            if (date.Kind == DateTimeKind.Unspecified)
                date = DateTime.SpecifyKind(date, DateTimeKind.Local);
            return (NSDate)date;
        }

        public class App
        {
            public static App Current
            {
                get { return current; }
            }
            private static App current;
            public EKEventStore EventStore
            {
                get { return eventStore; }
            }
            protected EKEventStore eventStore;
            static App()
            {
                current = new App();
            }
            protected App()
            {
                eventStore = new EKEventStore();
            }
        }
    }
}
