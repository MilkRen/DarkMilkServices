﻿using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Models;
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
            //Environment.Exit(0);
            Application.Current.Shutdown();
        }


        #endregion

        #endregion

        #region Ctor

        public AuthorizationWindowViewModel(Action closeWindow)
        {
            _closeWidnowAction = closeWindow;
            CloseWindowActionCommand = new lambdaCommand(OnCloseWindowCommandExecuted, CanCloseWindowCommandExecute);
        }

        public AuthorizationWindowViewModel()
        {
            CloseWindowActionCommand = new lambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
        }

        #endregion
    }
}
