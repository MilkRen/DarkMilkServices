using LauncherDM.Services.Interfaces;
using LauncherDM.Services;
using LauncherDM.ViewModels.UserControlsVM;
using System.Reflection;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Infastructure.Commands;
using LauncherDM.Models;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Net.Mail;
using System.Text.RegularExpressions;
using LauncherDM.Views.Windows;

namespace LauncherDM.ViewModels
{
    class RegAndLogWindowViewModel : ViewModel.Base.ViewModel
    {
        #region Services

        private readonly IResourcesHelperService _resourcesHelper;

        private readonly IDialogWindowService _dialogWindow;

        #endregion

        #region Fields

        private Action _closeAction;

        private Action _dragMoveAction;

        #endregion

        #region Bindings

        public string Title => _resourcesHelper.LocalizationGet("Authorization");

        public string LoginText => _resourcesHelper.LocalizationGet("Login");

        public string PasswordText => _resourcesHelper.LocalizationGet("Password");

        public string SignUpText => _resourcesHelper.LocalizationGet("SignUp");

        public string ConfirmPasswordText => _resourcesHelper.LocalizationGet("ConfirmPassword");

        public string RememberMeText =>_resourcesHelper.LocalizationGet("RememberMe");

        public string EmailText => _resourcesHelper.LocalizationGet("Email");

        public string ForgotPasswordText => _resourcesHelper.LocalizationGet("ForgotPassword");

        public string VersionText => string.Concat(_resourcesHelper.LocalizationGet("Version"), "", StaticFields.VersionPatch);

        #region ToolBar

        private ToolbarToWindowViewModel _toolbarVM;

        public ToolbarToWindowViewModel ToolbarVM
        {
            get => _toolbarVM;
            set => Set(ref _toolbarVM, value);
        }

        #endregion

        #region Login

        private string _login;

        public string Login
        {
            get => _login;
            set => Set(ref _login, value);
        }

        private string _password;

        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        #endregion

        #region SugnUp

        private string _regLogin;

        public string RegLogin
        {
            get => _regLogin;
            set => Set(ref _regLogin, value);
        }

        private string _email;

        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }

        private string _regPassword;

        public string RegPassword
        {
            get => _regPassword;
            set => Set(ref _regPassword, value);
        }

        #endregion

        #endregion

        #region Commmands

        #region MoveWindowCommand

        public Command MoveWindowCommand { get; }
        private bool CanMoveWindowCommandExecute(object p) => true;
        private void OnMoveWindowCommandExecuted(object p)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                _dialogWindow.DragMoveAction = _dragMoveAction;
                _dialogWindow.DragMoveWindow();
            }
        }

        #endregion

        #region AccountRecoveryWindow

        public Command ShowAccountRecoveryWindowCommand { get; }
        private bool CanShowAccountRecoveryWindowCommandExecute(object p) => true;
        private void OnShowAccountRecoveryWindowCommandExecuted(object p)
        {
            _dialogWindow.OpenAccountRecovery();
        }

        #endregion

        #region Login

        public Command LoginCommand { get; }
        private bool CanLoginCommandExecute(object p) => true;
        private void OnLoginCommandExecuted(object p)
        {
            IAuthorizationService authorization = new AuthorizationService();
            IXmlService xmlService = new XmlService();
            if (authorization.Authorization(Login, Password))
            {
                var userlist = xmlService.DeserializeUsersXMl();
                var user = new Users(Login, string.Empty);
                user.PasswordArray = user.CryptPassword(Password);

                if (!userlist.UserList.Exists(x => x.Login == user.Login))
                    userlist.UserList.Add(user);
                else
                {
                    IDialogMessageBoxService dialogMessageBox = new DialogMessageBoxService();
                    dialogMessageBox.DialogShow(_resourcesHelper.LocalizationGet("Error"), _resourcesHelper.LocalizationGet("ErrorLoginloggedIn"),
                        messageBoxImage: CustomMessageBoxImage.Error);
                    _dialogWindow.CloseAction = _closeAction;
                    _dialogWindow.CloseWindow();
                    return;
                }

                if(userlist.UserList.Count <= 5)
                    xmlService.SerializationUsersXml(userlist);
                else
                {
                    IDialogMessageBoxService dialogMessageBox = new DialogMessageBoxService();
                    dialogMessageBox.DialogShow(_resourcesHelper.LocalizationGet("Attention"), _resourcesHelper.LocalizationGet("AccountListIsFull"));
                    return;
                }

                // Todo: исправить 
                RegAndLogWindow.CloseShow = true; // костыль

                _dialogWindow.OpenWindow(this);
                _dialogWindow.CloseAction = _closeAction;
                _dialogWindow.CloseWindow();
            }
            else
            {
                IDialogMessageBoxService dialogMessageBox = new DialogMessageBoxService();
                dialogMessageBox.DialogShow(_resourcesHelper.LocalizationGet("Error"), _resourcesHelper.LocalizationGet("ErrorLogin"),
                    messageBoxImage:CustomMessageBoxImage.Error);
            }
        }

        #endregion

        #region SignUp

        public Command SignUpCommand { get; }
        private bool CanSignUpCommandExecute(object p) => true;
        private void OnSignUpCommandExecuted(object p)
        {
            ISignUpService signUpService = new SignUpService();

            if (Regex.IsMatch(RegLogin, "^(?=.*[A-Za-z0-9]$)[A-Za-z][A-Za-z\\d.-]{0,19}$"))
            {
            }

            if (IsValid(Email))
            {

            }

            // Todo: исправить 
            RegAndLogWindow.CloseShow = true; // костыль

            //if (signUpService.SignUp(RegLogin, Email, RegPassword))
            //{

            //}
            //else
            //{
            //    IDialogMessageBoxService dialogMessageBox = new DialogMessageBoxService();
            //    dialogMessageBox.DialogShow("Error Server Reques", "Error Server Reques");
            //}

        }

        public bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }


        #endregion

        #endregion

        public RegAndLogWindowViewModel(Action dragMove, Action closeWindow ,ToolbarToWindowViewModel toolbarViewModel, ResourcesHelperService resourcesHelper)
        {
            _resourcesHelper = resourcesHelper;
            _closeAction = closeWindow;
            ToolbarVM = toolbarViewModel;
            _dragMoveAction = dragMove;
            _dialogWindow = new DialogWindowService();
            ShowAccountRecoveryWindowCommand = new LambdaCommand(OnShowAccountRecoveryWindowCommandExecuted, CanShowAccountRecoveryWindowCommandExecute);
            LoginCommand = new LambdaCommand(OnLoginCommandExecuted, CanLoginCommandExecute);
            SignUpCommand = new LambdaCommand(OnSignUpCommandExecuted, CanSignUpCommandExecute);
            MoveWindowCommand = new LambdaCommand(OnMoveWindowCommandExecuted, CanMoveWindowCommandExecute);
        }
    }
}
