using System.Collections.Generic;
using UABS.Assets.Script.Event;
using UABS.Assets.Script.EventListener;

namespace UABS.Assets.Script.Dispatcher
{
    public class EventDispatcher
    {
        private readonly List<IAppEventListener> _listeners = new();
        public void Register(IAppEventListener listener) => _listeners.Add(listener);

        public void Dispatch(AppEvent e)
        {
            foreach (var listener in _listeners)
            {
                listener.OnEvent(e);
            }
        }
    }
}