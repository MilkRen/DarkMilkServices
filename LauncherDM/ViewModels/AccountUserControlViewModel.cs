
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LauncherDM.ViewModels
{
    public class AccountUserControlViewModel : ViewModel.Base.ViewModel
    {
        #region Bindings

        #region NameAccount

        private string _accountName;

        public string AccountName
        {
            get => _accountName;
            set => Set(ref _accountName, value);
        }

        #endregion

        #region ImagePath

        private string _displayedImagePath;

        public string DisplayedImagePath
        {
            get => _displayedImagePath;
            set => Set(ref _displayedImagePath, value);
        }

        #endregion



        #endregion

        public AccountUserControlViewModel(string accountName, string imagePath)
        {
            AccountName = accountName;
            DisplayedImagePath = imagePath;
        }
    }
}
