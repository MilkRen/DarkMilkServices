using System;
using System.Linq;
using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Infrastructure;
using LauncherDM.Infrastructure.ReactiveUI;
using LauncherDM.Models;
using LauncherDM.Services;
using LauncherDM.Services.Interfaces;
using LauncherDM.Views.Windows;
using Microsoft.VisualBasic.ApplicationServices;

namespace LauncherDM.ViewModels
{
    public class AccountUserControlViewModel : ViewModel.Base.ViewModel, Infrastructure.ReactiveUI.Base.IObserver<LoadUI>
    {
        #region Fields

        private Action _closeAction;

        private string _titleResource;

        private Users _user;

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
            _windowService.CloseAction = _closeAction;
            if (Password is not null)
            {
                IAuthorizationService authorization = new AuthorizationService();
                IXmlService xmlService = new XmlService();
                var user = new Users();
                user.PasswordArray = Password;
                
                if (authorization.Authorization(AccountName, user.DecryptPassword()))
                    _windowService.OpenMainWindow();

                _windowService.CloseWindow();
            }
            else
            { 
                _windowService.OpenWindow(this);
                // Todo: исправить 
                if (RegAndLogWindow.CloseShow) // костыль 
                    _windowService.CloseWindow();
            }
        }


        public Command DeleteAccountCommand { get; }

        private bool CanDeleteAccountCommandExecute(object p) => true;

        private void OnDeleteAccountCommandExecuted(object p)
        {
            // Todo: поправить. неправильное удаление 
            IXmlService xmlService = new XmlService();
            var userlist = xmlService.DeserializeUsersXMl();
            var selectUser = userlist.UserList.First(x => x.Login == _user.Login);
            userlist.UserList.Remove(selectUser);
            xmlService.DeleteFileUsers();
            xmlService.SerializationUsersXml(userlist);
            UpdateUI.PullUi.Notify(new LoadUI(true));
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

        #region ContextMenuText

        public string MenuItemText => _resourcesHelper.LocalizationGet("DeleteAccount");

        #endregion

        #endregion

        public AccountUserControlViewModel(Action closeMainWindow, Users user, ResourcesHelperService resourcesHelperService)
        {
            _user = user;
            AccountName = user.Login;
            DisplayedImagePath = user.ImagePath;
            Password = user.PasswordArray;
            _closeAction = closeMainWindow;
            ButtonText = $"[{AccountName[0]}]";
            _resourcesHelper = resourcesHelperService;
            _windowService = new DialogWindowService();
            ShowRegAndLogOrMainFormCommand = new LambdaCommand(OnShowRegAndLogOrMainFormCommandExecuted, CanShowRegAndLogOrMainFormCommandExecute);
            DeleteAccountCommand = new LambdaCommand(OnDeleteAccountCommandExecuted, CanDeleteAccountCommandExecute);
        }

        public AccountUserControlViewModel(Action closeMainWindow, string NameResource, ResourcesHelperService resourcesHelperService)
        {
            _resourcesHelper = resourcesHelperService;
            _titleResource = NameResource;
            SetAccountName(_titleResource, _resourcesHelper);
            _closeAction = closeMainWindow;
            _windowService = new DialogWindowService();
            ShowRegAndLogOrMainFormCommand = new LambdaCommand(OnShowRegAndLogOrMainFormCommandExecuted, CanShowRegAndLogOrMainFormCommandExecute);
            DeleteAccountCommand = new LambdaCommand(OnDeleteAccountCommandExecuted, CanDeleteAccountCommandExecute);
        }

        private void SetAccountName(string titleResource, IResourcesHelperService resourcesHelper)
        {
            AccountName = resourcesHelper.LocalizationGet(titleResource);
        }

        public void Update(LoadUI data)
        {
            if (data.UpdateUI)
            {
                AllPropertyChanged();
                SetAccountName(_titleResource, _resourcesHelper);
            }
        }
    }
}
