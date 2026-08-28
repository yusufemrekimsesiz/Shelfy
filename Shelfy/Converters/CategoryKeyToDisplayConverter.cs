using System.Globalization;
using Shelfy.Localization;

namespace Shelfy.Converters;

public class CategoryKeyToDisplayConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string key)
            return CategoryLocalizer.GetDisplayName(key);
        return string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}