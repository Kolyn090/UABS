namespace UABS.Assets.Script.Event
{
    public class CacheCreateEvent : AppEvent
    {
        public string NewCachePath;

        public CacheCreateEvent(string newCachePath)
        {
            NewCachePath = newCachePath;
        }
    }
}