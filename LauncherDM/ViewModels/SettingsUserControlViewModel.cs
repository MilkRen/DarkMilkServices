using LauncherDM.Services.Interfaces;
using System;
using LauncherDM.Services;
using System.Windows.Input;
using LauncherDM.Infastructure.Commands.Base;
using System.Windows.Media;
using LauncherDM.Infrastructure.ReactiveUI;
using LauncherDM.Infrastructure;
using LauncherDM.Properties;
using ServerTCP;
using LauncherDM.Infastructure.Commands;

namespace LauncherDM.ViewModels
{
    internal class SettingsUserControlViewModel : ViewModel.Base.ViewModel 
    {
        #region Services

        private readonly IResourcesHelperService _resourcesHelper;

        private readonly IDialogWindowService _dialogWindow;

        #endregion

        #region Fields

        private Action _actionClose;

        #endregion

        #region Bindings

        public string ChangeAccountText => _resourcesHelper.LocalizationGet("AccountChange");

        public string RusText => _resourcesHelper.LocalizationGet("Russian");

        public string EngText => _resourcesHelper.LocalizationGet("English");

        public string Language => _resourcesHelper.LocalizationGet("Languages");

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

        #region ChangeAccountCommand

        public Command ChangeAccountCommand { get; }
        private bool CanChangeAccountCommandExecute(object p) => true;
        private void OnChangeAccountCommandExecuted(object p)
        {
            _dialogWindow.OpenWindow(this);
            _dialogWindow.CloseAction = _actionClose;
            _dialogWindow.CloseWindow();
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

        public SettingsUserControlViewModel(Action actionClose, ResourcesHelperService resourcesHelper)
        {
            _actionClose = actionClose;
            _resourcesHelper = resourcesHelper;
            _dialogWindow = new DialogWindowService();
            RussianLangCommand = new LambdaCommand(OnRussianLangCommandExecuted, CanRussianLangCommandExecute);
            EnglishLangCommand = new LambdaCommand(OnEnglishLangCommandExecuted, CanEnglishLangCommandExecute);
            ChangeAccountCommand = new LambdaCommand(OnChangeAccountCommandExecuted, CanChangeAccountCommandExecute);

            if (MessageLanguages.Language == MessageLanguages.Languages.eng)
                EnglishEnabled = Brushes.Green;
            else
                RussianEnabled = Brushes.Green;
        }
    }
}
