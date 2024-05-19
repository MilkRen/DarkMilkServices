using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LauncherDM.Infastructure.Commands;
using System.Windows;
using LauncherDM.Services.Interfaces;

namespace LauncherDM.ViewModels.UserControlsVM
{
    public class ToolbarToWindowViewModel : ViewModel.Base.ViewModel
    {
        #region Fields

        private Action _closeWidnowAction;

        #endregion

        #region Services

        private readonly IWindowService _window;

        private readonly IDialogWindowService _windowService;

        #endregion

        #region Binding

        private string _title;

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        private Visibility _visibilityMinBut;

        public Visibility VisibilityMinBut
        {
            get => _visibilityMinBut;
            set => Set(ref _visibilityMinBut, value);
        }

        private Visibility _visibilitySettingsBut;

        public Visibility VisibilitySettingsBut
        {
            get => _visibilitySettingsBut;
            set => Set(ref _visibilitySettingsBut, value);
        }

        public int _widthMax;

        public int WidthMax
        {
            get => _widthMax;
            set => Set(ref _widthMax, value);
        }

        #endregion

        #region Commands

        #region CloseWindow

        public Command CloseWindowActionCommand { get; }

        private bool CanCloseWindowCommandExecute(object p) => true;

        private void OnCloseWindowCommandExecuted(object p)
        {
            _windowService.CloseAction = _closeWidnowAction;
            _windowService.CloseWindow();
            //this.Dispose();
        }

        #endregion

        #region CloseApp

        private bool CanCloseAppCommandExecute(object p) => true;

        private void OnCloseAppCommandExecuted(object p)
        {
            IApplicationService application = new ApplicationService();
            application.CloseApplication();
        }

        #endregion

        #region Maximize

        public Command MaximizeWindowCommand { get; }

        private bool CanMaximizeWindowCommandExecute(object p) => true;

        private void OnMaximizeWindowCommandExecuted(object p)
        {
            _window.Window.WindowState = _window.Window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        #endregion

        #region Minimize

        public Command MinimizeWindowCommand { get; }

        private bool CanMinimizeWindowCommandExecute(object p) => true;

        private void OnMinimizeWindowCommandExecuted(object p)
        {
            _window.Window.WindowState = WindowState.Minimized;
        }

        #endregion

        #region ShowSettings

        public Command ShowSettingsMiniCommand { get; }

        private bool CanShowSettingsMiniCommandExecute(object p) => true;

        private void OnShowSettingsMiniCommandExecuted(object p)
        {
            _windowService.OpenWindow(this);
        }

        #endregion

        #endregion

        public ToolbarToWindowViewModel(WindowService window, Action closeWidnow = null, string title = null, int widthMax = 0, Visibility visibilitySettings = Visibility.Hidden, Visibility visibilityMinBut = Visibility.Visible)
        {
            if (closeWidnow is not null)
            {
                _closeWidnowAction = closeWidnow;
                CloseWindowActionCommand = new LambdaCommand(OnCloseWindowCommandExecuted, CanCloseWindowCommandExecute);
            }
            else 
                CloseWindowActionCommand = new LambdaCommand(OnCloseAppCommandExecuted, CanCloseAppCommandExecute);

            _window = window;
            VisibilitySettingsBut = visibilitySettings;
            VisibilityMinBut = visibilityMinBut;
            Title = title;
            WidthMax = widthMax;
            _windowService = new DialogWindowService();
            MinimizeWindowCommand = new LambdaCommand(OnMinimizeWindowCommandExecuted, CanMinimizeWindowCommandExecute);
            MaximizeWindowCommand = new LambdaCommand(OnMaximizeWindowCommandExecuted, CanMaximizeWindowCommandExecute);
            ShowSettingsMiniCommand = new LambdaCommand(OnShowSettingsMiniCommandExecuted, CanShowSettingsMiniCommandExecute);
        }
    }
}
