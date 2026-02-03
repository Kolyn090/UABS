using UABS.Wrapper;
using Avalonia.Platform;
using Avalonia.Media.Imaging;
using System;
using Avalonia;
using UABS.Util;

namespace UABS.AvaloniaUI
{
    public class AvaloniaImageResource : IImageResource
    {
        public Bitmap Bitmap { get; }

        public AvaloniaImageResource(byte[] rawImageBytes)
        {
            if (rawImageBytes == null || rawImageBytes.Length == 0)
            {
                Log.Error($"ArgumentException: Pixel data cannot be null or empty: {nameof(rawImageBytes)}");
                throw new ArgumentException("Pixel data cannot be null or empty", nameof(rawImageBytes));
            }

            var writeableBitmap = new WriteableBitmap(
                new PixelSize(Width, Height),
                new Vector(96, 96),
                ToAvaloniaPixelFormat.Convert(UnityTextureFormat)
            );

            using (var fb = writeableBitmap.Lock())
            {
                System.Runtime.InteropServices.Marshal.Copy(rawImageBytes, 0, fb.Address, rawImageBytes.Length);
            }

            Bitmap = writeableBitmap;
            RawImageBytes = rawImageBytes;
        }

        public int Width => Bitmap.PixelSize.Width;
        public int Height => Bitmap.PixelSize.Height;

        public UnityTextureFormat UnityTextureFormat
        {
            get
            {
                if (Bitmap == null)
                    return UnityTextureFormat.RGBA32;

                if (Bitmap.Format.Equals(PixelFormat.Rgba8888))
                    return UnityTextureFormat.RGBA32;

                if (Bitmap.Format.Equals(PixelFormat.Bgra8888))
                    return UnityTextureFormat.BGRA32;

                return UnityTextureFormat.RGBA32;
            }
        }

        public byte[] RawImageBytes { get; }

        public static AvaloniaImageResource FromCommonImageResource(CommonImageResource commonImageResource)
        {
            return new AvaloniaImageResource(commonImageResource.RawImageBytes);
        }
    }
}