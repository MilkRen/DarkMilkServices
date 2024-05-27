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

        private readonly IDialogMessageBoxService _dialogMessageBox;

        private readonly IXmlService _xmlService;

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
            if (string.IsNullOrEmpty(Login) || string.IsNullOrWhiteSpace(Login))
            {
                _dialogMessageBox.DialogShow(_resourcesHelper.LocalizationGet("Attention"),
                    string.Format(_resourcesHelper.LocalizationGet("IsNotEmpty"), _resourcesHelper.LocalizationGet("Authorization"), _resourcesHelper.LocalizationGet("Login")));
                return;
            }

            if (string.IsNullOrEmpty(Password) || string.IsNullOrWhiteSpace(Password))
            {
                _dialogMessageBox.DialogShow(_resourcesHelper.LocalizationGet("Attention"),
                    string.Format(_resourcesHelper.LocalizationGet("IsNotEmpty"), _resourcesHelper.LocalizationGet("Authorization"), _resourcesHelper.LocalizationGet("Password")));
                return;
            }

            if (authorization.Authorization(Login, Password))
            {
                var userlist = _xmlService.DeserializeUsersXMl();
                var user = new Users(Login, string.Empty);
                user.PasswordArray = user.CryptPassword(Password);

                if (!userlist.UserList.Exists(x => x.Login == user.Login))
                    userlist.UserList.Add(user);
                else
                {
                    _dialogMessageBox.DialogShow(_resourcesHelper.LocalizationGet("Error"), _resourcesHelper.LocalizationGet("ErrorLoginloggedIn"),
                        messageBoxImage: CustomMessageBoxImage.Error);
                    _dialogWindow.CloseAction = _closeAction;
                    _dialogWindow.CloseWindow();
                    return;
                }

                if(userlist.UserList.Count <= 5)
                    _xmlService.SerializationUsersXml(userlist);
                else
                {
                    _dialogMessageBox.DialogShow(_resourcesHelper.LocalizationGet("Attention"), _resourcesHelper.LocalizationGet("AccountListIsFull"));
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
                _dialogMessageBox.DialogShow(_resourcesHelper.LocalizationGet("Error"), _resourcesHelper.LocalizationGet("ErrorLogin"),
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
            if (string.IsNullOrEmpty(RegLogin) || string.IsNullOrWhiteSpace(RegLogin))
            {
                _dialogMessageBox.DialogShow(_resourcesHelper.LocalizationGet("Attention"),
                    string.Format(_resourcesHelper.LocalizationGet("IsNotEmpty"), _resourcesHelper.LocalizationGet("SignUp"), _resourcesHelper.LocalizationGet("Login")));
                return;
            }

            if (string.IsNullOrEmpty(RegPassword) || string.IsNullOrWhiteSpace(RegPassword))
            {
                _dialogMessageBox.DialogShow(_resourcesHelper.LocalizationGet("Attention"),
                    string.Format(_resourcesHelper.LocalizationGet("IsNotEmpty"), _resourcesHelper.LocalizationGet("SignUp"), _resourcesHelper.LocalizationGet("Password")));
                return;
            }

            if (string.IsNullOrEmpty(Email) || string.IsNullOrWhiteSpace(Email))
            {
                _dialogMessageBox.DialogShow(_resourcesHelper.LocalizationGet("Attention"),
                    string.Format(_resourcesHelper.LocalizationGet("IsNotEmpty"), _resourcesHelper.LocalizationGet("SignUp"), _resourcesHelper.LocalizationGet("Email")));
                return;
            }

            if (RegLogin.Length < 3)
            {
                _dialogMessageBox.DialogShow(_resourcesHelper.LocalizationGet("Attention"),
                    string.Format(_resourcesHelper.LocalizationGet("LenghtMaxRegLogin"), "3"));
                return;
            }

            var haslatAndNom = new Regex(@"^(?=.*[A-Za-z0-9]$)");
            var banСharacter = new Regex(@"[^!@#$%^&*()_]");
            if (!(haslatAndNom.IsMatch(RegLogin) && banСharacter.IsMatch(RegLogin)))
            {
                _dialogMessageBox.DialogShow(_resourcesHelper.LocalizationGet("Attention"),
                    string.Format(_resourcesHelper.LocalizationGet("LoginError"), "[A-Z], [a-z], [0-9]"));
                return;
            }

            if (!IsValid(Email))
            {
                _dialogMessageBox.DialogShow(_resourcesHelper.LocalizationGet("Attention"),
                    _resourcesHelper.LocalizationGet("EmailError"));
                return;
            }

            if (RegPassword.Length < 8)
            {
                _dialogMessageBox.DialogShow(_resourcesHelper.LocalizationGet("Attention"),
                    string.Format(_resourcesHelper.LocalizationGet("LenghtMaxRegPassword"), "8"));
                return;
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            if (!(hasNumber.IsMatch(RegPassword) && hasUpperChar.IsMatch(RegPassword)))
            {
                _dialogMessageBox.DialogShow(_resourcesHelper.LocalizationGet("Attention"),
                    string.Format(_resourcesHelper.LocalizationGet("PasswordError"), "[0-9][A-Z]"));
                return;
            }

            // Todo: исправить 
            RegAndLogWindow.CloseShow = true; // костыль
            if (signUpService.SignUp(RegLogin, Email, RegPassword))
            {
                // Todo: исправить 
                RegAndLogWindow.CloseShow = true; // костыль

                var userlist = _xmlService.DeserializeUsersXMl();
                var user = new Users(RegLogin, string.Empty);
                user.PasswordArray = user.CryptPassword(RegPassword);
                userlist.UserList.Add(user);

                if (userlist.UserList.Count <= 5)
                    _xmlService.SerializationUsersXml(userlist);
                else
                {
                    _dialogMessageBox.DialogShow(_resourcesHelper.LocalizationGet("Attention"), _resourcesHelper.LocalizationGet("AccountListIsFull"));
                    return;
                }

                _dialogWindow.OpenWindow(this);
                _dialogWindow.CloseAction = _closeAction;
                _dialogWindow.CloseWindow();
            }
            else
            {
                _dialogMessageBox.DialogShow(_resourcesHelper.LocalizationGet("Attention"), _resourcesHelper.LocalizationGet("LoginBusy"));
            }

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
            _dialogMessageBox = new DialogMessageBoxService();
            _xmlService = new XmlService();
            ShowAccountRecoveryWindowCommand = new LambdaCommand(OnShowAccountRecoveryWindowCommandExecuted, CanShowAccountRecoveryWindowCommandExecute);
            LoginCommand = new LambdaCommand(OnLoginCommandExecuted, CanLoginCommandExecute);
            SignUpCommand = new LambdaCommand(OnSignUpCommandExecuted, CanSignUpCommandExecute);
            MoveWindowCommand = new LambdaCommand(OnMoveWindowCommandExecuted, CanMoveWindowCommandExecute);
        }
    }
}
