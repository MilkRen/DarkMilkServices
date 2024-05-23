using LauncherDM.Models;
using System.Collections.ObjectModel;
using System.Windows;
using LauncherDM.Services;
using LauncherDM.Services.Interfaces;

namespace LauncherDM.ViewModels
{
    internal class StoreUserControlViewModel : ViewModel.Base.ViewModel
    {

        #region Services

        private readonly IResourcesHelperService _resourcesHelper;

        #endregion

        #region Binding

        public string ProgramText => _resourcesHelper.LocalizationGet("Programs");

        public string GamesText => _resourcesHelper.LocalizationGet("Games");
        public bool IsOnline = true;

        #region ProgramsViewModel

        private ObservableCollection<ProgramsViewModel> _programsListView;

        public ObservableCollection<ProgramsViewModel> ProgramsListView
        {
            get => _programsListView;
            set => Set(ref _programsListView, value);
        }

        #endregion

        #endregion

        public StoreUserControlViewModel(ResourcesHelperService resourcesHelper)
        {
            _resourcesHelper = resourcesHelper;
            ProgramsListView = new ObservableCollection<ProgramsViewModel>();
            int a = 0;
            while (a < 20)
            {
                var prog = new Programs()
                {
                   Title = "StrikeJo",
                   ImagePath = "https://static1.cbrimages.com/wordpress/wp-content/uploads/2024/02/every-joestar-in-jojo-s-bizarre-adventure.jpg?q=50&fit=contain&w=1140&h=&dpr=1.5",
                   Price = "4999",
                   Description = "asdasds"
                };
                ProgramsListView.Add(new ProgramsViewModel(prog));
                a++;
            }
        }
    }
}
