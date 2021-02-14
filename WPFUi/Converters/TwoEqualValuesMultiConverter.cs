using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using WPFUi.Models;

namespace WPFUi.Converters
{
    public class TwoEqualValuesMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => Object.Equals(values[0], values[1]);

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
