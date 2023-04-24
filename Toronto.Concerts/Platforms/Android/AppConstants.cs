using System;
using Android.Content;
using Android.Provider;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Icu.Util;
using TimeZone = Android.Icu.Util.TimeZone;

namespace Toronto.Concerts.Platforms.Android
{
    public static class AppConstant
    {
        public static long GetDateTimeMS(int year, int month, int day, int hr, int min)
        {
            Calendar c = Calendar.GetInstance(TimeZone.Default);
 
            c.Set(CalendarField.DayOfMonth, day);
            c.Set(CalendarField.HourOfDay, hr);
            c.Set(CalendarField.Minute, min);
            c.Set(CalendarField.Month, month -1);
            c.Set(CalendarField.Year, year);
 
            return c.TimeInMillis;
        }
    }
}
