using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Services.Interfaces;
using LauncherDM.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using LauncherDM.ViewModels.UserControlsVM;
using System.Windows.Controls;

namespace LauncherDM.ViewModels
{
    class AuthorizationWindowViewModel : ViewModel.Base.ViewModel  
    {
        #region Fields

        private Action _closeAction;

        private Action _dragMoveAction;

        #endregion

        #region Services

        private readonly IResourcesHelperService _resourcesHelper;

        private readonly IDialogWindowService _windowService;

        #endregion

        #region Bindings

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

        public IEnumerable<AccountUserControlViewModel> AccountControls => _accountList;

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

        public AuthorizationWindowViewModel(Action dragMove, Action closeWindowAction ,ToolbarToWindowViewModel toolbarVM)
        {
            _dragMoveAction = dragMove;
            _closeAction = closeWindowAction;
            ToolbarVM = toolbarVM;

            _accountList = new ObservableCollection<AccountUserControlViewModel>()
            {
                new AccountUserControlViewModel(closeWindowAction,"Sex", "/Source/Images/Logo/MilkBottle.png"),
                new AccountUserControlViewModel(closeWindowAction,"Sex", ""),
                new AccountUserControlViewModel(closeWindowAction,"Sex", ""),
                new AccountUserControlViewModel(closeWindowAction,"Sex", ""),
                new AccountUserControlViewModel(closeWindowAction,"Sex", ""),
            };

            _windowService = new DialogWindowService();
            ShowRegAndLogFormCommand = new lambdaCommand(OnShowRegAndLogFormCommandExecuted, CanShowRegAndLogFormCommandExecute);
            MoveWindowCommand = new lambdaCommand(OnMoveWindowCommandExecuted, CanMoveWindowCommandExecute);
        }

        #endregion
    }
}
