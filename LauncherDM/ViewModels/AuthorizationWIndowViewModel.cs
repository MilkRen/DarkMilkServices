using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using System;
using System.Windows.Input;

namespace LauncherDM.ViewModels
{
    class AuthorizationWIndowViewModel : ViewModel.Base.ViewModel  
    {
        #region Commmands

        private Action closeWidnowAction;

        #region MoveWindowCommand

        public Command CloseWindowCommand { get; }
        private bool CanCloseWindowCommandExecute(object p) => true;
        private void OnCloseWindowCommandExecuted(object p) => closeWidnowAction() ?? Environment.Exit(0);

        #endregion

        #endregion

        public AuthorizationWIndowViewModel(Action closeWindow)
        {
            closeWidnowAction = closeWindow;
            CloseWindowCommand = new lambdaCommand(OnCloseWindowCommandExecuted, CanCloseWindowCommandExecute);
        }

        public AuthorizationWIndowViewModel()
        {
            CloseWindowCommand = new lambdaCommand(OnCloseWindowCommandExecuted, CanCloseWindowCommandExecute);
        }
    }
}
