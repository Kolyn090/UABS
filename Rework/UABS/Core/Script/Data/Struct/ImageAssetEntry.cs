using UABS.Wrapper;

namespace UABS.Data
{
    public sealed class ImageAssetEntry : AssetEntry
    {
        private IImageResource? _image;
        public IImageResource? Image {
            get
            {
                return _image;
            }
            set
            {
                if (value is {} image)
                    _image = CropImageResource.Crop(image, ImageRect);
            }
        }
        public ImageRect ImageRect { get; set; }

        public ImageAssetEntry()
        {
            PreviewType = AssetPreviewType.Image2D;
        }

        public static ImageAssetEntry ConvertToImageAssetEntry(AssetEntry baseObj)
        {
            ImageAssetEntry result = ConvertToDerived<ImageAssetEntry>(baseObj);
            result.PreviewType = AssetPreviewType.Image2D;
            return result;
        }
    }
}