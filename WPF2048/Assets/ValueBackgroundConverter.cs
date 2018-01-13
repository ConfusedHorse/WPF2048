using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WPF2048.Assets
{
    public class ValueBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int val))
                return Brushes.Black;

            try
            {
                var exp = Math.Log(val, 2);
                return ColorHelper.GetBackground(exp);
            }
            catch (Exception)
            {
                return Brushes.Black;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}