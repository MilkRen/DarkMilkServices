using LauncherDM.Infastructure.Commands;
using LauncherDM.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using LauncherDM.Infrastructure.ReactiveUI;
using LauncherDM.Services.Interfaces;
using ServerTCP;
using Amazon.S3.Model;
using System.Security.Cryptography;
using LauncherDM.Infrastructure;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Taskbar;
using System.Windows.Media;
using LauncherDM.Infastructure.Commands.Base;

namespace LauncherDM.ViewModels
{
    class LibraryUserControlViewModel : ViewModel.Base.ViewModel, Infrastructure.ReactiveUI.Base.IObserver<LoadUI>
    {
        #region Service

        private readonly IResourcesHelperService _resourcesHelper;

        private readonly ILibraryUserControlService _libraryUserService;

        private readonly IDialogWindowService _dialogWindow;

        private readonly IServerRequestService _serverRequest;

        #endregion

        #region Fields

        private const int MaxImageToItem = 5;

        #endregion

        #region Bindings

        public string ItemTitle => _resourcesHelper.LocalizationGet("Programs");

        public string ButDow => _resourcesHelper.LocalizationGet("Download");

        public string ButRun => _resourcesHelper.LocalizationGet("Run");

        #region SourceMedia

        private string _sourceMedia = DefaultMediaSource;

        public string SourceMedia
        {
            get => _sourceMedia;
            set => Set(ref _sourceMedia, value);
        }

        #endregion

        #region Title

        private string _title = "---";

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        #endregion

        #region ItemName

        private string _itemName;

        public string ItemName
        {
            get => _itemName;
            set => Set(ref _itemName, value);
        }

        #endregion

        #region Desc

        private string _descItem;

        public string DescItem
        {
            get => _descItem;
            set => Set(ref _descItem, value);
        }

        #endregion

        #region Tags

        private string _tags;

        public string Tags
        {
            get => _tags;
            set => Set(ref _tags, value);
        }

        #endregion

        #region ButtonWidth

        private int _widthDownload = 0;

        public int WidthDownload
        {
            get => _widthDownload;
            set => Set(ref _widthDownload, value);
        }

        private int _widthRun = 0;

        public int WidthRun
        {
            get => _widthRun;
            set => Set(ref _widthRun, value);
        }

        #endregion

        #region ItemListView

        private ObservableCollection<LibrarySelectItemViewModel> _itemListView;

        public ObservableCollection<LibrarySelectItemViewModel> ItemListView
        {
            get => _itemListView;
            set => Set(ref _itemListView, value);
        }

        #endregion

        #region ImageListView

        private ObservableCollection<SelectItemViewModel> _imageListView;
        public ObservableCollection<SelectItemViewModel> ImageListView
        {
            get => _imageListView;
            set => Set(ref _imageListView, value);
        }

        #endregion

        #endregion

        #region Command

        public Command DownloadCommand { get; }
        private bool CanDownloadCommandExecute(object p) => true;
        private void OnDownloadCommandExecuted(object p)
        {
            
        }

        public Command RunCommand { get; }
        private bool CanRunCommandExecute(object p) => true;
        private void OnRunCommandExecuted(object p)
        {

        }


        #endregion


        public LibraryUserControlViewModel(ResourcesHelperService resourcesHelper, ServerRequestService serverRequest)
        {
            _resourcesHelper = resourcesHelper;
            _serverRequest = serverRequest;
            _libraryUserService = new LibraryUserControlService(serverRequest);
            _dialogWindow = new DialogWindowService();
            DownloadCommand = new LambdaCommand(OnDownloadCommandExecuted, CanDownloadCommandExecute);
            RunCommand = new LambdaCommand(OnRunCommandExecuted, CanRunCommandExecute);
            LoadItems();
        }

