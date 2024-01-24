using System.Windows;

namespace LauncherDM.Services.Interfaces
{
    public interface IDialogWindowService
    {
       public void OpenWindow(object viewModel);

       public void CloseWindow(Window window);

       public void ShowWindow(Window window);

       public void HideWindow(Window window);

       public void OpenLoadingWindow();
    }
}
