using System.Windows;
using WPF2048.ViewModel;

namespace WPF2048.View
{
    /// <summary>
    /// Interaktionslogik für FieldControl.xaml
    /// </summary>
    public partial class FieldControl
    {
        private FieldViewModel _instance;

        public FieldControl()
        {
            InitializeComponent();
            
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (!(DataContext is FieldViewModel instance)) return;
            _instance = instance;

            for (var e = 0; e < FieldViewModel.ElementCount; e++)
            {
                var spielfeldElement = new ElementViewModel(e);
                BackgroundPattern.Children.Add(new ElementControl
                {
                    Margin = spielfeldElement.Margin
                });
            }
        }
    }
}
