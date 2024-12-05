namespace LauncherDM.Services.Interfaces
{
    internal interface ISignUpService
    {
        bool SignUp(string login, string email, string password);
    }
}
