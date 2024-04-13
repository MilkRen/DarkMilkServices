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

        #region ShowRegAndLogWindowCommand

        public Command ShowRegAndLogWindowCommand { get; }

        private bool CanShowRegistrationFormCommandExecute(object p) => true;

        private void OnShowRegistrationFormCommandExecuted(object p)
        {
            var windowService = new DialogWindowService();
            windowService.OpenWindow(this);
            _authorizationWindow.Hide();
        }

        #endregion

        #region MoveWindowCommand

        public Command MoveWindowCommand { get; }
        private bool CanMoveWindowCommandExecute(object p) => true;
        private void OnMoveWindowCommandExecuted(object p)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                _authorizationWindow.DragMove();
        }

        #endregion

        #endregion

        #region Ctor

        public AuthorizationWindowViewModel(Action closeWindow)
        {
            ToolbarVM = new ToolbarToWindowViewModel(closeWindow, Visibility.Visible);
            ShowRegAndLogWindowCommand = new lambdaCommand(OnShowRegistrationFormCommandExecuted, CanShowRegistrationFormCommandExecute);
            MoveWindowCommand = new lambdaCommand(OnMoveWindowCommandExecuted, CanMoveWindowCommandExecute);
        }

        #endregion
    }
}
