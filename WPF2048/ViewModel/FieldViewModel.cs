using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using BlurryControls.DialogFactory;
using BlurryControls.Internals;
using GalaSoft.MvvmLight;
using WPF2048.Assets;
using WPF2048.Module;

namespace WPF2048.ViewModel
{
    public class FieldViewModel : ViewModelBase
    {
        #region Fields

        private bool _winPossible;
        private int _moves;
        private int _score;
        private bool _isBusy;

        #endregion

        public FieldViewModel()
        {
            SystemParameters.StaticPropertyChanged += AdjustElementColors;
            Singleton.CurrentRoot = Properties.Settings.Default.ElementRoot;
            Singleton.CurrentHighScore = Properties.Settings.Default.HighScore;
            ResetGame();
        }

        ~FieldViewModel()
        {
            Properties.Settings.Default.Save();
        }

        #region Properties

        /// <summary>
        /// Represents all elements in the field
        /// </summary>
        public ObservableCollection<ElementViewModel> Elements { get; } = new ObservableCollection<ElementViewModel>();
        private ElementViewModel[] ObsoleteElements => Elements.Where(e => e.Obsolete).ToArray();
        private ElementViewModel[] ActiveElements => Elements.Where(e => !e.Obsolete).ToArray();

        /// <summary>
        /// There are possibles moves to be made
        /// </summary>
        public bool WinPossible
        {
            get => _winPossible;
            private set
            {
                _winPossible = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Displays how many moves have been performed
        /// </summary>
        public int Moves
        {
            get => _moves;
            private set
            {
                _moves = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Sum of the value of all elements
        /// </summary>
        public int Score
        {
            get => _score;
            private set
            {
                _score = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Blocks user interaction as long as calculations are made
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            private set
            {
                _isBusy = value;
                RaisePropertyChanged();
            }
        }

        public int ElementRoot
        {
            get => Singleton.CurrentRoot;
            set
            {
                Singleton.CurrentRoot = value;
                Properties.Settings.Default.ElementRoot = value;
                RaisePropertyChanged();
                RaisePropertyChanged(() => FieldSize);
            }
        }

        public int HighScore
        {
            get => Singleton.CurrentHighScore;
            set
            {
                Singleton.CurrentHighScore = value;
                Properties.Settings.Default.HighScore = value;
                RaisePropertyChanged();
                RaisePropertyChanged(() => HighScore);
            }
        }

        #endregion

        #region Static Properties

        public static int DefaultFontSize = 30;
        
        public const int StartValue = 2; // this is adjustable
        public const int AddCount = 2;
        public const double ElementSize = 150;
        public const int AnimationMilliseconds = 250;

        public static int[] SizeOptions = {3, 4, 5};
        public static int[] WinningPowers = {8, 11, 16};

        public static double FieldSize => Singleton.CurrentRoot * ElementSize;
        public static int WinningPower => WinningPowers[Singleton.CurrentRoot - SizeOptions[0]];
        public static int WinningCondition => (int) Math.Pow(StartValue, WinningPower);
        public static int ElementCount => Singleton.CurrentRoot * Singleton.CurrentRoot;

        public static Duration AnimationDuration = new Duration(TimeSpan.FromMilliseconds(AnimationMilliseconds));
        public static SolidColorBrush AccentColor = Brushes.WhiteSmoke;

        #endregion

        #region Private Methods

        /// <summary>
        /// Adjusts system accent color change to elements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void AdjustElementColors(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "WindowGlassColor") return;
            foreach (var element in Elements)
                element.Value = element.Value;
        }

        /// <summary>
        /// Adds new elements to the field
        /// </summary>
        /// <param name="add"></param>
        private void AddBareValues(int add = AddCount)
        {
            var desiredCount = ActiveElements.Length + add;
            var random = new Random();

            if (desiredCount > ElementCount)
                CheckDefeatCondition();
            else
                while (ActiveElements.Length < desiredCount)
                {
                    var rnd = random.Next(0, ElementCount);
                    if (ActiveElements.FirstOrDefault(se => se.Index == rnd) == null)
                        Elements.Add(new ElementViewModel(rnd, StartValue));
                }
        }

        /// <summary>
        /// Moves elements or merges them with elements to the top
        /// </summary>
        private void PerformMoveUp()
        {
            foreach (var movingElement in Elements.OrderBy(e => e.Y).ThenBy(e => e.X))
            {
                if (movingElement.X == 0) continue;
                var collidingElement = Elements.OrderByDescending(e => e.X).FirstOrDefault(e => e.Y == movingElement.Y && e.X < movingElement.X);
                if (collidingElement != null)
                {
                    if (!collidingElement.Blocked && !collidingElement.Obsolete && collidingElement.Value == movingElement.Value)
                        MergeElements(movingElement, collidingElement);
                    else if (collidingElement.X + 1 < ElementRoot)
                        movingElement.Index = collidingElement.Index + ElementRoot;
                }
                else movingElement.Index = movingElement.Y;
            }
        }

        /// <summary>
        /// Moves elements or merges them with elements to the bottom
        /// </summary>
        private void PerformMoveDown()
        {
            foreach (var movingElement in Elements.OrderBy(e => e.Y).ThenByDescending(e => e.X))
            {
                if (movingElement.X == ElementRoot - 1) continue;
                var collidingElement = Elements.OrderBy(e => e.X).FirstOrDefault(e => e.Y == movingElement.Y && e.X > movingElement.X);
                if (collidingElement != null)
                {
                    if (!collidingElement.Blocked && !collidingElement.Obsolete && collidingElement.Value == movingElement.Value)
                        MergeElements(movingElement, collidingElement);
                    else if (collidingElement.X > 1)
                        movingElement.Index = collidingElement.Index - ElementRoot;
                }
                else movingElement.Index = ElementRoot * ElementRoot - ElementRoot + movingElement.Y;
            }
        }

        /// <summary>
        /// Moves elements or merges them with elements to the right
        /// </summary>
        private void PerformMoveRight()
        {
            foreach (var movingElement in Elements.OrderBy(e => e.X).ThenByDescending(e => e.Y))
            {
                if (movingElement.Y == ElementRoot - 1) continue;
                var collidingElement = Elements.OrderBy(e => e.Y).FirstOrDefault(e => e.X == movingElement.X && e.Y > movingElement.Y);
                if (collidingElement != null)
                {
                    if (!collidingElement.Blocked && !collidingElement.Obsolete && collidingElement.Value == movingElement.Value)
                        MergeElements(movingElement, collidingElement);
                    else if (collidingElement.Y > 1)
                        movingElement.Index = collidingElement.Index - 1;
                }
                else movingElement.Index = movingElement.X * ElementRoot + ElementRoot - 1;
            }
        }

        /// <summary>
        /// Moves elements or merges them with elements to the left
        /// </summary>
        private void PerformMoveLeft()
        {
            foreach (var movingElement in Elements.OrderBy(e => e.X).ThenBy(e => e.Y))
            {
                if (movingElement.Y == 0) continue;
                var collidingElement = Elements.OrderByDescending(e => e.Y).FirstOrDefault(e => e.X == movingElement.X && e.Y < movingElement.Y);
                if (collidingElement != null)
                {
                    if (!collidingElement.Blocked && !collidingElement.Obsolete && collidingElement.Value == movingElement.Value)
                        MergeElements(movingElement, collidingElement);
                    else if (collidingElement.Y + 1 < ElementRoot)
                        movingElement.Index = collidingElement.Index + 1;
                }
                else movingElement.Index = movingElement.X * ElementRoot;
            }
        }

        /// <summary>
        /// Merges two tiles and adjusts the score accordingly
        /// </summary>
        /// <param name="movingElement"></param>
        /// <param name="collidingElement"></param>
        private void MergeElements(ElementViewModel movingElement, ElementViewModel collidingElement)
        {
            movingElement.Index = collidingElement.Index;
            movingElement.Value *= 2;
            movingElement.Blocked = true;
            collidingElement.Obsolete = true;
            Score += movingElement.Value;
        }

        /// <summary>
        /// Remove obsolete values which have been merged into another element
        /// </summary>
        private void CleanField()
        {
            lock (this)
            {
                foreach (var obsoleteElement in ObsoleteElements)
                    Elements.Remove(obsoleteElement);
            }
        }

        /// <summary>
        /// Enable all elements to be merged again
        /// </summary>
        private void CleanBlock()
        {
            foreach (var element in Elements)
                element.Blocked = false;
        }

        /// <summary>
        /// Check if an element has reached a given value
        /// </summary>
        private void CheckWinCondition()
        {
            if (!ActiveElements.Any(e => e.Value >= WinningCondition)) return;

            WinPossible = true;
            AdjustHighScore();
            var result = BlurBehindMessageBox.Show(Properties.Resources.WinBody, Properties.Resources.WinHeader,
                BlurryDialogButton.YesNoCancel);
            if (result == BlurryDialogResult.Yes) Singleton.FieldViewModel.ResetGame();
        }

        /// <summary>
        /// Check if any possible moves can be made
        /// </summary>
        private void CheckDefeatCondition()
        {
            if (ActiveElements.Length < ElementCount ||
                ActiveElements.Select(element => ActiveElements.Where(e =>
                        e.Value == element.Value &&
                        (Math.Abs(e.X - element.X) == 1 && e.Y == element.Y ||
                         Math.Abs(e.Y - element.Y) == 1 && e.X == element.X)).ToArray())
                    .Any(possibleCollisions => possibleCollisions.Any())) return;

            WinPossible = false;
            AdjustHighScore();
            var result = BlurBehindMessageBox.Show(Properties.Resources.DefeatBody, Properties.Resources.DefeatHeader,
                BlurryDialogButton.YesNoCancel);
            if (result == BlurryDialogResult.Yes) Singleton.FieldViewModel.ResetGame();
        }

        /// <summary>
        /// Check if any changes were made to prevent exploit spawn
        /// </summary>
        /// <param name="previousState"></param>
        /// <returns></returns>
        private bool CheckChanges(IReadOnlyCollection<ElementViewModel> previousState)
        {
            return ActiveElements.Length != previousState.Count ||
                   !previousState.All(previousElement => ActiveElements.Any(previousElement.Equals));
        }

        /// <summary>
        /// Adjusts the current highscore
        /// </summary>
        private void AdjustHighScore()
        {
            if (Score > Properties.Settings.Default.HighScore)
                HighScore = Score;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clear all values and reset to dafault conditions
        /// </summary>
        public void ResetGame()
        {
            AdjustHighScore();

            Elements.Clear();
            Moves = 0;
            Score = AddCount * StartValue;
            WinPossible = true;
            AddBareValues();
        }

        /// <summary>
        /// A key Left | Right | Up | Down | W | A | S | D is pressed
        /// </summary>
        /// <param name="direction"></param>
        public void KeyAction(Direction direction)
        {
            if (!_winPossible || _isBusy) return;
            IsBusy = true;

            var previousState = 
                Elements.Select(e => new ElementViewModel(e.Index, e.Value)).ToArray();

            switch (direction)
            {
                case Direction.Up:    PerformMoveUp(); break;
                case Direction.Down:  PerformMoveDown(); break;
                case Direction.Right: PerformMoveRight(); break;
                case Direction.Left:  PerformMoveLeft(); break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
            
            if (CheckChanges(previousState))
            {
                Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(AnimationMilliseconds));
                    Application.Current.Dispatcher.Invoke(() => Singleton.FieldViewModel.CleanField());
                });
                CleanBlock();

                Moves++;
                AddBareValues(1);

                CheckWinCondition();
                CheckDefeatCondition();
            }

            IsBusy = false;
        }

        #endregion
    }
}