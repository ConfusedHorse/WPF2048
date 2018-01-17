using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using WPF2048.Module;
using WPF2048.ViewModel;

namespace WPF2048.View
{
    /// <summary>
    /// Interaction logic for DashboardControl.xaml
    /// </summary>
    public partial class DashboardControl
    {
        public DashboardControl()
        {
            InitializeComponent();
            SetToolTip();
            Singleton.FieldViewModel.PropertyChanged += FieldViewModelOnPropertyChanged;
        }

        private void FieldViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != nameof(FieldViewModel.ElementRoot)) return;
            SetToolTip();
        }

        private void Dashboard_OnMouseEnter(object sender, MouseEventArgs e)
        {
            FadeFromTo(Dashboard, AlternativeDashboard);
        }

        private void Dashboard_OnMouseLeave(object sender, MouseEventArgs e)
        {
            FadeFromTo(AlternativeDashboard, Dashboard);
        }

        private void FadeFromTo(FrameworkElement from, FrameworkElement to)
        {
            var fadeOut = new DoubleAnimation(from.Opacity, 0, FieldViewModel.AnimationDuration);
            var fadeIn = new DoubleAnimation(to.Opacity, 1, FieldViewModel.AnimationDuration);
            var sb = new Storyboard { Children = { fadeIn, fadeOut } };

            Storyboard.SetTargetProperty(fadeOut, new PropertyPath("(FrameworkElement.Opacity)"));
            Storyboard.SetTargetName(fadeOut, from.Name);
            Storyboard.SetTargetProperty(fadeIn, new PropertyPath("(FrameworkElement.Opacity)"));
            Storyboard.SetTargetName(fadeIn, to.Name);

            BeginStoryboard(sb);
        }

        private void SetToolTip()
        {
            AlternativeDashboard.ToolTip =
                string.Format(Properties.Resources.WinTileToolTip, FieldViewModel.WinningCondition);
        }
    }
}
