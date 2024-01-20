using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using System.Windows;

namespace LauncherDM.ViewModel
{
    internal class MainWindowViewModel : Base.ViewModel
    {
        #region Заголовок окна
        private string _title = "Главное меню";

        /// <summary>Заголовок окна</summary>
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        #endregion

        #region Команды

        #region CloseAppCommand

        public Command CloseAppCommand { get; }
        private bool CanCloseAppCommandExecute(object p) => true;
        private void OnCloseAppCommandExecuted(object p) => Application.Current.Shutdown();

        #endregion




        #endregion



        public MainWindowViewModel()
        {
            CloseAppCommand = new lambdaCommand(OnCloseAppCommandExecuted, CanCloseAppCommandExecute);
        } 

    }
}
