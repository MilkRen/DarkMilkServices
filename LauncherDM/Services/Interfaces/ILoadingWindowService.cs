namespace LauncherDM.Services.Interfaces
{
    interface ILoadingWindowService
    {
        bool CheckRequestServer();

        string GetTitle();

        string DescInfoConnect();
    }
}
