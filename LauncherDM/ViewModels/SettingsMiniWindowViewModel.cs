﻿using LauncherDM.ViewModels.UserControlsVM;
using System;
using System.Windows.Input;
using System.Windows.Media;
using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Infrastructure;
using LauncherDM.Infrastructure.ReactiveUI;
using LauncherDM.Services;
using LauncherDM.Services.Interfaces;
using LauncherDM.Models;
using LauncherDM.Properties;
using ServerTCP;

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

        public string VersionText => string.Concat(_resourcesHelper.LocalizationGet("Version"), " ", StaticFields.VersionPatch);

        public string Title => _resourcesHelper.LocalizationGet("SettingsSmall");

        public string ToolTipLogoText => _resourcesHelper.LocalizationGet("AboutSmallText");

        public string Language => _resourcesHelper.LocalizationGet("Languages");

        public string RusText => _resourcesHelper.LocalizationGet("Russian");

        public string EngText => _resourcesHelper.LocalizationGet("English");

        #region Toolbar

        private ToolbarToWindowViewModel _toolbarVM;

        public ToolbarToWindowViewModel ToolbarVM
        {
            get => _toolbarVM;
            set => Set(ref _toolbarVM, value);
        }

        #endregion

        #region EnglishEnabled

        private SolidColorBrush _englishEnabled;

        public SolidColorBrush EnglishEnabled
        {
            get => _englishEnabled;
            set => Set(ref _englishEnabled, value);
        }

        #endregion

        #region RussianEnabled

        private SolidColorBrush _russianEnabled;

        public SolidColorBrush RussianEnabled
        {
            get => _russianEnabled;
            set => Set(ref _russianEnabled, value);
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

        #region RussianLangCommand

        public Command RussianLangCommand { get; }
        private bool CanRussianLangCommandExecute(object p) => true;
        private void OnRussianLangCommandExecuted(object p)
        {
            MessageLanguages.Language = MessageLanguages.Languages.rus;
            SettingsApp.Default.Language = "ru";
            SettingsApp.Default.Save();
            EnglishEnabled = Brushes.Transparent;
            RussianEnabled = Brushes.Green;
            UpdateUI.PullUi.Notify(new LoadUI(true));
            AllPropertyChanged();
        }

        #endregion

        #region EnglishLangCommand

        public Command EnglishLangCommand { get; }
        private bool CanEnglishLangCommandExecute(object p) => true;
        private void OnEnglishLangCommandExecuted(object p)
        {
            MessageLanguages.Language = MessageLanguages.Languages.eng;
            SettingsApp.Default.Language = "en";
            SettingsApp.Default.Save();
            EnglishEnabled = Brushes.Green;
            RussianEnabled = Brushes.Transparent;
            UpdateUI.PullUi.Notify(new LoadUI(true));
            AllPropertyChanged();
        }

        #endregion

        #endregion

        public SettingsMiniWindowViewModel(Action dragMove, ToolbarToWindowViewModel toolbarVM, ResourcesHelperService resourcesHelper)
        {
            _dragMoveAction = dragMove;
            ToolbarVM = toolbarVM;
            _resourcesHelper = resourcesHelper;
            _windowService = new DialogWindowService();
            MoveWindowCommand = new LambdaCommand(OnMoveWindowCommandExecuted, CanMoveWindowCommandExecute);
            RussianLangCommand = new LambdaCommand(OnRussianLangCommandExecuted, CanRussianLangCommandExecute);
            EnglishLangCommand = new LambdaCommand(OnEnglishLangCommandExecuted, CanEnglishLangCommandExecute);

            if (MessageLanguages.Language == MessageLanguages.Languages.eng)
                EnglishEnabled = Brushes.Green;
            else
                RussianEnabled = Brushes.Green;
        }
    }
}
