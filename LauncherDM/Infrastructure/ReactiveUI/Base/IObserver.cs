namespace LauncherDM.Infrastructure.ReactiveUI.Base
{
    public interface IObserver<TypeDefinition>
    {
        void Update(TypeDefinition data);
    }
}
