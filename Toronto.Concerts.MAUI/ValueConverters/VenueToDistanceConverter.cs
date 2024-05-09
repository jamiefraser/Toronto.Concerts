using Geolocation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toronto.Concerts.MAUI.Services;

namespace Toronto.Concerts.MAUI.ValueConverters
{
    public class VenueToDistanceConverter : IValueConverter
    {
        private readonly UserLocationService userLocationService;
        public VenueToDistanceConverter(IServiceProvider serviceProvider)
        {
            userLocationService = serviceProvider.GetRequiredService<UserLocationService>();
        }
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var locationString = value as string;
            var lat = double.Parse(locationString.Split(',')[0]);
            var lng = double.Parse(locationString.Split(',')[1]);
            var venueCoordinate = new Coordinate(lat, lng);
            var currentCoordinate = new Coordinate(userLocationService.Latitude, userLocationService.Longitude);
            var distance = GeoCalculator.GetDistance(venueCoordinate, currentCoordinate, 1, DistanceUnit.Kilometers);
            return distance.ToString("0.00") + " km";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
