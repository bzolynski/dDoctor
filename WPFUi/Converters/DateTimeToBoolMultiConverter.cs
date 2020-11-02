using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace WPFUi.Converters
{
    public class DateTimeToBoolMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2)
                return true;
            else
            {
                if (values[0] is IEnumerable<DateTime> && values[1] is DateTime)
                {
                    var avaliebleDates = (IEnumerable<DateTime>)values[0];
                    var date = (DateTime)values[1];

                    // If passed date is in avaliebleDates return true = date is not blacked
                    return avaliebleDates?.Contains(date) ?? true;
                }

                return false;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}
