using System.Globalization;

namespace DartsCounter.Converters
{
    public class MultiplierToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int multiplier && parameter is string target)
            {
                if (int.TryParse(target, out int targetMultiplier))
                {
                    return multiplier == targetMultiplier ? Colors.Orange : Colors.Gray;
                }
            }
            return Colors.Gray;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
