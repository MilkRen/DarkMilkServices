using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;

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

                DoubleAnimation buttonAnimation = new DoubleAnimation();
                buttonAnimation.From = Borderz.ActualWidth;
                buttonAnimation.To = 150;
                buttonAnimation.Duration = TimeSpan.FromSeconds(3);
                Borderz.BeginAnimation(Button.WidthProperty, buttonAnimation);
        }
    }
}
