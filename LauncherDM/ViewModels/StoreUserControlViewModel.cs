using System.Collections.ObjectModel;
using System.Windows.Controls;
using LauncherDM.Infastructure.Commands;
using LauncherDM.Services;
using LauncherDM.Services.Interfaces;
using System.Windows.Media.Animation;
using System;
using System.Drawing;
using System.Windows.Input;
using LauncherDM.Infastructure.Commands.Base;
using System.Windows.Media;
using ServerTCP.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Taskbar;

namespace LauncherDM.ViewModels
{
    internal class StoreUserControlViewModel : ViewModel.Base.ViewModel
    {
        //Todo: убрать эти костыли 

        public static GamesForXml gamesArray;

        public static ProgramsForXml progArray;

        #region Fields

        private const int MaxImageToItem = 5;

        #endregion

        #region Services

        private readonly IResourcesHelperService _resourcesHelper;

        private readonly IDialogWindowService _dialogWindow;

        private readonly IStoreUserControlService _storeService;

        #endregion

        #region Binding

        public string ProgramText => _resourcesHelper.LocalizationGet("Programs");

        public string GamesText => _resourcesHelper.LocalizationGet("Games");

        private string _payButtonText = "AddLibrary";
        public string PayButtonText => _resourcesHelper.LocalizationGet(_payButtonText);

        #region ProgramsViewModel

        private ObservableCollection<ItemsViewModel> _programsListView;

        public ObservableCollection<ItemsViewModel> ProgramsListView
        {
            get => _programsListView;
            set => Set(ref _programsListView, value);
        }

        #endregion

        #region GamesViewModel

        private ObservableCollection<ItemsViewModel> _gamesListView;

        public ObservableCollection<ItemsViewModel> GamesListView
        {
            get => _gamesListView;
            set => Set(ref _gamesListView, value);
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

        private ObservableCollection<SelectItemViewModel> _itemListView;

        public ObservableCollection<SelectItemViewModel> ItemListView
        {
            get => _itemListView;
            set => Set(ref _itemListView, value);
        }

        private SolidColorBrush _backgroundButton;

        public SolidColorBrush BackgroundButton
        {
            get => _backgroundButton;
            set => Set(ref _backgroundButton, value);
        }

        #endregion

        #endregion

        #region Command

        #region ClickImageItemCommand

        public Command ClickImageItemCommand { get; }
        private bool CanClickImageItemCommandExecute(object p) => true;
        private void OnClickImageItemCommandExecuted(object p)
        {
            _dialogWindow.OpenImageItemWindow(ImageItem);
        }

        #endregion

        #region SaleItemCommand

        public Command SaleItemCommand { get; }
        private bool CanSaleItemCommandExecute(object p) => true;
        private void OnSaleItemCommandExecuted(object p)
        {
            if (_storeService.SaleItem(TitleItem))
            {

            }
            BackgroundButton = (SolidColorBrush)new BrushConverter().ConvertFrom("#33c02b");
            _payButtonText = "InTheLibrary";
            OnPropertyChanged("PayButtonText");
        }

        #endregion

        #endregion

        // Todo: надо убрать это безобразие 
        public static Border ItemProgram;

        public StoreUserControlViewModel(ResourcesHelperService resourcesHelper, ServerRequestService serverRequest)
        {
            BackgroundButton = (SolidColorBrush)new BrushConverter().ConvertFrom("#738aea");
            _resourcesHelper = resourcesHelper;
            _dialogWindow = new DialogWindowService();
            ProgramsListView = new ObservableCollection<ItemsViewModel>();
            GamesListView = new ObservableCollection<ItemsViewModel>();
            ClickImageItemCommand = new LambdaCommand(OnClickImageItemCommandExecuted, CanClickImageItemCommandExecute);
            SaleItemCommand = new LambdaCommand(OnSaleItemCommandExecuted, CanSaleItemCommandExecute);
            _storeService = new StoreUserControlService(serverRequest);
            LoadStore(serverRequest);
        }

        public void LoadStore(ServerRequestService serverRequest)
        {
            ICheckNetworkService networkService = new CheckNetworkService();
            progArray = _storeService.GetPrograms();
            var progPath = _storeService.GetProgramsPath();
            foreach (var prog in progArray.ProgramsArray)
            {
                ProgramsListView.Add(new ItemsViewModel(prog, progPath, new LambdaCommand(o =>
                {
                    TitleItem = prog.name;
                    TagText = prog.tag;
                    ImageItem = string.Concat(progPath, TitleItem, ".png"); 
                    DescItem = prog.description;

                    var countLoadImage = 1;
                    ItemListView = new ObservableCollection<SelectItemViewModel>();
                    while (MaxImageToItem >= countLoadImage)
                    {
                        var image = string.Concat(progPath, TitleItem,
                            countLoadImage.ToString(), ".png");
                        if (networkService.CheckingUriFileConnection(image))
                            ItemListView.Add(new SelectItemViewModel(image, new LambdaCommand(o =>
                            {
                                ImageItem = image;
                            }, o => true)));
                        else
                            break;

                        countLoadImage++;
                    }

                    BackgroundButton = (SolidColorBrush)new BrushConverter().ConvertFrom("#738aea");
                    _payButtonText = "AddLibrary";
                    OnPropertyChanged("PayButtonText");
                    AnimationItemShow();
                }, o => true)));
            }

            gamesArray = _storeService.GetGames();
            var gamesPath = _storeService.GetGamesPath();
            foreach (var games in gamesArray.GamesArray)
            {
                GamesListView.Add(new ItemsViewModel(games, gamesPath, new LambdaCommand(o =>
                {
                    TitleItem = games.name;
                    TagText = games.tag;
                    ImageItem = string.Concat(gamesPath, TitleItem, ".png");
                    DescItem = games.description;

                    var countLoadImage = 1;
                    ItemListView = new ObservableCollection<SelectItemViewModel>();
                    while (MaxImageToItem >= countLoadImage)
                    {
                        var image = string.Concat(gamesPath, TitleItem,
                            countLoadImage.ToString(), ".png");
                        if (networkService.CheckingUriFileConnection(image))
                            ItemListView.Add(new SelectItemViewModel(image, new LambdaCommand(o =>
                            {
                                ImageItem = image;
                            }, o => true)));
                        else
                            break;

                        countLoadImage++;
                    }

                    BackgroundButton = (SolidColorBrush)new BrushConverter().ConvertFrom("#738aea");
                    _payButtonText = "AddLibrary";
                    OnPropertyChanged("PayButtonText");
                    AnimationItemShow();
                }, o => true)));
            }
        }

        private void AnimationItemShow()
        {
            var buttonAnimation = new DoubleAnimation();
            buttonAnimation.From = ItemProgram.ActualWidth;
            buttonAnimation.To = 400;
            buttonAnimation.Duration = TimeSpan.FromSeconds(1);
            ItemProgram.BeginAnimation(Button.WidthProperty, buttonAnimation);
        }
    }
}
