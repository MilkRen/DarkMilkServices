using System;
using System.Windows;

namespace LauncherDM.Services.Interfaces
{
    public interface IDialogWindowService
    { 
        Action CloseAction { get; set; } 
        
        void OpenWindow(object viewModel);
        
        void CloseWindow();
        
        void ShowWindow(Window window);
        
        void HideWindow(Window window);

        void OpenLoadingWindow();
    }
}
