using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Services;
using LauncherDM.Services.Interfaces;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LauncherDM.ViewModels
{
    class LoadingWindowViewModel : ViewModel.Base.ViewModel
    {
        #region Fields

        private Action _hideWindow;

        private Action _dragMoveAction;

        #endregion

        #region Services

        private readonly IResourcesHelperService _resourcesHelper;

        private readonly IDialogWindowService _windowService;

        private readonly IWindowService _window;

        #endregion

        #region Bindings

        #region Title

        private string _title = "DarkMilk";

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        #endregion

        #region Description

        private string _descInfoConnect;

        public string DescInfoConnect
        {
            get => _descInfoConnect;
            set => Set(ref _descInfoConnect, value);
        }

        #endregion

        #region MenuItem

        public string CloseApp => _resourcesHelper.LocalizationGet("CloseApp");

        #endregion

        #endregion

        #region Commmands

        #region MoveWindow

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

        #region CloseWindow

        public Command CloseWindowCommand { get; }
        private bool CanCloseWindowCommandExecute(object p) => true;
        private void OnCloseWindowCommandExecuted(object p)
        {
            IApplicationService application = new ApplicationService();
            application.CloseApplication();
        }

        #endregion

        #endregion

        #region Ctor

        public LoadingWindowViewModel(Action dragMoveWindow, Action hideWindow, WindowService window, ResourcesHelperService resourcesHelper)
        {
            _dragMoveAction = dragMoveWindow;
            _resourcesHelper = resourcesHelper;
            _window = window;
            _hideWindow = hideWindow;
            _windowService = new DialogWindowService();
            MoveWindowCommand = new lambdaCommand(OnMoveWindowCommandExecuted, CanMoveWindowCommandExecute);
            CloseWindowCommand = new lambdaCommand(OnCloseWindowCommandExecuted, CanCloseWindowCommandExecute);
            Loading();
        }

        #endregion

        private void Loading()
        {
            ICheckNetworkService checkNetwork = new CheckNetworkService();
            ILoadingWindowService server = new LoadingWindowService(new ServerRequestService());
            OpenWindow();
            return;

            Task.Run(() =>
            {
                DescInfoConnect = _resourcesHelper.LocalizationGet("Сonnection");
                if (!checkNetwork.CheckingNetworkConnection())
                {
                    OpenWindow();
                    return;
                }

                var countMs = 0;
                while (true)
                {
                    if (server.CheckRequestServer())
                    {
                        DescInfoConnect = server.GetTitle();
                        server.CheckUpdate();
                        Thread.Sleep(5000);
                        OpenWindow();
                        break;
                    }
                    else
                    {
                        if (countMs >= 10000)
                        {
                            IDialogMessageBoxService dialogMessageBox = new DialogMessageBoxService();
                            dialogMessageBox.DialogShow(_resourcesHelper.LocalizationGet("Error"), _resourcesHelper.LocalizationGet("ServerClose"));
                            OpenWindow();
                            break;
                        }

                        countMs += 2000;
                        DescInfoConnect = string.Format(CultureInfo.InvariantCulture,
                            _resourcesHelper.LocalizationGet("Reconnection"), countMs);
                        Thread.Sleep(countMs);
                    }
                }
            });
        }

        private void OpenWindow()
        {
            _window.Window.Dispatcher.Invoke(() =>
            {
                _windowService.OpenWindow(this);
                _windowService.HideAction = _hideWindow;
                _windowService.HideWindow();
            });
        }
    }
}
