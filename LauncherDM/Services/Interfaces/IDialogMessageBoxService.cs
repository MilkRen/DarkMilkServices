using System.Windows;

namespace LauncherDM.Services.Interfaces
{
    public enum CustomMessageBoxImage
    {
        None = 0,
        Error = 16, // 0x00000010
        Hand = 16, // 0x00000010
        Stop = 16, // 0x00000010
        Question = 32, // 0x00000020
        Exclamation = 48, // 0x00000030
        Warning = 48, // 0x00000030
        Asterisk = 64, // 0x00000040
        Information = 64 // 0x00000040
    }

    public enum CustomMessageBoxButton
    {
        OK = 0,
        OKCancel = 1,
        YesNoCancel = 3,
        YesNo = 4
    }

    public interface IDialogMessageBoxService
    { 
        void DialogShow(string title, string text, CustomMessageBoxButton messageBoxButton = CustomMessageBoxButton.OK, CustomMessageBoxImage messageBoxImage = CustomMessageBoxImage.Information);
    }
}
