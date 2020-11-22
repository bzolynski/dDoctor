using System;
using System.Globalization;
using System.Windows.Data;
using WPFUi.Models;

namespace WPFUi.Converters
{
    public class MultiBindingToBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var result = false;
            if (values[0] is AppointmentViewReservationModel)
                result = values[0] == values[1];
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
