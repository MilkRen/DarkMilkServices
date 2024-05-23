using System.Windows.Controls;
using LauncherDM.ViewModels;

namespace LauncherDM.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для StoreUserControl.xaml
    /// </summary>
    public partial class StoreUserControl : UserControl
    {
        public StoreUserControl()
        {
                InitializeComponent();
                // Todo: это безобразие надо будет убрать!
                StoreUserControlViewModel.ItemProgram = BorderItem;
        }
    }
}
