using LauncherDM.Services.Interfaces;
using LauncherDM.Services;
using LauncherDM.ViewModels.UserControlsVM;
using System.Reflection;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Infastructure.Commands;

namespace LauncherDM.ViewModels
{
    class RegAndLogWindowViewModel : ViewModel.Base.ViewModel
    {
        #region Services

        private readonly IResourcesHelperService _resourcesHelper;

        private readonly IDialogWindowService _dialogWindow;

        private readonly IRegAndLogWindowService _regAndLogWindowService;

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

        public string VersionText => string.Concat("Version: ", Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty);

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
            if (authorization.Authorization(Login, Password))
            {

            }
            else
            {
                IDialogMessageBoxService dialogMessageBox = new DialogMessageBoxService();
                dialogMessageBox.DialogShow("Error Server Reques", "Error Server Reques");
            }
        }

        #endregion

        #region SignUp

        public Command SignUpCommand { get; }
        private bool CanSignUpCommandExecute(object p) => true;
        private void OnSignUpCommandExecuted(object p)
        {

        }

        #endregion

        #endregion

        public RegAndLogWindowViewModel(ToolbarToWindowViewModel toolbarViewModel, ResourcesHelperService resourcesHelper)
        {
            _resourcesHelper = resourcesHelper;
            ToolbarVM = toolbarViewModel;
            _dialogWindow = new DialogWindowService();
            _regAndLogWindowService = new RegAndLogWindowServiceService();
            ShowAccountRecoveryWindowCommand = new lambdaCommand(OnShowAccountRecoveryWindowCommandExecuted, CanShowAccountRecoveryWindowCommandExecute);
            LoginCommand = new lambdaCommand(OnLoginCommandExecuted, CanLoginCommandExecute);
            SignUpCommand = new lambdaCommand(OnSignUpCommandExecuted, CanSignUpCommandExecute);
        }
    }
}
