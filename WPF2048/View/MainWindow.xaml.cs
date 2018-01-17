using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BlurryControls.DialogFactory;
using BlurryControls.Internals;
using WPF2048.Assets;
using WPF2048.Module;
using WPF2048.ViewModel;

namespace WPF2048.View
{
    /// <summary>
    ///     Interaction logic MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            EventManager.RegisterClassHandler(typeof(Window),
                Keyboard.KeyUpEvent, new KeyEventHandler(KeyEventHandler), true);
        }

        private static void KeyEventHandler(object sender, KeyEventArgs keyEventArgs)
        {
            switch (keyEventArgs.Key)
            {
                case Key.Left:
                case Key.A:
                    Singleton.FieldViewModel.KeyAction(Direction.Left);
                    break;
                case Key.Up:
                case Key.W:
                    Singleton.FieldViewModel.KeyAction(Direction.Up);
                    break;
                case Key.Right:
                case Key.D:
                    Singleton.FieldViewModel.KeyAction(Direction.Right);
                    break;
                case Key.Down:
                case Key.S:
                    Singleton.FieldViewModel.KeyAction(Direction.Down);
                    break;
            }
        }

        private void ResetButton_OnClick(object sender, RoutedEventArgs e)
        {
            var customButtonCollection = new ButtonCollection();
            foreach (var sizeOption in FieldViewModel.SizeOptions)
            {
                var optionButton = new Button
                {
                    Content = sizeOption,
                    Width = 50d,
                    ToolTip = string.Format(Properties.Resources.WinTileToolTip,
                        Math.Pow(FieldViewModel.StartValue,
                            FieldViewModel.WinningPowers[sizeOption - FieldViewModel.SizeOptions[0]]))
                };
                optionButton.Click += OptionButtonOnClick;
                customButtonCollection.Add(optionButton);
            }
            BlurBehindMessageBox.Show(this, Properties.Resources.ResetBody, Properties.Resources.ResetHeader,
                customButtonCollection);
        }

        private void OptionButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (!(sender is Button optionButton)) return;
            Singleton.FieldViewModel.ElementRoot = int.Parse(optionButton.Content.ToString());
            Singleton.FieldViewModel.ResetGame();
            FieldControl.CreateField();
        }
    }
}