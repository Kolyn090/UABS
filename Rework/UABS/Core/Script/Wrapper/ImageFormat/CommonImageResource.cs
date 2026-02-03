using System;
using UABS.Util;

namespace UABS.Wrapper
{
    public class CommonImageResource : IImageResource
    {
        public int Width { get; }

        public int Height { get; }

        public UnityTextureFormat UnityTextureFormat { get; }

        public byte[] RawImageBytes { get; }

        public CommonImageResource(int width,
                                    int height,
                                    UnityTextureFormat unityTextureFormat,
                                    byte[] rawPixelData)
        {
            Width = width;
            Height = height;
            UnityTextureFormat = unityTextureFormat;
            RawImageBytes = rawPixelData;
        }
    }
}