using WPF2048.ViewModel;

namespace WPF2048.Module
{
    public class Singleton
    {
        #region Fields

        private static FieldViewModel _fieldViewModel;

        #endregion

        private Singleton() { }

        #region Properties

        public static int CurrentRoot { get; set; } = 4;
        public static int CurrentHighScore { get; set; } = 0;

        #endregion

        #region ViewModels

        public static FieldViewModel FieldViewModel =>
            _fieldViewModel ?? (_fieldViewModel = new FieldViewModel());

        #endregion
    }
}