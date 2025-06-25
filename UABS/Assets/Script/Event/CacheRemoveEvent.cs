namespace UABS.Assets.Script.Event
{
    public class CacheRemoveEvent : AppEvent
    {
        public string RemovedPath;

        public CacheRemoveEvent(string removedPath)
        {
            RemovedPath = removedPath;
        }
    }
}