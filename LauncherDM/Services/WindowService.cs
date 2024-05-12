using System.Windows;
using LauncherDM.Services.Interfaces;

namespace LauncherDM.Services
{
    public class WindowService : IWindowService
    {
        public Window Window { get; }

        public WindowService(Window window)
        {
            Window = window;
        }
    }
}
