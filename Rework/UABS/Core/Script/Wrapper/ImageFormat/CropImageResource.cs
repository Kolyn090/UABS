using System;
using UABS.Util;

namespace UABS.Wrapper
{
    public static class CropImageResource
    {
        public static IImageResource Crop(IImageResource source, ImageRect rect)
        {
            // Validate rect
            if (rect.X < 0 || rect.Y < 0 ||
                rect.X + rect.Width > source.Width ||
                rect.Y + rect.Height > source.Height)
            {
                Log.Error("ArgumentOutOfRangeException: Crop rect out of bounds.");
                throw new ArgumentOutOfRangeException(nameof(rect), "Crop rect out of bounds");
            }

            int bpp = 4; // Always render in RGBA32
            checked
            {
                int expectedSize = source.Width * source.Height * bpp;
                if (source.RawImageBytes.Length < expectedSize)
                {
                    Log.Error($"InvalidOperationException: RawImageBytes too small. Expected at least {expectedSize}, actual {source.RawImageBytes.Length}");
                    throw new InvalidOperationException(
                        $"RawImageBytes too small. Expected at least {expectedSize}, " +
                        $"actual {source.RawImageBytes.Length}");
                }
            }

            // int srcStride = source.Width * bpp;
            int dstStride = (int)(rect.Width * bpp);

            var dstPixels = new byte[(int)rect.Height * dstStride];
            var srcPixels = source.RawImageBytes;

            for (int row = 0; row < rect.Height; row++)
            {
                int srcOffset =
                    (((int)rect.Y + row) * source.Width + (int)rect.X) * bpp;

                int dstOffset =
                    row * dstStride;

                Buffer.BlockCopy(
                    srcPixels,
                    srcOffset,
                    dstPixels,
                    dstOffset,
                    dstStride);
            }

            return new CommonImageResource(
                (int)rect.Width,
                (int)rect.Height,
                source.UnityTextureFormat,
                dstPixels);
        }
    }
}