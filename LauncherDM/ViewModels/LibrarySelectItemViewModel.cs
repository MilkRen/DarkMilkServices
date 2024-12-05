using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Infrastructure.ReactiveUI;
using LauncherDM.Services;
using LauncherDM.Services.Interfaces;
using System.IO;
using System.Runtime;
using System.Windows;

namespace LauncherDM.ViewModels
{
    class LibrarySelectItemViewModel : ViewModel.Base.ViewModel
    {
        private readonly IResourcesHelperService _resourcesHelper;

        public string DeleteProg => _resourcesHelper.LocalizationGet("DeleteProg");





        private string _nameItem;

        public string NameItem
        {
            get => _nameItem;
            set => Set(ref _nameItem, value);
        }


        #region ImageItemPath

        private string _imageItemPath;

        public string ImageItemPath
        {
            get => _imageItemPath;
            set => Set(ref _imageItemPath, value);
        }

        #endregion



        private Visibility _vis;

        public Visibility Vis
        {
            get => _vis;
            set => Set(ref _vis, value);
        }



        #region Command

        public Command ClickItemCommand { get; }


        public Command DeleteProgCommand { get; }
        private bool CanDeleteProgCommandExecute(object p) => true;
        private void OnDeleteProgCommandExecuted(object p)
        {
            if (Directory.Exists("DownloadFile\\" + NameItem))
            {

                Vis = Visibility.Hidden;
                Directory.Delete("DownloadFile\\" + NameItem, true);
                File.Delete("DownloadFile\\" + NameItem + ".rar");
                return;
            }
        }


        #endregion

        public LibrarySelectItemViewModel(string nameItem, string imageItemPath, LambdaCommand lambdaCommand = null)
        {
            NameItem = nameItem;
            ImageItemPath = imageItemPath;
            ClickItemCommand = lambdaCommand;
            _resourcesHelper = new ResourcesHelperService();
            DeleteProgCommand = new LambdaCommand(OnDeleteProgCommandExecuted, CanDeleteProgCommandExecute);
        }
    }
}
