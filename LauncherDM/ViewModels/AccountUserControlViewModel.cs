using System;
using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Infrastructure.ReactiveUI;
using LauncherDM.Models;
using LauncherDM.Services;
using LauncherDM.Services.Interfaces;

namespace LauncherDM.ViewModels
{
    public class AccountUserControlViewModel : ViewModel.Base.ViewModel, Infrastructure.ReactiveUI.Base.IObserver<LanguagesUpdate>
    {
        #region Fields

        private Action _closeAction;

        private string _titleResource;

        #endregion

        #region Services

        private readonly IDialogWindowService _windowService;

        private readonly IResourcesHelperService _resourcesHelper;

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

        public AccountUserControlViewModel(Action closeMainWindow, string NameResource, ResourcesHelperService resourcesHelperService)
        {
            _resourcesHelper = resourcesHelperService;
            _titleResource = NameResource;
            SetAccountName(_titleResource, _resourcesHelper);
            _closeAction = closeMainWindow;
            _windowService = new DialogWindowService();
            ShowRegAndLogOrMainFormCommand = new LambdaCommand(OnShowRegAndLogOrMainFormCommandExecuted, CanShowRegAndLogOrMainFormCommandExecute);
        }

        private void SetAccountName(string titleResource, IResourcesHelperService resourcesHelper)
        {
            AccountName = resourcesHelper.LocalizationGet(titleResource);
        }

        public void Update(LanguagesUpdate data)
        {
            if (data.UpdateUI)
            {
                AllPropertyChanged();
                SetAccountName(_titleResource, _resourcesHelper);
            }
        }
    }
}
