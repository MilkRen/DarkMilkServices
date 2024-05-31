using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Infrastructure.ReactiveUI;
using ServerTCP;
using ServerTCP.Models;

namespace LauncherDM.ViewModels
{
    internal class ItemsViewModel : ViewModel.Base.ViewModel, Infrastructure.ReactiveUI.Base.IObserver<LoadUI>
    {
        
        #region Binding

        #region Title

        private string _title;

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        #endregion

        #region ImageSource

        private string _imagePath;

        public string ImagePath
        {
            get => _imagePath;
            set => Set(ref _imagePath, value);
        }

        #endregion

        #region Price

        private string _price;

        public string Price
        {
            get => _price;
            set => Set(ref _price, value);
        }

        #endregion

        #region Description

        private string _description;

        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }

        #endregion

        #region Price

        private string _toolTipProgramsText;

        public string ToolTipProgramsText
        {
            get => _toolTipProgramsText;
            set => Set(ref _toolTipProgramsText, value);
        }

        #endregion


        #endregion

        #region Command

        public Command ClickProgramCommand { get; }
        
        #endregion

        public  ItemsViewModel(Programs prog, string progPath, LambdaCommand lambdaCommand = null)
        {
            Title = prog.name;
            ImagePath = string.Concat(progPath, Title, ".png");
            Price = prog.price.ToString();
            Description = MessageLanguages.Language == MessageLanguages.Languages.rus ? prog.description : prog.descriptionEng ;
            ToolTipProgramsText = string.Concat(Description.Substring(0, Description.Length / 4), "...");
            ClickProgramCommand = lambdaCommand;
        }

        public ItemsViewModel(Games games, string progPath, LambdaCommand lambdaCommand = null)
        {
            Title = games.name;
            ImagePath = string.Concat(progPath, Title, ".png");
            Price = games.price.ToString();
            Description = MessageLanguages.Language == MessageLanguages.Languages.rus ? games.description : games.descriptionEng;
            ToolTipProgramsText = string.Concat(Description.Substring(0, Description.Length / 4), "...");
            ClickProgramCommand = lambdaCommand;
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
