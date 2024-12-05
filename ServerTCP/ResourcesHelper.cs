using ServerTCP.Properties;
using System.Globalization;

namespace ServerTCP
{
    internal class ResourcesHelper
    {
        public static string LocalizationGet(string resource, MessageLanguages.Languages lang)
        {
            if (string.IsNullOrEmpty(resource))
                throw new ArgumentNullException(nameof(resource));

            string resourceReady = string.Empty;
            switch (lang)
            {
                case MessageLanguages.Languages.rus:
                    resourceReady = Resources.ResourceManager.GetString(resource, new CultureInfo("ru-RU"));
                    break;
                default:
                    resourceReady = Resources.ResourceManager.GetString(resource, new CultureInfo("en-GB"));
                    break;
            }

            if (string.IsNullOrEmpty(resourceReady))
                throw new ArgumentNullException(nameof(resourceReady));

            return resourceReady;
        }
    }
}
