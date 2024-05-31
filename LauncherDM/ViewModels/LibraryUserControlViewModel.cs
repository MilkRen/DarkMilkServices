using LauncherDM.Infastructure.Commands;
using LauncherDM.Services;
using System.Collections.ObjectModel;
using System.Windows.Forms.VisualStyles;
using LauncherDM.Services.Interfaces;

namespace LauncherDM.ViewModels
{
    class LibraryUserControlViewModel : ViewModel.Base.ViewModel
    {
        #region Service

        private readonly IResourcesHelperService _resourcesHelper;

        private readonly ILibraryUserControlService _libraryUserService;

        #endregion

        #region Bindings

        public string ItemTitle => _resourcesHelper.LocalizationGet("Programs");


        #region ItemListView

        private ObservableCollection<LibrarySelectItemViewModel> _itemListView;

        public ObservableCollection<LibrarySelectItemViewModel> ItemListView
        {
            get => _itemListView;
            set => Set(ref _itemListView, value);
        }

        #endregion

        #endregion



        public LibraryUserControlViewModel(ResourcesHelperService resourcesHelper, ServerRequestService serverRequest)
        {
            _resourcesHelper = resourcesHelper;
            _libraryUserService = new LibraryUserControlService(serverRequest);
            ItemListView = new ObservableCollection<LibrarySelectItemViewModel>();
            LoadItems();
        }

        private void LoadItems()
        {
          var a =  StoreUserControlViewModel.progArray;
           var b = StoreUserControlViewModel.gamesArray;

           var g = _libraryUserService.GetAllItems();
            ItemListView.Add(new LibrarySelectItemViewModel("sdads", "https://get.wallhere.com/photo/anime-anime-girls-2235748.jpg", new LambdaCommand(o =>
            {
                //ImageItem = image;
            }, o => true)));
            ItemListView.Add(new LibrarySelectItemViewModel("sdads", "https://get.wallhere.com/photo/anime-anime-girls-2235748.jpg"));
            ItemListView.Add(new LibrarySelectItemViewModel("sdads", "https://get.wallhere.com/photo/anime-anime-girls-2235748.jpg"));
            ItemListView.Add(new LibrarySelectItemViewModel("sdads", "https://get.wallhere.com/photo/anime-anime-girls-2235748.jpg"));
            ItemListView.Add(new LibrarySelectItemViewModel("sdads", "https://get.wallhere.com/photo/anime-anime-girls-2235748.jpg"));
        }
    }
}
