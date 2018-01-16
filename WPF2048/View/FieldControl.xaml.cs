using System.Windows;
using WPF2048.ViewModel;

namespace WPF2048.View
{
    /// <summary>
    /// Interaktionslogik für FieldControl.xaml
    /// </summary>
    public partial class FieldControl
    {
        public FieldControl()
        {
            InitializeComponent();
            
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            CreateField();
        }

        internal void CreateField()
        {
            AdjustSize();
            BackgroundPattern.Children.Clear();
            for (var e = 0; e < FieldViewModel.ElementCount; e++)
            {
                var spielfeldElement = new ElementViewModel(e);
                BackgroundPattern.Children.Add(new ElementControl
                {
                    Margin = spielfeldElement.Margin
                });
            }
        }

        private void AdjustSize()
        {
            BackgroundPattern.Width = FieldViewModel.FieldSize;
            BackgroundPattern.Height = FieldViewModel.FieldSize;
            FieldBorder.Width = FieldViewModel.FieldSize + FieldBorder.BorderThickness.Left * 2;
            FieldBorder.Height = FieldViewModel.FieldSize + FieldBorder.BorderThickness.Left * 2;
        }
    }
}
