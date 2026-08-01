using System.Globalization;

namespace Shelfy.Converters;

public class BoolToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var isExpired = value is bool b && b;
        return isExpired ? Colors.Red : Colors.Black;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
