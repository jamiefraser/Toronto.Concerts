using System;
using System.Globalization;

namespace Toronto.Concerts.MAUI.ValueConverters
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value.GetType() == typeof(string))
            {
                var isString = string.IsNullOrEmpty((string)value) ? "empty" : (string)value;
                return isString=="empty"  ? true : Visibility.Collapsed;
            }
            return value !=null  ? true : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
