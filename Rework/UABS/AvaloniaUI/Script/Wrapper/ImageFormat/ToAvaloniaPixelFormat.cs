using Avalonia.Platform;
using UABS.Wrapper;

namespace UABS.AvaloniaUI
{
    public static class ToAvaloniaPixelFormat
    {
        public static PixelFormat Convert(UnityTextureFormat format)
        {
            return format switch
            {
                UnityTextureFormat.BGRA32       => PixelFormat.Bgra8888,
                UnityTextureFormat.RGBA32       => PixelFormat.Rgba8888,
                _ => PixelFormat.Bgra8888      // default fallback
            };
        }
    }
}