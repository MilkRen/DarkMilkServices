using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LauncherDM.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для LogoUserControl.xaml
    /// </summary>
    public partial class LogoUserControl : UserControl
    {
        DispatcherTimer dispatcherTimer = new();
        public LogoUserControl()
        {
            InitializeComponent();
            dispatcherTimer.Tick += (s, e) =>
            {
                dispatcherTimer.Stop();
                NavigationService.Navigate(new MenuPage());
            };
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer.Start();
        }
    }
}
