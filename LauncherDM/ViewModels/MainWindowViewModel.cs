﻿using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Services;
using LauncherDM.Services.Interfaces;
using LauncherDM.ViewModels.UserControlsVM;
using System;
using System.Windows;
using System.Windows.Input;

namespace LauncherDM.ViewModel
{
    internal class MainWindowViewModel : Base.ViewModel
    {

        #region Services

        private readonly IResourcesHelperService _resourcesHelper;

        private readonly IDialogWindowService _dialogWindow;

        #endregion

        #region Fields

        private Action _closeAction;

        private Action _dragMoveAction;

        #endregion

        #region Binding

        public string StoreText => _resourcesHelper.LocalizationGet("Store");

        public string ForumText => _resourcesHelper.LocalizationGet("Forum");

        public string AccountText => _resourcesHelper.LocalizationGet("Account");

        #region Title

        private string _title = "DarkMilk";

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        #endregion

        #region ToolBar

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
                _dialogWindow.DragMoveAction = _dragMoveAction;
                _dialogWindow.DragMoveWindow();
            }
        }

        #endregion

        #endregion


        public MainWindowViewModel(Action dragMove, ToolbarToWindowViewModel toolbarViewModel, ResourcesHelperService resourcesHelper)
        {
            _dragMoveAction = dragMove;
            ToolbarVM = toolbarViewModel;
            _resourcesHelper = resourcesHelper;
            _dialogWindow = new DialogWindowService();
            MoveWindowCommand = new LambdaCommand(OnMoveWindowCommandExecuted, CanMoveWindowCommandExecute);
        }
    }
}
