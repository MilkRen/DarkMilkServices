using System.Windows;

namespace LauncherDM.Services.Interfaces
{
    public enum CustomMessageBoxImage
    {
        None,
        Error, // 0x00000010
        Hand, // 0x00000010
        Stop, // 0x00000010
        Question, // 0x00000020
        Exclamation, // 0x00000030
        Warning, // 0x00000030
        Asterisk, // 0x00000040
        Information // 0x00000040
    }

    public enum CustomMessageBoxButton
    {
        OK,
        OKCancel,
        YesNoCancel,
        YesNo
    }

    public interface IDialogMessageBoxService
    { 
        void DialogShow(string title, string text, CustomMessageBoxButton messageBoxButton = CustomMessageBoxButton.OK, CustomMessageBoxImage messageBoxImage = CustomMessageBoxImage.Information);
    }
}
