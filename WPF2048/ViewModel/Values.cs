using System;
using System.Windows;
using System.Windows.Media;

namespace WPF2048.ViewModel
{
    public static class Values
    {
        private const int DefaultElementRoot = 4;
        private const int DefaultWinningPower = 12; // defaults to 2^12 = 4096

        public const int DefaultFontSize = 30;

        public const int ElementRoot = 4;
        public const int StartValue = 2;
        public const int AddCount = 2;
        public const double FieldSize = 600;

        public const int WinningPower = DefaultWinningPower + ElementRoot - DefaultElementRoot;
        public static double ElementSize = FieldSize / ElementRoot;
        public static int ElementCount = ElementRoot * ElementRoot;

        public static Duration AnimationDuration = new Duration(TimeSpan.FromSeconds(0.3));
        public static SolidColorBrush AccentColor = Brushes.WhiteSmoke;
    }
}