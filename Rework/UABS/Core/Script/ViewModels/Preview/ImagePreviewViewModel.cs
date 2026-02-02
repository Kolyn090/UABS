using UABS.Data;
using UABS.Util;
using UABS.Wrapper;

namespace UABS.ViewModel
{
    public sealed class ImagePreviewViewModel : AssetPreviewViewModel
    {
        public IImageResource? ImageResource { get; }

        public ImagePreviewViewModel(AssetEntry assetEntry)
            : base(AssetPreviewType.Image2D)
        {
            if (assetEntry is ImageAssetEntry imageAssetEntry)
            {
                ImageResource = imageAssetEntry.Image;
            }
            else
            {
                Log.Error("Asset entry is not of type Image. Abort.");
            }
        }
    }
}