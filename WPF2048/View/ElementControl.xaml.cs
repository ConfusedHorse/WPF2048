using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;
using WPF2048.ViewModel;

namespace WPF2048.View
{
    /// <summary>
    ///     Interaction logic for ElementControl.xaml
    /// </summary>
    public partial class ElementControl
    {
        private Thickness _oldMargin;
        private double _oldOpacity;
        private ElementViewModel _instance;

        public ElementControl()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (!(DataContext is ElementViewModel instance)) return;
            _instance = instance;
            _oldMargin = Margin = _instance.Margin;
            
            instance.PropertyChanged += InstanceOnPropertyChanged;
            AnimateSpawn();
        }

        private void AnimateSpawn(double from = 0)
        {
            var da = new DoubleAnimation(from, 1, FieldViewModel.AnimationDuration);
            var sb = new Storyboard {Children = {da}};
            Storyboard.SetTargetProperty(da, new PropertyPath("(UserControl.Opacity)"));
            BeginStoryboard(sb);
        }

        private void InstanceOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != nameof(ElementViewModel.Margin)) return;
            AnimateMove();
        }

        private void AnimateMove()
        {
            var da = new ThicknessAnimation(_oldMargin, _instance.Margin, FieldViewModel.AnimationDuration);
            var sb = new Storyboard { Children = { da } };
            Storyboard.SetTargetProperty(da, new PropertyPath("(UserControl.Margin)"));
            da.Completed += ResetOpacity;
            BeginStoryboard(sb);

            _oldOpacity = Opacity;
            Opacity = 0.75;
            _oldMargin = _instance.Margin;
        }

        private void ResetOpacity(object sender, EventArgs eventArgs)
        {
            AnimateSpawn(_oldOpacity);
        }
    }
}
