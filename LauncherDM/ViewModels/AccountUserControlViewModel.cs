using System;
using Amazon.Runtime;
using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Models;
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

        public Command ShowRegAndLogOrMainFormCommand { get; }

        private bool CanShowRegAndLogOrMainFormCommandExecute(object p) => true;

        private void OnShowRegAndLogOrMainFormCommandExecuted(object p)
        {
            if (Password is not null)
            {
                IAuthorizationService authorization = new AuthorizationService();
                IXmlService xmlService = new XmlService();
                var user = new Users();
                user.PasswordArray = Password;
                
                if (authorization.Authorization(AccountName, user.DecryptPassword()))
                    _windowService.OpenMainWindow();
            }
            else
                _windowService.OpenWindow(this);

            _windowService.CloseAction = _closeAction;
            _windowService.CloseWindow();
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

        #region Password

        private byte[] _password;

        public byte[] Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        #endregion

        #region ButtonText

        private string _buttonText = "+";

        public string ButtonText
        {
            get => _buttonText;
            set => Set(ref _buttonText, value);
        }

        #endregion

        #endregion

        public AccountUserControlViewModel(Action closeMainWindow, Users user)
        {
            AccountName = user.Login;
            DisplayedImagePath = user.ImagePath;
            Password = user.PasswordArray;
            _closeAction = closeMainWindow;
            ButtonText = $"[{AccountName[0]}]";
            _windowService = new DialogWindowService();
            ShowRegAndLogOrMainFormCommand = new LambdaCommand(OnShowRegAndLogOrMainFormCommandExecuted, CanShowRegAndLogOrMainFormCommandExecute);
        }

        public AccountUserControlViewModel(Action closeMainWindow, string titleName)
        {
            AccountName = titleName;
            _closeAction = closeMainWindow;
            _windowService = new DialogWindowService();
            ShowRegAndLogOrMainFormCommand = new LambdaCommand(OnShowRegAndLogOrMainFormCommandExecuted, CanShowRegAndLogOrMainFormCommandExecute);
        }
    }
}
