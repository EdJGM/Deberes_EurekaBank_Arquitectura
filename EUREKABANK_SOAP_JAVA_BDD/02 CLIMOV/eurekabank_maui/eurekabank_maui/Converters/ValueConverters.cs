using System.Globalization;

namespace Eurekabank_Maui.Converters
{
    /// <summary>
    /// Converter para verificar si un valor es igual al parámetro
    /// </summary>
    public class EqualConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value?.Equals(parameter) ?? false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converter para verificar si un valor es mayor que cero
    /// </summary>
    public class GreaterThanZeroConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int intValue)
                return intValue > 0;
            
            if (value is double doubleValue)
                return doubleValue > 0;
            
            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converter para invertir un valor booleano
    /// </summary>
    public class InverseBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
                return !boolValue;
            
            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
                return !boolValue;
            
            return false;
        }
    }
}
