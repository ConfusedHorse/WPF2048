using System;
using System.Windows;
using System.Windows.Threading;
using WPF2048.ViewModel;

namespace WPF2048.View
{
    /// <summary>
    /// Interaction logic for FlipControl.xaml
    /// </summary>
    public partial class FlipControl
    {
        #region Fields

        private const int AnimationMilliseconds = FieldViewModel.AnimationMilliseconds;
        private const int AnimationSteps = 50;

        private int _oldInteger;
        private int _currentInteger;
        private int _newInteger;
        private readonly DispatcherTimer _animationTimer =
            new DispatcherTimer { Interval = TimeSpan.FromMilliseconds((double)AnimationMilliseconds / AnimationSteps) };
        private DateTime _supposedToEndAt;
        private double _currentAnimationValue;

        #endregion

        #region Properties

        public static readonly DependencyProperty IntegerProperty = DependencyProperty.Register(
            "Integer",
            typeof(int?),
            typeof(FlipControl),
            new PropertyMetadata(null, OnIntegerChanged));

        public int? Integer
        {
            get => (int?)GetValue(IntegerProperty);
            set => SetValue(IntegerProperty, value);
        }

        private static void OnIntegerChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            var flipControl = (FlipControl)dependencyObject;
            if (!flipControl.Integer.HasValue) return;

            var oldInteger = (int?)e.OldValue;
            var newInteger = flipControl.Integer.Value;

            if (newInteger == oldInteger) return;
            flipControl.AnimateInteger(oldInteger ?? 0, newInteger);
        }

        #endregion

        public FlipControl()
        {
            _animationTimer.Tick += AnimationTimerOnTick;
            InitializeComponent();
        }

        #region Private Methods

        private void AnimateInteger(int oldInteger, int newInteger)
        {
            _oldInteger = _animationTimer.IsEnabled ? _currentInteger : oldInteger;
            _newInteger = newInteger;

            _animationTimer.Stop();
            _currentAnimationValue = 0.0;
            _supposedToEndAt = DateTime.Now.AddMilliseconds(AnimationMilliseconds);
            _animationTimer.Start();
        }

        private void AnimationTimerOnTick(object sender, EventArgs eventArgs)
        {
            if (DateTime.Now < _supposedToEndAt)
            {
                _currentAnimationValue += 1.0 / AnimationSteps;
                _currentInteger = (int)Logarithmic(_currentAnimationValue, _oldInteger, _newInteger);
                if (_currentInteger >= _newInteger || _currentAnimationValue >= AnimationSteps) _animationTimer.Stop();
                Text = _currentInteger.ToString();
            }
            else
            {
                Text = _newInteger.ToString();
                _animationTimer.Stop();
            }
        }

        private static double Logarithmic(double x, double start, double stop)
        {
            return start + (Math.Log10(x + 0.1) + 1) * (stop - start); ;
        }

        #endregion
    }
}
