using LauncherDM.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LauncherDM.Services;
using LauncherDM.ViewModels.UserControlsVM;
using System.Windows;
using System.Reflection;
using LauncherDM.Views.Windows;
using System.Windows.Input;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Infastructure.Commands;

namespace LauncherDM.ViewModels
{
    class RegAndLogWindowViewModel : ViewModel.Base.ViewModel
    {
        #region Services

        private readonly IResourcesHelperService _resourcesHelper;

        #endregion

        #region Bindings

        public string Title => _resourcesHelper.LocalizationGet("Authorization");

        public string LoginText => _resourcesHelper.LocalizationGet("Login");

        public string PasswordText => _resourcesHelper.LocalizationGet("Password");

        public string SignUpText => _resourcesHelper.LocalizationGet("SignUp");

        public string ConfirmPasswordText => _resourcesHelper.LocalizationGet("ConfirmPassword");

        public string RememberMeText =>_resourcesHelper.LocalizationGet("RememberMe");

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

        #endregion

        #region Commmands

        #region AccountRecoveryWindow

        public Command ShowAccountRecoveryWindowCommand { get; }
        private bool CanShowAccountRecoveryWindowCommandExecute(object p) => true;
        private void OnShowAccountRecoveryWindowCommandExecuted(object p)
        {
            IDialogWindowService windowService = new DialogWindowService();
            windowService.OpenAccountRecovery();
        }

        #endregion

        #endregion

        public RegAndLogWindowViewModel(ToolbarToWindowViewModel toolbarViewModel, ResourcesHelperService resourcesHelper)
        {
            _resourcesHelper = resourcesHelper;
            ToolbarVM = toolbarViewModel;
            ShowAccountRecoveryWindowCommand = new lambdaCommand(OnShowAccountRecoveryWindowCommandExecuted, CanShowAccountRecoveryWindowCommandExecute);
        }
    }
}
