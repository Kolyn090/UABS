using UABS.Assets.Script.Misc;

namespace UABS.Assets.Script.Event
{
    public class AssetDisplayInfoEvent : AppEvent
    {
        public AssetDisplayInfo Info { get; }

        public AssetDisplayInfoEvent(AssetDisplayInfo info)
        {
            Info = info;
        }
    }
}