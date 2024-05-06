namespace LauncherDM.Services.Interfaces
{
    interface ILoadingWindowService
    {
        bool CheckRequestServer();

        string GetTitle();

        bool CheckUpdate();
    }
}
