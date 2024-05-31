using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;

namespace LauncherDM.ViewModels
{
    class LibrarySelectItemViewModel : ViewModel.Base.ViewModel
    {
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

        #region Command

        public Command ClickItemCommand { get; }

        #endregion

        public LibrarySelectItemViewModel(string nameItem, string imageItemPath, LambdaCommand lambdaCommand = null)
        {
            NameItem = nameItem;
            ImageItemPath = imageItemPath;
            ClickItemCommand = lambdaCommand;
        }
    }
}
