using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace WPFUi.Converters
{
    public class DateToBoolConverter : IValueConverter
    {
        private readonly DateTime minDatetime = new DateTime();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (DateTime)value > minDatetime ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
