using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Models;
using LauncherDM.Services.Interfaces;
using LauncherDM.Services;
using LauncherDM.Views.Windows;
using System;
using System.Net.Mime;
using System.Windows;
using System.Windows.Input;

namespace LauncherDM.ViewModels
{
    class AuthorizationWindowViewModel : ViewModel.Base.ViewModel  
    {
        #region Fields

        private Action _closeWidnowAction;

        private readonly Window _authorizationWindow = Application.Current.MainWindow;

        #endregion

        #region Commmands

        #region CloseWindowActionCommand

        public Command CloseWindowActionCommand { get; }
        private bool CanCloseWindowCommandExecute(object p) => true;
        private void OnCloseWindowCommandExecuted(object p) => _closeWidnowAction();

        #endregion

        #region CloseApplicationCommand

        private bool CanCloseApplicationCommandExecute(object p) => true;

        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }


        #endregion

        #region ShowLoginFormCommand

        public Command ShowLoginFormCommand { get; }
        private bool CanShowLoginFormCommandExecute(object p) => true;
        private void OnShowLoginFormCommandExecuted(object p)
        {
            IDialogWindowService windowService = new DialogWindowService();
            windowService.OpenWindow(this);
        }

        #endregion

        #region ShowRegistrationFormComman

        public Command ShowRegistrationFormCommand { get; }
        private bool CanShowRegistrationFormCommandExecute(object p) => true;

        private void OnShowRegistrationFormCommandExecuted(object p)
        {
            IDialogWindowService windowService = new DialogWindowService();
            windowService.OpenWindow(this);
            _authorizationWindow.Hide();
        }

        #endregion

        #endregion

        #region Ctor

        public AuthorizationWindowViewModel()
        {
            CloseWindowActionCommand = new lambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            ShowRegistrationFormCommand = new lambdaCommand(OnShowRegistrationFormCommandExecuted, CanShowRegistrationFormCommandExecute);
        }

        public AuthorizationWindowViewModel(Action closeWindow)
        {
            _closeWidnowAction = closeWindow;
            CloseWindowActionCommand = new lambdaCommand(OnCloseWindowCommandExecuted, CanCloseWindowCommandExecute);
        }

        #endregion
    }
}
