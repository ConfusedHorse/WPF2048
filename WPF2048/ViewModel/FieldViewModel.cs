using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            ResetGame();
        }

        #region Properties

        /// <summary>
        /// Represents all elements in the field
        /// </summary>
        public ObservableCollection<ElementViewModel> Elements { get; } = new ObservableCollection<ElementViewModel>();

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

        #endregion

        #region Private Methods

        /// <summary>
        /// Adds new elements to the field
        /// </summary>
        /// <param name="add"></param>
        private void AddBareValues(int add = Values.AddCount)
        {
            var desiredCount = Elements.Count + add;
            var random = new Random();

            if (desiredCount > Values.ElementCount) CheckDefeatCondition();
            while (Elements.Count < desiredCount)
            {
                var rnd = random.Next(0, Values.ElementCount);
                if (Elements.FirstOrDefault(se => se.Index == rnd) == null)
                    Elements.Add(new ElementViewModel(rnd, Values.StartValue, true));
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
                    if (!collidingElement.Blocked && collidingElement.Value == movingElement.Value)
                    {
                        movingElement.Index = collidingElement.Index;
                        movingElement.Value *= 2;
                        movingElement.Blocked = true;
                        collidingElement.Obsolete = true;
                    }
                    else if (collidingElement.X + 1 < Values.ElementRoot)
                    {
                        movingElement.Index = collidingElement.Index + Values.ElementRoot;
                    }
                }
                else
                {
                    movingElement.Index = movingElement.Y;
                }
            }
        }

        /// <summary>
        /// Moves elements or merges them with elements to the bottom
        /// </summary>
        private void PerformMoveDown()
        {
            foreach (var movingElement in Elements.OrderBy(e => e.Y).ThenByDescending(e => e.X))
            {
                if (movingElement.X == Values.ElementRoot - 1) continue;
                var collidingElement = Elements.OrderBy(e => e.X).FirstOrDefault(e => e.Y == movingElement.Y && e.X > movingElement.X);
                if (collidingElement != null)
                {
                    if (!collidingElement.Blocked && collidingElement.Value == movingElement.Value)
                    {
                        movingElement.Index = collidingElement.Index;
                        movingElement.Value *= 2;
                        movingElement.Blocked = true;
                        collidingElement.Obsolete = true;
                    }
                    else if (collidingElement.X > 1)
                    {
                        movingElement.Index = collidingElement.Index - Values.ElementRoot;
                    }
                }
                else
                {
                    movingElement.Index = Values.ElementRoot * Values.ElementRoot - Values.ElementRoot + movingElement.Y;
                }
            }
        }

        /// <summary>
        /// Moves elements or merges them with elements to the right
        /// </summary>
        private void PerformMoveRight()
        {
            foreach (var movingElement in Elements.OrderBy(e => e.X).ThenByDescending(e => e.Y))
            {
                if (movingElement.Y == Values.ElementRoot - 1) continue;
                var collidingElement = Elements.OrderBy(e => e.Y).FirstOrDefault(e => e.X == movingElement.X && e.Y > movingElement.Y);
                if (collidingElement != null)
                {
                    if (!collidingElement.Blocked && collidingElement.Value == movingElement.Value)
                    {
                        movingElement.Index = collidingElement.Index;
                        movingElement.Value *= 2;
                        movingElement.Blocked = true;
                        collidingElement.Obsolete = true;
                    }
                    else if (collidingElement.Y > 1)
                    {
                        movingElement.Index = collidingElement.Index -1;
                    }
                }
                else
                {
                    movingElement.Index = movingElement.X * Values.ElementRoot + Values.ElementRoot - 1;
                }
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
                    if (!collidingElement.Blocked && collidingElement.Value == movingElement.Value)
                    {
                        movingElement.Index = collidingElement.Index;
                        movingElement.Value *= 2;
                        movingElement.Blocked = true;
                        collidingElement.Obsolete = true;
                    }
                    else if (collidingElement.Y + 1 < Values.ElementRoot)
                    {
                        movingElement.Index = collidingElement.Index + 1;
                    }
                }
                else
                {
                    movingElement.Index = movingElement.X * Values.ElementRoot;
                }
            }
        }

        /// <summary>
        /// Remove obsolete values which have been merged into another element
        /// </summary>
        private void CleanField()
        {
            var obsoleteElements = Elements.Where(e => e.Obsolete).ToArray();
            foreach (var obsoleteElement in obsoleteElements)
                Elements.Remove(obsoleteElement);
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
            if (!Elements.Any(e => e.Value >= Math.Pow(Values.ElementRoot, Values.WinningPower))) return;

            WinPossible = true;
            var result = BlurBehindMessageBox.Show(Properties.Resources.WinBody, Properties.Resources.WinHeader,
                BlurryDialogButton.YesNoCancel);
            if (result == BlurryDialogResult.Yes) Singleton.FieldViewModel.ResetGame();
        }

        /// <summary>
        /// Check if any possible moves can be made
        /// </summary>
        private void CheckDefeatCondition()
        {
            if (Elements.Count < Values.ElementCount) return;

            if (Elements.Count < Values.ElementCount ||
                Elements.Select(element => Elements.Where(e =>
                        e.Value == element.Value &&
                        (Math.Abs(e.X - element.X) == 1 && e.Y == element.Y ||
                         Math.Abs(e.Y - element.Y) == 1 && e.X == element.X)).ToArray())
                    .Any(possibleCollisions => possibleCollisions.Any())) return;

            WinPossible = false;
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
            return Elements.Count != previousState.Count ||
                   !previousState.All(previousElement => Elements.Any(previousElement.Equals));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clear all values and reset to dafault conditions
        /// </summary>
        public void ResetGame()
        {
            Elements.Clear();
            Moves = 0;
            Score = Values.AddCount * Values.StartValue;
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
                case Direction.Up:
                    PerformMoveUp();
                    break;
                case Direction.Down:
                    PerformMoveDown();
                    break;
                case Direction.Right:
                    PerformMoveRight();
                    break;
                case Direction.Left:
                    PerformMoveLeft();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
            
            if (CheckChanges(previousState))
            {
                CleanField();
                CleanBlock();

                Moves++;
                AddBareValues(1);
                Score = Elements.Sum(e => e.Value);

                CheckWinCondition();
                CheckDefeatCondition();
            }

            IsBusy = false;
        }

        #endregion
    }
}