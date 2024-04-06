using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LauncherDM.Infastructure.Commands;

namespace LauncherDM.ViewModels.UserControlsVM
{
    public class ToolbarToWindowViewModel : ViewModel.Base.ViewModel
    {
        private Action _closeWidnowAction;


        public Command CloseWindowActionCommand { get; }

        private bool CanCloseWindowCommandExecute(object p) => true;

        private void OnCloseWindowCommandExecuted(object p)
        {
            var windowService = new DialogWindowService();
            windowService.CloseAction = _closeWidnowAction;
            windowService.CloseWindow();
        }

        public ToolbarToWindowViewModel(Action closeWidnow)
        {
            _closeWidnowAction = closeWidnow;
            CloseWindowActionCommand = new lambdaCommand(OnCloseWindowCommandExecuted, CanCloseWindowCommandExecute);
        }

    }
}
