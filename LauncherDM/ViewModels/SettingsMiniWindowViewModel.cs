using LauncherDM.ViewModels.UserControlsVM;
using System;
using System.Windows.Input;
using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Services;
using LauncherDM.Services.Interfaces;
using LauncherDM.Models;

namespace LauncherDM.ViewModels
{
    class SettingsMiniWindowViewModel : ViewModel.Base.ViewModel
    {
        #region Fields

        private Action _dragMoveAction;

        #endregion

        #region Services

        private readonly IResourcesHelperService _resourcesHelper;

        private readonly IDialogWindowService _windowService;

        #endregion

        #region Bindings

        public string VersionText => string.Concat(_resourcesHelper.LocalizationGet("Version"), "", StaticFields.VersionPatch);

        public string Title => _resourcesHelper.LocalizationGet("SettingsSmall");

        public string ToolTipLogoText => _resourcesHelper.LocalizationGet("AboutSmallText");


        #region Toolbar

        private ToolbarToWindowViewModel _toolbarVM;

        public ToolbarToWindowViewModel ToolbarVM
        {
            get => _toolbarVM;
            set => Set(ref _toolbarVM, value);
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
                _windowService.DragMoveAction = _dragMoveAction;
                _windowService.DragMoveWindow();
            }
        }

        #endregion

        #endregion

        public SettingsMiniWindowViewModel(Action dragMove, ToolbarToWindowViewModel toolbarVM, ResourcesHelperService resourcesHelper)
        {
            _dragMoveAction = dragMove;
            ToolbarVM = toolbarVM;
            _resourcesHelper = resourcesHelper;
            _windowService = new DialogWindowService();
            MoveWindowCommand = new lambdaCommand(OnMoveWindowCommandExecuted, CanMoveWindowCommandExecute);
        }
    }
}
