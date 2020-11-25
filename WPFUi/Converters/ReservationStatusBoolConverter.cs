using Domain.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace WPFUi.Converters
{
    public class ReservationStatusBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = true;
            if (value is ReservationStatus v && parameter is ReservationStatus p)
            {
                result = !(v == p);
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
