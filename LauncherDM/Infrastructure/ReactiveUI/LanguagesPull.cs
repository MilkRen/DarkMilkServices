using System.Collections.Generic;
using System.Windows.Documents;
using LauncherDM.Infrastructure.ReactiveUI.Base;

namespace LauncherDM.Infrastructure.ReactiveUI
{
    public class LanguagesPull : IObservable<LanguagesUpdate>
    {
        private readonly List<IObserver<LanguagesUpdate>> _observers;

        public LanguagesPull()
        {
            _observers = new List<IObserver<LanguagesUpdate>>();
        }

        public void Notify(LanguagesUpdate data)
        {
            foreach (var subs in _observers)
                subs.Update(data);
        }

        public void Subscribe(IObserver<LanguagesUpdate> observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver<LanguagesUpdate> observer)
        {
            _observers.Remove(observer);
        }

        public void UnsubscribeAll()
        {
            _observers.Clear();
        }
    }
}
