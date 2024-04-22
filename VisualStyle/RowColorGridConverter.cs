using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace voicio.VisualStyle
{
    public class RowColorGridConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool gridValue)
            {
                switch (gridValue)
                {
                    case true:
                        return new SolidColorBrush(Colors.Purple);
                    case false:
                        return new SolidColorBrush(Colors.Transparent);
                }
            }
            return new SolidColorBrush(Colors.Transparent);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
