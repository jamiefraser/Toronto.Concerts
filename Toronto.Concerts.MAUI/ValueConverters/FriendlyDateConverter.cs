using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toronto.Concerts.MAUI.ValueConverters
{
    public class FriendlyDateConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value.GetType().Equals(typeof(DateTime)))
            {
                var date = (DateTime)value;
                if(date.Date == DateTime.Today)
                {
                    return "Today";
                }
                if(date.Date == DateTime.Today.AddDays(1))
                {
                    return "Tomorrow";
                }
                return date.ToString("MMM dd");
            }
            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null) return value;
            if(value.GetType().Equals(typeof(string)))
            {
                var date = (string)value;
                if(date == "Today")
                {
                    return DateTime.Today;
                }
                if(date == "Tomorrow")
                {
                    return DateTime.Today.AddDays(1);
                }
                return DateTime.Parse(date);
            }
            return value;
        }
    }
}
