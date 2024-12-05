using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace LauncherDM.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для LogoUserControl.xaml
    /// </summary>
    public partial class LogoUserControl : UserControl
    {
        //DispatcherTimer dispatcherTimer = new();
        public LogoUserControl()
        {
            InitializeComponent();
            //dispatcherTimer.Tick += (s, e) =>
            //{
            //    dispatcherTimer.Stop();
            //};
            //dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            //dispatcherTimer.Start();
        }
    }
}
