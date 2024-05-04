﻿using System;
using LauncherDM.Services.Interfaces;
using ServerTCP;
using System.Globalization;
using System.Threading;
using LauncherDM.Properties;

namespace LauncherDM.Services
{
    class ResourcesHelperService : IResourcesHelperService
    {
        public string LocalizationGet(string resource)
        {
            if (string.IsNullOrEmpty(resource))
                throw new ArgumentNullException(nameof(resource));

            string resourceReady = string.Empty;
            switch (MessageLanguages.Language)
            {
                case MessageLanguages.Languages.rus:
                    resourceReady =  Resources.ResourceManager.GetString(resource, new CultureInfo("ru-RU"));
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
