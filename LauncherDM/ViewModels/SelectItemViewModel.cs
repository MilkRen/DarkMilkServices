using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Infrastructure.ReactiveUI;

namespace LauncherDM.ViewModels
{
    class SelectItemViewModel : ViewModel.Base.ViewModel, Infrastructure.ReactiveUI.Base.IObserver<LoadUI>
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

        public SelectItemViewModel(string imageItemPath, LambdaCommand lambdaCommand = null)
        {
            ImageItemPath = imageItemPath;
            ClickItemCommand = lambdaCommand;
        }

        public void Update(LoadUI data)
        {
            if (data.UpdateUI)
            {
                AllPropertyChanged();
            }
        }
    }
}
