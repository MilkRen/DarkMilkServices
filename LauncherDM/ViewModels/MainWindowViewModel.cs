﻿using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Services;
using LauncherDM.Services.Interfaces;
using LauncherDM.ViewModels.UserControlsVM;
using System;
using System.Windows.Input;
using LauncherDM.ViewModels;
using LauncherDM.Views.UserControls;
using LauncherDM.Infrastructure.ReactiveUI;
using LauncherDM.Infrastructure;

namespace LauncherDM.ViewModel
{
    internal class MainWindowViewModel : Base.ViewModel, Infrastructure.ReactiveUI.Base.IObserver<LoadUI>
    {
        #region Services

        private readonly IResourcesHelperService _resourcesHelper;

        private readonly IDialogWindowService _dialogWindow;

        private readonly IServerRequestService _serverRequest;

        #endregion

        #region Fields

        private Action _dragMoveAction;

        private Action _closeAction;

        #endregion

        #region Binding

        public string StoreText => _resourcesHelper.LocalizationGet("Store");

        public string ForumText => _resourcesHelper.LocalizationGet("Forum");

        public string AccountText => _resourcesHelper.LocalizationGet("Account");

        public string LibraryText => _resourcesHelper.LocalizationGet("Library");

        public string FriendsText => _resourcesHelper.LocalizationGet("Friends");

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

        #region StoreUserControlVM

        private StoreUserControlViewModel _storeUserControlVM;

        public StoreUserControlViewModel StoreUserControlVM
        {
            get => _storeUserControlVM;
            set => Set(ref _storeUserControlVM, value);
        }

        #endregion

        #region SettingsUserControlVM

        private SettingsUserControlViewModel _settingsUserControlVM;

        public SettingsUserControlViewModel SettingsUserControlVM
        {
            get => _settingsUserControlVM;
            set => Set(ref _settingsUserControlVM, value);
        }

        #endregion

        #region MyAccountUserControlViewModel

        private MyAccountUserControlViewModel _myAccountUserControlVM;

        public MyAccountUserControlViewModel MyAccountUserControlVM
        {
            get => _myAccountUserControlVM;
            set => Set(ref _myAccountUserControlVM, value);
        }

        #endregion

        #region LibraryUserControlViewModel

        private LibraryUserControlViewModel _libraryUserControlVM;

        public LibraryUserControlViewModel LibraryUserControlVM
        {
            get => _libraryUserControlVM;
            set => Set(ref _libraryUserControlVM, value);
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


        #region ReloadPageCommand

        public Command ReloadPageCommand { get; }
        private bool CanReloadPageCommandExecute(object p) => true;
        private void OnReloadPageCommandExecuted(object p)
        {
            LoadData(_closeAction, new ResourcesHelperService(), new ServerRequestService());
            AllPropertyChanged();
        }

        #endregion


        #endregion


        public MainWindowViewModel(Action closeAction, Action dragMove, ToolbarToWindowViewModel toolbarViewModel, ResourcesHelperService resourcesHelper, ServerRequestService serverRequest)
        {
            _dragMoveAction = dragMove;
            _closeAction = closeAction;
            ToolbarVM = toolbarViewModel;
            _resourcesHelper = resourcesHelper;
            _serverRequest = serverRequest;
            _dialogWindow = new DialogWindowService();
            MoveWindowCommand = new LambdaCommand(OnMoveWindowCommandExecuted, CanMoveWindowCommandExecute);
            ReloadPageCommand = new LambdaCommand(OnReloadPageCommandExecuted, CanReloadPageCommandExecute);
            LoadData(closeAction, resourcesHelper, serverRequest);
        }

        private void LoadData(Action closeAction, ResourcesHelperService resourcesHelper, ServerRequestService serverRequest)
        {
            StoreUserControlVM = new StoreUserControlViewModel(resourcesHelper, serverRequest);
            UpdateUI.PullUi.Subscribe(StoreUserControlVM);
            SettingsUserControlVM = new SettingsUserControlViewModel(closeAction, resourcesHelper);
            MyAccountUserControlVM = new MyAccountUserControlViewModel();
            LibraryUserControlVM = new LibraryUserControlViewModel(resourcesHelper, serverRequest);
            UpdateUI.PullUi.Subscribe(LibraryUserControlVM);
        }


        public void Update(LoadUI data)
        {
            if (data.UpdateUI)
            {
                AllPropertyChanged();
            }
        }
    }
}
