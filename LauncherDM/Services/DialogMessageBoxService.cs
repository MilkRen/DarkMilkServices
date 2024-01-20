using LauncherDM.Services.Interfaces;
using System.Windows;
 

namespace LauncherDM.Services
{
    class DialogMessageBoxService : IDialogMessageBoxService
    {
        public void DialogShow(string title, string text)
        {
            MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
