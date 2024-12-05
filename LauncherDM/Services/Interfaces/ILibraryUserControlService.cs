using ServerTCP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherDM.Services.Interfaces
{
    interface ILibraryUserControlService
    {
        SaleGamesForXml GetGamesItem();

        SaleProgramsForXml GetProgramItem();
    }
}
