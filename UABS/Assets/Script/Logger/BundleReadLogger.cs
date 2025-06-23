using UABS.Assets.Script.Event;
using UABS.Assets.Script.EventListener;
using UnityEngine;

namespace UABS.Assets.Script.Logger
{
    public class BundleReadLogger : IAppEventListener
    {
        public void OnEvent(AppEvent e)
        {
            if (e is BundleReadEvent bre)
            {
                Debug.Log($"[BundleReadLogger] Bundle '{bre.FilePath}' loaded.");
            }
        }
    }
}