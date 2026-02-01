namespace UABS.Wrapper
{
    public class CommonImageResource : IImageResource
    {
        public int Width { get; }

        public int Height { get; }

        public ImagePixelFormat ImagePixelFormat { get; }

        public byte[] RawImageBytes { get; }

        public CommonImageResource(int width,
                                    int height,
                                    ImagePixelFormat imagePixelFormat,
                                    byte[] rawPixelData)
        {
            Width = width;
            Height = height;
            ImagePixelFormat = imagePixelFormat;
            RawImageBytes = rawPixelData;
        }
    }
}