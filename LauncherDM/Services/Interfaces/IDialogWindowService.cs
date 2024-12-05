using System;
using System.Windows;

namespace LauncherDM.Services.Interfaces
{
    public interface IDialogWindowService
    { 
        Action CloseAction { get; set; } 

        Action HideAction { get; set; } 

        Action DragMoveAction { get; set; } 

        void OpenWindow(object viewModel);
        
        void CloseWindow();
        
        void HideWindow();

        void DragMoveWindow();

        void OpenLoadingWindow();

        void OpenAccountRecovery();

        void OpenMainWindow();

        void OpenImageItemWindow(string imagePath);
    }
}
