using System;
using LauncherDM.Services.Interfaces;

namespace LauncherDM.Services
{
    class ApplicationService : IApplicationService
    {
        public void CloseApplication()
        {
            Environment.Exit(0);
        }
    }
}
