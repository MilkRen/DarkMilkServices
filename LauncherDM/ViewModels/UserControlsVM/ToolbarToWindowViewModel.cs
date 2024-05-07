using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LauncherDM.Infastructure.Commands;
using System.Windows;

namespace LauncherDM.ViewModels.UserControlsVM
{
    public class ToolbarToWindowViewModel : ViewModel.Base.ViewModel
    {
        #region Fields

        private Action _closeWidnowAction;

        private Action _minWindowAction;

        private Action _maxWindowAction;

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

        #endregion

        #region Commands

        #region CloseWindow

        public Command CloseWindowActionCommand { get; }

        private bool CanCloseWindowCommandExecute(object p) => true;

        private void OnCloseWindowCommandExecuted(object p)
        {
            var windowService = new DialogWindowService();
            windowService.CloseAction = _closeWidnowAction;
            windowService.CloseWindow();
        }

        #endregion

        #region CloseApp

        private bool CanCloseAppCommandExecute(object p) => true;

        private void OnCloseAppCommandExecuted(object p)
        {
            Environment.Exit(0);
        }

        #endregion

        #region Maximize

        public Command MaximizeWindowCommand { get; }

        private bool CanMaximizeWindowCommandExecute(object p) => true;

        private void OnMaximizeWindowCommandExecuted(object p)
        {
            var windowService = new DialogWindowService();
            windowService.MaxWindowAction = _maxWindowAction;
            windowService.MaxWindow();
        }

        #endregion

        #region Minimize

        public Command MinimizeWindowCommand { get; }

        private bool CanMinimizeWindowCommandExecute(object p) => true;

        private void OnMinimizeWindowCommandExecuted(object p)
        {
            var windowService = new DialogWindowService();
            windowService.MinWindowAction = _minWindowAction;
            windowService.MinWindow();
        }

        #endregion

        #endregion

        public ToolbarToWindowViewModel(Action closeWidnow)
        {
            _closeWidnowAction = closeWidnow;
            CloseWindowActionCommand = new lambdaCommand(OnCloseWindowCommandExecuted, CanCloseWindowCommandExecute);
        }

        public ToolbarToWindowViewModel(Action minWindowAction, Action maxWindowAction)
        {
            _minWindowAction = minWindowAction;
            _maxWindowAction = maxWindowAction;
            CloseWindowActionCommand = new lambdaCommand(OnCloseAppCommandExecuted, CanCloseAppCommandExecute);
            MinimizeWindowCommand = new lambdaCommand(OnMinimizeWindowCommandExecuted, CanMinimizeWindowCommandExecute);
            MaximizeWindowCommand = new lambdaCommand(OnMaximizeWindowCommandExecuted, CanMaximizeWindowCommandExecute);
        }
    }
}
