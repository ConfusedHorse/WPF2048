using System.Windows;
using System.Windows.Input;
using BlurryControls.DialogFactory;
using BlurryControls.Internals;
using WPF2048.Assets;
using WPF2048.Module;

namespace WPF2048.View
{
    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
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
            var result = BlurBehindMessageBox.Show(Properties.Resources.ResetBody, Properties.Resources.ResetHeader,
                BlurryDialogButton.YesNo);
            if (result == BlurryDialogResult.Yes) Singleton.FieldViewModel.ResetGame();
        }
    }
}