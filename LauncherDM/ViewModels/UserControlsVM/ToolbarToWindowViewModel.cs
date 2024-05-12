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

        #region Properties

        private Visibility _visibilityMaxBut;

        public Visibility VisibilityMaxBut
        {
            get => _visibilityMaxBut;
            set => Set(ref _visibilityMaxBut, value);
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


        #endregion

        #region Commands

        #region CloseWindow

        public Command CloseWindowActionCommand { get; }

        private bool CanCloseWindowCommandExecute(object p) => true;

        private void OnCloseWindowCommandExecuted(object p)
        {
            _windowService.CloseAction = _closeWidnowAction;
            _windowService.CloseWindow();
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
            _window.Window.WindowState = WindowState.Maximized;
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

        #endregion

        public ToolbarToWindowViewModel(WindowService window, Action closeWidnow = null, Visibility visibilitySettings = Visibility.Hidden)
        {
            if (closeWidnow is not null)
            {
                _closeWidnowAction = closeWidnow;
                CloseWindowActionCommand = new lambdaCommand(OnCloseWindowCommandExecuted, CanCloseWindowCommandExecute);
            }
            else 
                CloseWindowActionCommand = new lambdaCommand(OnCloseAppCommandExecuted, CanCloseAppCommandExecute);

            _window = window;
            VisibilitySettingsBut = visibilitySettings;
            _windowService = new DialogWindowService();
            MinimizeWindowCommand = new lambdaCommand(OnMinimizeWindowCommandExecuted, CanMinimizeWindowCommandExecute);
            MaximizeWindowCommand = new lambdaCommand(OnMaximizeWindowCommandExecuted, CanMaximizeWindowCommandExecute);
        }
    }
}
