using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toronto.Concerts.MAUI.ValueConverters
{
    public class ScreenFullHeightMinusOneControlValueConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            ContentView oneControl = parameter as ContentView;
            if(oneControl != null)
            {
                return (double)value - oneControl.Height;
            }
            else
            {
                return null;
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {

            throw new NotImplementedException();
        }
    }
}
