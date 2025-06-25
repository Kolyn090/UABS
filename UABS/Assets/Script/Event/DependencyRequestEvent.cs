namespace UABS.Assets.Script.Event
{
    public class DependencyRequestEvent : AppEvent
    {
        public string ReadFromCachePath { get; }

        public DependencyRequestEvent(string readFromCachePath)
        {
            ReadFromCachePath = readFromCachePath;
        }
    }
}