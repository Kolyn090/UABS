using AssetsTools.NET.Extra;

namespace UABS.Assets.Script.Event
{
    public class BundleReadEvent : AppEvent
    {
        public BundleFileInstance Bundle { get; }
        public string FilePath { get; }

        public BundleReadEvent(BundleFileInstance bundle, string filePath)
        {
            Bundle = bundle;
            FilePath = filePath;
        }
    }
}