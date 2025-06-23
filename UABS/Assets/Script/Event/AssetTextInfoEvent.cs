using UABS.Assets.Script.Misc;

namespace UABS.Assets.Script.Event
{
    public class AssetTextInfoEvent : AppEvent
    {
        public AssetTextInfo Info { get; }

        public AssetTextInfoEvent(AssetTextInfo info)
        {
            Info = info;
        }
    }
}