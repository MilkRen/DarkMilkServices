using System.Collections.ObjectModel;
using System.Windows.Controls;
using LauncherDM.Infastructure.Commands;
using LauncherDM.Services;
using LauncherDM.Services.Interfaces;
using System.Windows.Media.Animation;
using System;

namespace LauncherDM.ViewModels
{
    internal class StoreUserControlViewModel : ViewModel.Base.ViewModel
    {
        #region Fields

        private const int MaxImageToItem = 5;

        #endregion

        #region Services

        private readonly IResourcesHelperService _resourcesHelper;

        #endregion

        #region Binding

        public string ProgramText => _resourcesHelper.LocalizationGet("Programs");

        public string GamesText => _resourcesHelper.LocalizationGet("Games");

        public string PayButtonText => _resourcesHelper.LocalizationGet("AddLibrary");

        #region ProgramsViewModel

        private ObservableCollection<ProgramsViewModel> _programsListView;

        public ObservableCollection<ProgramsViewModel> ProgramsListView
        {
            get => _programsListView;
            set => Set(ref _programsListView, value);
        }

        #endregion

        #region SelectItem

        private string _titleItem;

        public string TitleItem
        {
            get => _titleItem;
            set => Set(ref _titleItem, value);
        }

        private string _imageItem;

        public string ImageItem
        {
            get => _imageItem;
            set => Set(ref _imageItem, value);
        }

        private string _descItem;

        public string DescItem
        {
            get => _descItem;
            set => Set(ref _descItem, value);
        }

        private string _tagText;

        public string TagText
        {
            get => _tagText;
            set => Set(ref _tagText, value);
        }

        private ObservableCollection<ProgramItemViewModel> _itemListView;

        public ObservableCollection<ProgramItemViewModel> ItemListView
        {
            get => _itemListView;
            set => Set(ref _itemListView, value);
        }

        #endregion

        #endregion

        // Todo: надо убрать это безобразие 
        public static Border ItemProgram;

        public StoreUserControlViewModel(ResourcesHelperService resourcesHelper, ServerRequestService serverRequest)
        {
            _resourcesHelper = resourcesHelper;
            ProgramsListView = new ObservableCollection<ProgramsViewModel>();
            LoadStore(serverRequest);
        }

        public void LoadStore(ServerRequestService serverRequest)
        {
            IStoreUserControlService store = new StoreUserControlService(serverRequest);
            ICheckNetworkService networkService = new CheckNetworkService();
            var progArray = store.GetPrograms();
            var progPath = store.GetProgramsPath();
            foreach (var prog in progArray.Programs)
            {
                ProgramsListView.Add(new ProgramsViewModel(prog, progPath, new LambdaCommand(o =>
                {
                    TitleItem = prog.name;
                    TagText = prog.tag;
                    ImageItem = string.Concat(progPath, TitleItem, ".png"); 
                    DescItem = prog.description;

                    var countLoadImage = 1;
                    ItemListView = new ObservableCollection<ProgramItemViewModel>();
                    while (MaxImageToItem >= countLoadImage)
                    {
                        var image = string.Concat(progPath, TitleItem,
                            countLoadImage.ToString(), ".png");
                        if (networkService.CheckingUriFileConnection(image))
                            ItemListView.Add(new ProgramItemViewModel(image, new LambdaCommand(o =>
                            {
                                ImageItem = image;
                            }, o => true)));
                        else
                            break;

                        countLoadImage++;
                    }

                    AnimationItemShow();
                }, o => true)));
            }
        }

        public void AnimationItemShow()
        {
            var buttonAnimation = new DoubleAnimation();
            buttonAnimation.From = ItemProgram.ActualWidth;
            buttonAnimation.To = 400;
            buttonAnimation.Duration = TimeSpan.FromSeconds(1);
            ItemProgram.BeginAnimation(Button.WidthProperty, buttonAnimation);
        }
    }
}
