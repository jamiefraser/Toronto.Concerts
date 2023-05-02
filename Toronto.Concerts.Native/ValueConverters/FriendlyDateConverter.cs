using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Toronto.Concerts.Native.ValueConverters
{
    public class FriendlyDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime val = (DateTime)value;
            if (val.Equals(DateTime.Now.Date))
            {
                return "Today";
            }
            if(val.Equals(DateTime.Now.AddDays(1).Date))
            {
                return "Tomorrow";
            }
            return val.ToString("MMM dd");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
