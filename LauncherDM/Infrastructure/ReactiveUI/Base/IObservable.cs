namespace LauncherDM.Infrastructure.ReactiveUI.Base
{
    public interface IObservable<TypeDefinition>
    {
        void Notify(TypeDefinition data);

        void Subscribe(IObserver<TypeDefinition> observer);

        void Unsubscribe(IObserver<TypeDefinition> observer);
    }
}
