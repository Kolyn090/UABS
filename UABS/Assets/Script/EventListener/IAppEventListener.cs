using UABS.Assets.Script.Event;

namespace UABS.Assets.Script.EventListener
{
    public interface IAppEventListener
    {
        void OnEvent(AppEvent e);
    }
}