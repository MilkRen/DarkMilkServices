﻿using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Services.Interfaces;
using LauncherDM.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using LauncherDM.ViewModels.UserControlsVM;
using System.Windows.Controls;
using LauncherDM.Infrastructure;
using LauncherDM.Models;
using LauncherDM.Infrastructure.ReactiveUI;

namespace LauncherDM.ViewModels
{
    class AuthorizationWindowViewModel : ViewModel.Base.ViewModel, Infrastructure.ReactiveUI.Base.IObserver<LanguagesUpdate>
    {
        #region Fields

        private Action _closeAction;

        private Action _dragMoveAction;

        #endregion

        #region Services

        private IResourcesHelperService _resourcesHelper;

        private readonly IDialogWindowService _windowService;

        private readonly IXmlService _xmlService;

        #endregion

        #region Bindings

        public string RegText => _resourcesHelper.LocalizationGet("SignUp");

        #region Toolbar

        private ToolbarToWindowViewModel _toolbarVM;

        public ToolbarToWindowViewModel ToolbarVM
        {
            get => _toolbarVM;
            set => Set(ref _toolbarVM, value);
        }

        #endregion

        #region AccountUserControl

        private ObservableCollection<AccountUserControlViewModel> _accountList;

        public ObservableCollection<AccountUserControlViewModel> AccountControls
        {
            get => _accountList;
            set => Set(ref _accountList, value);
        }

        #endregion

        #endregion

        #region Commmands

        #region ShowLoginForm

        public Command ShowRegAndLogFormCommand { get; }

        private bool CanShowRegAndLogFormCommandExecute(object p) => true;

        private void OnShowRegAndLogFormCommandExecuted(object p)
        {
            _windowService.OpenWindow(this);
            _windowService.CloseAction = _closeAction;
            _windowService.CloseWindow();
        }

        #endregion

        #region MoveWindowCommand

        public Command MoveWindowCommand { get; }
        private bool CanMoveWindowCommandExecute(object p) => true;
        private void OnMoveWindowCommandExecuted(object p)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                _windowService.DragMoveAction = _dragMoveAction;
                _windowService.DragMoveWindow();
            }
        }

        #endregion

        #endregion

        #region Ctor

        public AuthorizationWindowViewModel(Action dragMove, Action closeWindowAction, ToolbarToWindowViewModel toolbarVM, ResourcesHelperService resourcesHelperService)
        {
            _dragMoveAction = dragMove;
            _closeAction = closeWindowAction;
            ToolbarVM = toolbarVM;
            _resourcesHelper = resourcesHelperService;

            AccountControls = new ObservableCollection<AccountUserControlViewModel>();
            _xmlService = new XmlService();
            var xmlUsersList = _xmlService.DeserializeUsersXMl();

            if (xmlUsersList.UserList.Count == 0)
            {
                var accountUserVm = new AccountUserControlViewModel(closeWindowAction, "Account", resourcesHelperService);
                UpdateUI.LanguagesPull.Subscribe(accountUserVm);
                AccountControls.Add(accountUserVm);
            }
            else
            {
                foreach (var user in xmlUsersList.UserList)
                    AccountControls.Add(new AccountUserControlViewModel(closeWindowAction, user));

                if (xmlUsersList.UserList.Count <= 5)
                {
                    var accountUserVm = new AccountUserControlViewModel(closeWindowAction, "Account", resourcesHelperService);
                    UpdateUI.LanguagesPull.Subscribe(accountUserVm);
                    AccountControls.Add(accountUserVm);
                }
            }
            
            _windowService = new DialogWindowService();
            ShowRegAndLogFormCommand = new LambdaCommand(OnShowRegAndLogFormCommandExecuted, CanShowRegAndLogFormCommandExecute);
            MoveWindowCommand = new LambdaCommand(OnMoveWindowCommandExecuted, CanMoveWindowCommandExecute);
        }

        public void Update(LanguagesUpdate languagesUpdate)
        {
            if (languagesUpdate.UpdateUI)
                AllPropertyChanged();
        }

        #endregion
    }
}
