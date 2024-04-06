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
using LauncherDM.Views.UserControls;
using LauncherDM.ViewModels.UserControlsVM;
using System.Windows.Controls;

namespace LauncherDM.ViewModels
{
    class AuthorizationWindowViewModel : ViewModel.Base.ViewModel  
    {
        #region Fields

        private readonly Window _authorizationWindow = Application.Current.MainWindow;

        #endregion

        #region Properties

        private ToolbarToWindowViewModel _toolbarVM;

        public ToolbarToWindowViewModel ToolbarVM
        {
            get => _toolbarVM;
            set => Set(ref _toolbarVM, value);
        }

        #endregion


        #region Commmands

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
            var windowService = new DialogWindowService();
            windowService.OpenLoginWindow();
            _authorizationWindow.Hide();
        }

        #endregion

        #region ShowRegistrationFormComman

        public Command ShowRegistrationFormCommand { get; }

        private bool CanShowRegistrationFormCommandExecute(object p) => true;

        private void OnShowRegistrationFormCommandExecuted(object p)
        {
            var windowService = new DialogWindowService();
            windowService.OpenRegistrationWindow();
            _authorizationWindow.Hide();
        }

        #endregion

        #endregion

        #region Ctor

        public AuthorizationWindowViewModel(Action closeWindow)
        {
            ToolbarVM = new ToolbarToWindowViewModel(closeWindow);
            ShowLoginFormCommand = new lambdaCommand(OnShowLoginFormCommandExecuted, CanShowLoginFormCommandExecute);
            ShowRegistrationFormCommand = new lambdaCommand(OnShowRegistrationFormCommandExecuted, CanShowRegistrationFormCommandExecute);
        }

        #endregion
    }
}
