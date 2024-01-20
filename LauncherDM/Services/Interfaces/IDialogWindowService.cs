using System.Windows;

namespace LauncherDM.Services.Interfaces
{
    public interface IDialogWindowService
    {
       public void OpenWindow(Window window);

       public void CloseWindow(Window window);

       public void ShowWindow(Window window);

       public void HideWindow(Window window);
    }
}
