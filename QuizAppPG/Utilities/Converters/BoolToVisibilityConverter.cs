using System.Globalization;

namespace QuizAppPG.Utilities.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? "Visible" : "Collapsed";
            }
            return "Collapsed";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture)
        {
            throw new NotImplementedException();
        }
    }
}