using System;
using System.Globalization;
using System.Windows.Data;

namespace NationalInstruments.Examples.ShowAllHardware
{
    public class AliasConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Display the user alias if it is not null or empty.
            // Otherwise, display the resource name. If they are both
            // null or empty, leave the cell empty in the grid.
            return Array.Find(values, name => !string.IsNullOrEmpty(name as string));
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
