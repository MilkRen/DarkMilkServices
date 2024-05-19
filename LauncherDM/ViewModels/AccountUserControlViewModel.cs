using System;
using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Services;
using LauncherDM.Services.Interfaces;

namespace LauncherDM.ViewModels
{
    public class AccountUserControlViewModel : ViewModel.Base.ViewModel
    {
        #region Fields

        private Action _closeAction;

        #endregion

        #region Services

        private readonly IDialogWindowService _windowService;

        #endregion

        #region Commands

        public Command ShowRegAndLogFormCommand { get; }

        private bool CanShowRegAndLogFormCommandExecute(object p) => true;

        private void OnShowRegAndLogFormCommandExecuted(object p)
        {
            _windowService.OpenWindow(this);
            //_windowService.CloseAction = _closeAction;
            //_windowService.CloseWindow();
        }

        #endregion

        #region Bindings

        #region NameAccount

        private string _accountName;

        public string AccountName
        {
            get => _accountName;
            set => Set(ref _accountName, value);
        }

        #endregion

        #region ImagePath

        private string _displayedImagePath;

        public string DisplayedImagePath
        {
            get => _displayedImagePath;
            set => Set(ref _displayedImagePath, value);
        }

        #endregion

        #endregion

        public AccountUserControlViewModel(Action closeMainWindow, string accountName, string imagePath)
        {
            AccountName = accountName;
            DisplayedImagePath = imagePath;
            _closeAction = closeMainWindow;
            _windowService = new DialogWindowService();
            ShowRegAndLogFormCommand = new LambdaCommand(OnShowRegAndLogFormCommandExecuted, CanShowRegAndLogFormCommandExecute);
        }
    }
}
