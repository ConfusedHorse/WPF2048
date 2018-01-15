using System.Windows;
using GalaSoft.MvvmLight;

namespace WPF2048.ViewModel
{
    public class ElementViewModel : ViewModelBase
    {
        private int _index;
        private int _value;
        private bool _animate;
        private int _fontSize = 30; // TODO adjust according to value
        private bool _blocked;
        private bool _obsolete;

        public ElementViewModel(int initial = 0, int value = 0, bool animate = false)
        {
            _index = initial;
            _value = value;
            _animate = animate;
        }

        #region Properties

        
        public int Value
        {
            get => _value;
            internal set
            {
                _value = value;
                RaisePropertyChanged();
            }
        }

        public int FontSize
        {
            get => _fontSize;
            internal set
            {
                _fontSize = value;
                RaisePropertyChanged();
            }
        }

        public int Index
        {
            get => _index;
            internal set
            {
                _index = value;
                RaisePropertyChanged();
                RaisePropertyChanged(() => X);
                RaisePropertyChanged(() => Y);
                RaisePropertyChanged(() => Margin);
            }
        }

        public Thickness Margin => new Thickness(Left, Top, 
            FieldViewModel.FieldSize - Left - FieldViewModel.ElementSize, FieldViewModel.FieldSize - Top - FieldViewModel.ElementSize);

        // ReSharper disable once PossibleLossOfFraction
        public int X => _index / Properties.Settings.Default.ElementRoot;

        public int Y => _index % Properties.Settings.Default.ElementRoot;

        public double Top => X * FieldViewModel.ElementSize;

        public double Left => Y * FieldViewModel.ElementSize;

        public bool Animate
        {
            get => _animate;
            set
            {
                _animate = value;
                RaisePropertyChanged();
            }
        }

        public bool Blocked
        {
            get => _blocked;
            set
            {
                _blocked = value;
                RaisePropertyChanged();
            }
        }

        public bool Obsolete
        {
            get => _obsolete;
            set
            {
                _obsolete = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public override string ToString()
        {
            return $"{Index:0#}: ({X:0#}|{Y:0#})='{Value:0000#}'";
        }

        protected bool Equals(ElementViewModel other)
        {
            return _index == other._index && _value == other._value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == GetType() && Equals((ElementViewModel) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_index * 397) ^ _value;
            }
        }
    }
}