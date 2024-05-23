using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;

namespace LauncherDM.ViewModels
{
    class ProgramItemViewModel : ViewModel.Base.ViewModel  
    {
        #region Binding

        #region Title

        private string _imageItemPath;

        public string ImageItemPath
        {
            get => _imageItemPath;
            set => Set(ref _imageItemPath, value);
        }

        #endregion


        #endregion

        #region Command

        public Command ClickItemCommand { get; }

        #endregion

        public ProgramItemViewModel(string imageItemPath, LambdaCommand lambdaCommand = null)
        {
            ImageItemPath = imageItemPath;
            ClickItemCommand = lambdaCommand;
        }
    }
}
