using System.Globalization;

namespace DartsCounter.Converters
{
    public class MultiplierToBorderThicknessConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int multiplier && parameter is string target)
            {
                if (int.TryParse(target, out int targetMultiplier))
                {
                    return multiplier == targetMultiplier ? 3.0 : 0.0;
                }
            }
            return 0.0;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
