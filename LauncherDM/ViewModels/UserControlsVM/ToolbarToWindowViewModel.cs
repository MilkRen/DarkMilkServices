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

        public Command CloseWindowActionCommand { get; }

        private bool CanCloseWindowCommandExecute(object p) => true;

        private void OnCloseWindowCommandExecuted(object p)
        {
            var windowService = new DialogWindowService();
            windowService.CloseAction = _closeWidnowAction;
            windowService.CloseWindow();
        }

        #endregion

        public ToolbarToWindowViewModel(Action closeWidnow, Visibility visibilityMinBut = Visibility.Visible)
        {
            VisibilityMinBut = visibilityMinBut;
            _closeWidnowAction = closeWidnow;
            CloseWindowActionCommand = new lambdaCommand(OnCloseWindowCommandExecuted, CanCloseWindowCommandExecute);
        }

    }
}
