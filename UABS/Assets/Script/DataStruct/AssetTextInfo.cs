using AssetsTools.NET.Extra;

namespace UABS.Assets.Script.Misc
{
    public struct AssetTextInfo
    {
        public string name;
        public long pathID;
        public long fileID;
        public long uncompressedSize;
        public long compressedSize;
        public AssetClassID type;
        public string path;
    }
}