        private void LoadItems()
        {
            ItemListView = new ObservableCollection<LibrarySelectItemViewModel>();
            ImageListView = new ObservableCollection<SelectItemViewModel>();

            var preLoadProg =  StoreUserControlViewModel.progArray;
            var preLoadGame = StoreUserControlViewModel.gamesArray;
            var gamePath = StoreUserControlViewModel.gamesPath;
            var progPath = StoreUserControlViewModel.progPath;

            var games = _libraryUserService.GetGamesItem();
            var programs = _libraryUserService.GetProgramItem();

            ICheckNetworkService networkService = new CheckNetworkService();
            foreach (var game in games.SaleGamesArray)
            {
                var gameSelect = preLoadGame.GamesArray.First(x => x.id == game.game_id);
                var image = string.Concat(gamePath, gameSelect.name, ".png");
                ItemListView.Add(new LibrarySelectItemViewModel(gameSelect.name, image,
                    new LambdaCommand(o =>
                    {
                        NowGame = true;
                        TitleLoad(true);
                        ItemName = gameSelect.name;
                        Tags = gameSelect.tag;
                        WidthDownload = 150;
                        SourceMedia = string.Concat(gamePath, gameSelect.name, ".mp4");

                        // Todo:  костыль
                        DescEng = gameSelect.descriptionEng; // костыль
                        Desc = gameSelect.description; // костыль
                        DescLoad();
                        ImageListView.Clear();

                        var countLoadImage = 1;
                        while (MaxImageToItem >= countLoadImage)
                        {
                            var imageDop = string.Concat(gamePath, gameSelect.name,
                                countLoadImage.ToString(), ".png");
                            if (networkService.CheckingUriFileConnection(imageDop))
                                ImageListView.Add(new SelectItemViewModel(imageDop, new LambdaCommand(o =>
                                {
                                    _dialogWindow.OpenImageItemWindow(imageDop);
                                }, o => true)));
                            else
                                break;

                            countLoadImage++;
                        }

                    }, o => true)));
            }

            foreach (var prog in programs.SaleProgramsArray)
            {
                var progSelect = preLoadProg.ProgramsArray.First(x => x.id == prog.program_id);
                var image = string.Concat(progPath, progSelect.name, ".png");
                ItemListView.Add(new LibrarySelectItemViewModel(progSelect.name, image,
                    new LambdaCommand(o =>
                    {
                        // Todo:  костыль
                        NowGame = false;
                        TitleLoad(false);
                        DescEng = progSelect.descriptionEng;
                        Desc = progSelect.description;
                        DescLoad();
                        ItemName = progSelect.name;
                        Tags = progSelect.tag;
                        WidthDownload = 150;
                        ImageListView.Clear();
                        //SourceMedia = DefaultMediaSource1;

                        var countLoadImage = 1;
                        while (MaxImageToItem >= countLoadImage)
                        {
                            var imageDop = string.Concat(progPath, progSelect.name,
                                countLoadImage.ToString(), ".png");
                            if (networkService.CheckingUriFileConnection(imageDop))
                                ImageListView.Add(new SelectItemViewModel(imageDop, new LambdaCommand(o =>
                                {
                                    _dialogWindow.OpenImageItemWindow(imageDop);
                                }, o => true)));
                            else
                                break;

                            countLoadImage++;
                        }


                    }, o => true)));
            }
        }

        private bool NowGame;

        private string DescEng;

        private string Desc;

        private const string DefaultMediaSource = "https://darkmilk.store/Launcher/Video/wallper.mp4";


        private void TitleLoad(bool game)
        {
            Title = game ? _resourcesHelper.LocalizationGet("Game") : _resourcesHelper.LocalizationGet("Program");
        }

        private void DescLoad()
        {
            DescItem = MessageLanguages.Language == MessageLanguages.Languages.rus ? Desc : DescEng;
        }

        public void Update(LoadUI data)
        {
            if (data.UpdateUI)
            {
                AllPropertyChanged();
                TitleLoad(NowGame);
                DescLoad();
                LoadItems();
            }
        }
    }
}
