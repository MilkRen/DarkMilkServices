using System;
using System.Windows;

namespace LauncherDM.Services.Interfaces
{
    public interface IDialogWindowService
    { 
        Action CloseAction { get; set; } 

        Action HideAction { get; set; } 

        Action MaxWindowAction { get; set; } 
        
        Action MinWindowAction { get; set; } 
        
        void OpenWindow(object viewModel);
        
        void CloseWindow();
        
        void HideWindow();

        void MaxWindow();

        void MinWindow();

        void OpenLoadingWindow();

        void OpenAccountRecovery();

    }
}
