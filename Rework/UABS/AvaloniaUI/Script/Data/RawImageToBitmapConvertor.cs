using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using UABS.Util;
using UABS.Wrapper;

namespace UABS.AvaloniaUI
{
    public sealed class RawImageToBitmapConverter : IValueConverter
    {
        public static readonly RawImageToBitmapConverter Instance = new();

        public object? Convert(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            if (value is not IImageResource img)
            {
                Log.Error("Value is not IImageResource, return null");
                return null;
            }

            if (img.Width <= 0 || img.Height <= 0 || img.RawImageBytes == null)
            {
                Log.Error("Image has invalid size or has no raw image bytes, return null.");
                return null;
            }

            return CreateBitmap(
                    PixelFormat.Rgba8888,
                    img.Width,
                    img.Height,
                    FlipVertically(img.RawImageBytes, img.Width, img.Height, 4),
                    img.Width * 4);
        }

        public object ConvertBack(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
            => throw new NotSupportedException();

        private static Bitmap CreateBitmap(
            PixelFormat format,
            int width,
            int height,
            byte[] pixels,
            int stride)
        {
            var size = pixels.Length;
            var ptr = Marshal.AllocHGlobal(size);

            try
            {
                Marshal.Copy(pixels, 0, ptr, size);

                return new Bitmap(
                    format,
                    AlphaFormat.Unpremul,
                    ptr,
                    new PixelSize(width, height),
                    new Vector(96, 96),
                    stride);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        private static Bitmap CreateFromGray8(IImageResource img)
        {
            int width = img.Width;
            int height = img.Height;
            var flipped = FlipVertically(img.RawImageBytes, width, height, 1);

            var rgba = new byte[width * height * 4];

            for (int i = 0, j = 0; i < flipped.Length; i++, j += 4)
            {
                byte g = flipped[i];
                rgba[j + 0] = g;
                rgba[j + 1] = g;
                rgba[j + 2] = g;
                rgba[j + 3] = 255;
            }

            return CreateBitmap(PixelFormat.Rgba8888, width, height, rgba, width * 4);
        }

        private static byte[] FlipVertically(byte[] src, int width, int height, int bpp)
        {
            int stride = width * bpp;
            var dst = new byte[src.Length];

            for (int y = 0; y < height; y++)
            {
                int srcOffset = (height - 1 - y) * stride;
                int dstOffset = y * stride;

                Buffer.BlockCopy(src, srcOffset, dst, dstOffset, stride);
            }

            return dst;
        }
    }
}