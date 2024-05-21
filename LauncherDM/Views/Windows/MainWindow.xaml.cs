using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace LauncherDM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var notifyIcon1 = new NotifyIcon();
            var logoIcon = (Icon)Properties.Resources.ResourceManager?.GetObject("OnlyLogo")!;
            notifyIcon1.Icon = logoIcon ?? SystemIcons.Exclamation;
            notifyIcon1.Text = "DarkMilk";

            notifyIcon1.ContextMenuStrip = new ContextMenuStrip();
            notifyIcon1.ContextMenuStrip.Items.Add("Store", Image.FromHbitmap(Properties.Resources.OnlyLogoTwoImage.GetHbitmap()), (sender, args) => {Show();});
            notifyIcon1.ContextMenuStrip.Items.Add("Close", Image.FromHbitmap(Properties.Resources.CloseBlack.GetHbitmap()), (sender, args) => { Environment.Exit(0); });
            notifyIcon1.Visible = true;
        }
    }
}
