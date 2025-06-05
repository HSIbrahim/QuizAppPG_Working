using System.Globalization;

namespace QuizAppPG.Utilities.Converters
{
    public class DifficultyLevelConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DifficultyLevel difficultyLevel)
            {
                return difficultyLevel switch
                {
                    DifficultyLevel.Easy => "Lätt",
                    DifficultyLevel.Medium => "Medel",
                    DifficultyLevel.Hard => "Svår",
                    _ => value.ToString()
                };
            }
            return value?.ToString();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                if (Enum.TryParse(typeof(DifficultyLevel), stringValue, true, out var result))
                {
                    return (DifficultyLevel)result;
                }
                return DifficultyLevel.Easy;
            }
            return DifficultyLevel.Easy;
        }
    }
}