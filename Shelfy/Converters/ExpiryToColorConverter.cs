using System.Globalization;
using Shelfy.Core;

namespace Shelfy.Converters;

public class ExpiryToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is PantryItem item)
        {
            if (item.IsExpired) return Colors.Red;
            if (item.IsExpiringSoon) return Colors.OrangeRed;
        }
        return Colors.Black;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}