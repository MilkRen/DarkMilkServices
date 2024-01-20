using LauncherDM.Services.Interfaces;
using System;
using System.Windows;

namespace LauncherDM.Services
{
    class DialogWindowService : IDialogWindowService
    {
        public void OpenWindow(Window window)
        {
            window.Show();
        }

        public void CloseWindow(Window window)
        {
            throw new NotImplementedException();
        }

        public void ShowWindow(Window window)
        {
            throw new NotImplementedException();
        }

        public void HideWindow(Window window)
        {
            throw new NotImplementedException();
        }
    }
}
