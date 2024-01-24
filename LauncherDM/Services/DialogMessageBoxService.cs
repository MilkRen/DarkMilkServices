using System.Collections.Generic;
using LauncherDM.Services.Interfaces;
using System.Windows;
 

namespace LauncherDM.Services
{
    class DialogMessageBoxService : IDialogMessageBoxService
    {
        public void DialogShow(string title, string text, CustomMessageBoxButton messageBoxButton = CustomMessageBoxButton.OK, CustomMessageBoxImage messageBoxImage = CustomMessageBoxImage.Information)
        {
            MessageBox.Show(text, title, MessageBoxButtonMapping[messageBoxButton], MessageBoxIconMapping[messageBoxImage]);
        }

        private static readonly Dictionary<CustomMessageBoxImage, MessageBoxImage> MessageBoxIconMapping = 
            new Dictionary<CustomMessageBoxImage, MessageBoxImage>
            {
                { CustomMessageBoxImage.None, MessageBoxImage.None },
                { CustomMessageBoxImage.Information, MessageBoxImage.Information },
                { CustomMessageBoxImage.Warning, MessageBoxImage.Warning },
                { CustomMessageBoxImage.Error, MessageBoxImage.Error },
                { CustomMessageBoxImage.Question, MessageBoxImage.Question },
                { CustomMessageBoxImage.Hand, MessageBoxImage.Hand },
                { CustomMessageBoxImage.Stop, MessageBoxImage.Stop },
                { CustomMessageBoxImage.Exclamation, MessageBoxImage.Exclamation },
                { CustomMessageBoxImage.Asterisk, MessageBoxImage.Asterisk },

            };

        private static readonly Dictionary<CustomMessageBoxButton, MessageBoxButton> MessageBoxButtonMapping =
            new Dictionary<CustomMessageBoxButton, MessageBoxButton>
            {
                { CustomMessageBoxButton.OK, MessageBoxButton.OK },
                { CustomMessageBoxButton.OKCancel, MessageBoxButton.OKCancel },
                { CustomMessageBoxButton.YesNoCancel, MessageBoxButton.YesNoCancel },
                { CustomMessageBoxButton.YesNo, MessageBoxButton.YesNo }
            };
    }
}
