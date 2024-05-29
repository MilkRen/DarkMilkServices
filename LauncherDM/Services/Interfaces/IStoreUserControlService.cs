using ServerTCP.Models;

namespace LauncherDM.Services.Interfaces
{
    interface IStoreUserControlService
    {
        ProgramsForXml GetPrograms();
        
        string GetProgramsPath();
        
        GamesForXml GetGames();

        string GetGamesPath();
    }
}
