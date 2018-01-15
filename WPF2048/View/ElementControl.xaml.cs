using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;
using WPF2048.ViewModel;

namespace WPF2048.View
{
    /// <summary>
    /// Interaktionslogik für ElementControl.xaml
    /// </summary>
    public partial class ElementControl
    {
        private Thickness _oldMargin;
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

            if (!_instance.Animate) return;
            instance.PropertyChanged += InstanceOnPropertyChanged;
            AnimateSpawn();
        }

        private void AnimateSpawn()
        {
            var da = new DoubleAnimation(0, 1, FieldViewModel.AnimationDuration);
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
            BeginStoryboard(sb);

            _oldMargin = _instance.Margin;
        }
    }
}
