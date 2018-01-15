using WPF2048.ViewModel;

namespace WPF2048.Module
{
    public class Singleton
    {
        #region Fields

        private static FieldViewModel _fieldViewModel;

        #endregion

        private Singleton() { }

        #region GlobalRoot

        public static int CurrentRoot { get; set; }

        #endregion

        #region ViewModels

        public static FieldViewModel FieldViewModel =>
            _fieldViewModel ?? (_fieldViewModel = new FieldViewModel());

        #endregion
    }
}