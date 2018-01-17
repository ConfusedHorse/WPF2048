using System.Windows.Threading;

namespace WPF2048
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App
    {
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
