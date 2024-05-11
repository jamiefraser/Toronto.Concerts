using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Toronto.Concerts.MAUI.ValueConverters
{
    public class PerformerToImageConverter : IValueConverter
    {
        private string getImageNameForTile(Data.Concert concert)
        {
            if (string.IsNullOrEmpty(concert.performers))
            {
                System.Diagnostics.Debug.WriteLine("empty performers");
            }
            if (concert.performers.Contains("choir", StringComparison.InvariantCultureIgnoreCase) || concert.performers.Contains("elmer iseler", StringComparison.InvariantCultureIgnoreCase) || concert.performers.Contains("singers", StringComparison.InvariantCultureIgnoreCase) || concert.presenter.Contains("singers", StringComparison.InvariantCultureIgnoreCase) || concert.presenter.Contains("choir", StringComparison.InvariantCultureIgnoreCase))
                return  "choir.png";
            if (concert.performers.Contains("orchestra", StringComparison.InvariantCultureIgnoreCase) || concert.presenter.Contains("orchestra", StringComparison.InvariantCultureIgnoreCase))
                return "orchestra.png";
            if (concert.performers.Contains("symphony", StringComparison.InvariantCultureIgnoreCase) || concert.presenter.Contains("symphony", StringComparison.InvariantCultureIgnoreCase))
                return "orchestra.png";
            if (concert.performers.Contains("opera", StringComparison.InvariantCultureIgnoreCase) || concert.presenter.Contains("opera", StringComparison.InvariantCultureIgnoreCase))
                return "opera.png";
            if (concert.performers.Contains("piano", StringComparison.InvariantCultureIgnoreCase) || concert.presenter.Contains("piano", StringComparison.InvariantCultureIgnoreCase) || concert.title.Contains("piano", StringComparison.InvariantCultureIgnoreCase))
                return "piano.png";
            if (concert.performers.Contains("organ", StringComparison.InvariantCultureIgnoreCase) || concert.presenter.Contains("organ", StringComparison.InvariantCultureIgnoreCase))
                return "pipeorgan.png";
            if (concert.performers.Contains("chamber", StringComparison.InvariantCultureIgnoreCase) || concert.title.Contains("chamber", StringComparison.InvariantCultureIgnoreCase) || concert.performers.Contains("quartet", StringComparison.InvariantCultureIgnoreCase) || concert.title.Contains("quartet", StringComparison.InvariantCultureIgnoreCase) || concert.performers.Contains("ensemble", StringComparison.InvariantCultureIgnoreCase) || concert.title.Contains("ensemble", StringComparison.InvariantCultureIgnoreCase) || concert.performers.Contains("consort", StringComparison.InvariantCultureIgnoreCase))
                return "chambermusic.png";

            return "genericclassicalmusic.png";
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            try
            {
                var concert = (Data.Concert)value;
                return getImageNameForTile((Data.Concert)value);
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
