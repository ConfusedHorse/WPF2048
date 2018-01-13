using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WPF2048.Assets
{
    public class ValueForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int val))
                return Brushes.White;

            try
            {
                var exp = Math.Log(val, 2);
                var b = ColorHelper.GetBackground(exp).Color;
                return b.R + b.G + b.B > 2 * 127 ? Brushes.Black : Brushes.White;
            }
            catch (Exception)
            {
                return Brushes.White;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}