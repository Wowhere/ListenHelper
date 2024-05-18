using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace voicio.ViewModels
{
    public class TreeDataGridConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "value.";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
