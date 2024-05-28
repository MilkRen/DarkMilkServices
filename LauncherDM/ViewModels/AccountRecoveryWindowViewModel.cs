using LauncherDM.Services.Interfaces;
using LauncherDM.ViewModels.UserControlsVM;
using System;
using System.Windows.Input;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Services;
using LauncherDM.Infastructure.Commands;
using System.Net.Mail;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace LauncherDM.ViewModels
{
    class AccountRecoveryWindowViewModel : ViewModel.Base.ViewModel
    {
        #region Fields

        private Action _closeAction;

        private Action _dragMoveAction;

        #endregion

        #region Services

        private readonly IResourcesHelperService _resourcesHelper;

        private readonly IDialogMessageBoxService _dialogMessageBox;

        private readonly IDialogWindowService _dialogWindow;

        #endregion

        #region Bindings

        public string Title => _resourcesHelper.LocalizationGet("AccountRecovery");

        public string SupportText => _resourcesHelper.LocalizationGet("SupportRecovery");

        public string LoginText => _resourcesHelper.LocalizationGet("Login");

        public string EmailText => _resourcesHelper.LocalizationGet("Email");

        public string SendText => _resourcesHelper.LocalizationGet("Send");

        #region ToolBar

        private ToolbarToWindowViewModel _toolbarVM;

        public ToolbarToWindowViewModel ToolbarVM
        {
            get => _toolbarVM;
            set => Set(ref _toolbarVM, value);
        }

        #endregion

        #region Email

        private string _email;

        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }

        #endregion

        #region Login

        private string _login;

        public string Login
        {
            get => _login;
            set => Set(ref _login, value);
        }

        #endregion

        #endregion

        #region Command

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

        #region MoveWindowCommand

        public Command SendSupportCommand { get; }
        private bool CanSendSupportCommandExecute(object p) => true;
        private void OnSendSupportCommandExecuted(object p)
        {
            if (!IsValidEmail(Email))
            {
                _dialogMessageBox.DialogShow(_resourcesHelper.LocalizationGet("Attention"),
                    _resourcesHelper.LocalizationGet("EmailError"));
                return;
            }
            
            IAccountRecoveryWindowService accountRecovery = new AccountRecoveryWindowService();
            var data = string.Concat(Login, ",", Email);
            if (accountRecovery.SendMessage(data))
                _dialogMessageBox.DialogShow(_resourcesHelper.LocalizationGet("Attention"),
                    _resourcesHelper.LocalizationGet("SendMessage"));
            else
                _dialogMessageBox.DialogShow(_resourcesHelper.LocalizationGet("Error"),
                    _resourcesHelper.LocalizationGet("SendError"), messageBoxImage: CustomMessageBoxImage.Error);

            _dialogWindow.CloseAction = _closeAction;
            _dialogWindow.CloseWindow();
        }

        // Todo: надо бы в сервис добавить в регистрации 
        public bool IsValidEmail(string email)
        {
            try
            {
                var mail = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        #endregion

        #endregion

        public AccountRecoveryWindowViewModel(Action dragMove, Action closeWindow, ToolbarToWindowViewModel toolbarViewModel, ResourcesHelperService resourcesHelper)
        {
            _resourcesHelper = resourcesHelper;
            _closeAction = closeWindow;
            ToolbarVM = toolbarViewModel;
            _dragMoveAction = dragMove;
            _dialogWindow = new DialogWindowService();
            _dialogMessageBox = new DialogMessageBoxService();
            MoveWindowCommand = new LambdaCommand(OnMoveWindowCommandExecuted, CanMoveWindowCommandExecute);
            SendSupportCommand = new LambdaCommand(OnSendSupportCommandExecuted, CanSendSupportCommandExecute);
        }
    }
}
