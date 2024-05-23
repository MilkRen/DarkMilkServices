namespace LauncherDM.Infrastructure.ReactiveUI
{
    public interface IObserver<TypeDefinition>
    {
        void Update(TypeDefinition data);
    }
}
