using LauncherDM.ViewModels.UserControlsVM;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;
using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Models;

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



        #endregion

        #region Command

        public Command ClickProgramCommand { get; }
        private bool CanClickProgramCommandExecute(object p) => true;
        private void OnClickProgramCommandExecuted(object p)
        {
            MessageBox.Show("sd");
        }

        #endregion

        public ProgramsViewModel(Programs prog)
        {
            Title = prog.Title;
            ImagePath = prog.ImagePath;
            Price = prog.Price;
            Description = prog.Description;
            ClickProgramCommand = new LambdaCommand(OnClickProgramCommandExecuted, CanClickProgramCommandExecute);
        }
    }
}
