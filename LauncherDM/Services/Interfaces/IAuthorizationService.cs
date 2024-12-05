namespace LauncherDM.Services.Interfaces
{
    internal interface IAuthorizationService
    {
        bool Authorization(string login, string password);
    }
}
