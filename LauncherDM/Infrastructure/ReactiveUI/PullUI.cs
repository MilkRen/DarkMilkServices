using System.Collections.Generic;
using System.Windows.Documents;
using LauncherDM.Infrastructure.ReactiveUI.Base;

namespace LauncherDM.Infrastructure.ReactiveUI
{
    public class PullUI : IObservable<LoadUI>
    {
        private readonly List<IObserver<LoadUI>> _observers;

        public PullUI()
        {
            _observers = new List<IObserver<LoadUI>>();
        }

        public void Notify(LoadUI data)
        {
            foreach (var subs in _observers.ToArray())
                subs.Update(data);
        }

        public void Subscribe(IObserver<LoadUI> observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver<LoadUI> observer)
        {
            _observers.Remove(observer);
        }

        public void UnsubscribeAll()
        {
            _observers.Clear();
        }
    }
}
