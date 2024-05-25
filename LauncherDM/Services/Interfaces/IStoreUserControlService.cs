using ServerTCP.Models;

namespace LauncherDM.Services.Interfaces
{
    interface IStoreUserControlService
    {
        ProgramsForXml GetPrograms();
        
        string GetProgramsPath();
        
        object GetGames();

        string GetGamesPath();
    }
}
