using System.Globalization;
using System.Windows.Data;

namespace AnalogClock.Controls;

/// <summary>
/// Преобразует секунды в угол часов.
/// </summary>
[ValueConversion(typeof(DateTime), typeof(int))]
public class SecondsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var date = (DateTime)value;
        return date.Second * 6;
    }

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}

/// <summary>
/// Преобразует минуты в угол часов.
/// </summary>
[ValueConversion(typeof(DateTime), typeof(int))]
public class MinutesConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var date = (DateTime)value;
        return (date.Minute * 6) + (date.Second * 0.1);
    }

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}

/// <summary>
/// Преобразует часы в угол часов.
/// </summary>
[ValueConversion(typeof(DateTime), typeof(int))]
public class HoursConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var date = (DateTime)value;
        return (date.Hour * 30) + (date.Minute / 2);
    }

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}
