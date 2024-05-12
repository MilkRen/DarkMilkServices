
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

        private string _accountName;

        public string AccountName
        {
            get => _accountName;
            set => Set(ref _accountName, value);
        }

        #endregion

        public AccountUserControlViewModel(string accountName)
        {
            AccountName = accountName;
        }
    }
}
