using Avalonia.Platform;
using UABS.Wrapper;

namespace UABS.AvaloniaUI
{
    public static class ToAvaloniaPixelFormat
    {
        public static PixelFormat Convert(ImagePixelFormat format)
        {
            return format switch
            {
                ImagePixelFormat.Unknown      => PixelFormat.Bgra8888,
                ImagePixelFormat.BGRA32       => PixelFormat.Bgra8888,
                ImagePixelFormat.RGBA32       => PixelFormat.Rgba8888,
                ImagePixelFormat.Grayscale8   => PixelFormats.Gray8,
                _ => PixelFormat.Bgra8888      // default fallback
            };
        }
    }
}