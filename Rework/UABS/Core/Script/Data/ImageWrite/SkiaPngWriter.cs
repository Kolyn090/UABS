using SkiaSharp;
using System;
using System.IO;

namespace UABS.Data
{
    // TODO: Make wrapper
    public static class SkiaPngWriter
    {
        public static void SaveRgbaAsPng(
            byte[] rgba,
            int width,
            int height,
            string outputPath,
            bool flipVertically = true)
        {
            if (rgba.Length != width * height * 4)
                throw new ArgumentException("Invalid RGBA buffer size");

            using var bitmap = new SKBitmap(
                new SKImageInfo(width, height, SKColorType.Rgba8888, SKAlphaType.Unpremul)
            );

            // Copy pixels row by row (and flip if needed)
            int srcStride = width * 4;
            IntPtr dst = bitmap.GetPixels();

            for (int y = 0; y < height; y++)
            {
                int srcY = flipVertically ? (height - 1 - y) : y;
                IntPtr dstRow = dst + y * srcStride;

                System.Runtime.InteropServices.Marshal.Copy(
                    rgba,
                    srcY * srcStride,
                    dstRow,
                    srcStride
                );
            }

            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);

            File.WriteAllBytes(outputPath, data.ToArray());
        }
    }
}