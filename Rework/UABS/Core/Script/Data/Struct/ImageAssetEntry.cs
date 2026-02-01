using UABS.Wrapper;

namespace UABS.Data
{
    public sealed class ImageAssetEntry : AssetEntry
    {
        public IImageResource? Image { get; set; }
        public ImageRect ImageRect { get; set; }

        public static ImageAssetEntry ConvertToImageAssetEntry(AssetEntry baseObj)
        {
            return ConvertToDerived<ImageAssetEntry>(baseObj);
        }
    }
}