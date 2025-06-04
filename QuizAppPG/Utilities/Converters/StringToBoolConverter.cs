using System.Globalization;
using Microsoft.Maui.Controls; // For IValueConverter

namespace QuizAppPG.Utilities.Converters
{
    public class StringToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
        {
            return value != null && value.Equals(parameter);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture)
        {
            if (value is bool isChecked && isChecked)
            {
                return parameter;
            }
            return null;
        }
    }
}