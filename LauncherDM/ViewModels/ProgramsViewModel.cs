using System;
using LauncherDM.ViewModels.UserControlsVM;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;
using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Models;
using ServerTCP;
using ServerTCP.Models;

namespace LauncherDM.ViewModels
{
    internal class ProgramsViewModel : ViewModel.Base.ViewModel
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

        public  ProgramsViewModel(Programs prog, string progPath, LambdaCommand lambdaCommand = null)
        {
            Title = prog.name;
            ImagePath = string.Concat(progPath, Title, ".png");
            Price = prog.price.ToString();
            Description = MessageLanguages.Language == MessageLanguages.Languages.rus ? prog.description : prog.descriptionEng ;
            ToolTipProgramsText = string.Concat(Description.Substring(0, Description.Length / 4), "...");
            ClickProgramCommand = lambdaCommand;
        }
    }
}
