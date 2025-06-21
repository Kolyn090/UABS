using System;
using UnityEngine;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System.Collections.Generic;
using System.IO;
using BCnEncoder.Decoder;
using BCnEncoder.Shared;
using CommunityToolkit.HighPerformance;
using System.Runtime.InteropServices;

namespace UABS.Assets.Script.Reader
{
    public class ReadTexturesFromBundle
    {
        public static List<Texture2D> ReadTextures(string bundlePath)
        {
            List<Texture2D> result = new();
            AssetsManager am = new();
            BundleFileInstance bunInst = am.LoadBundleFile(bundlePath, true);
            AssetsFileInstance fileInst = am.LoadAssetsFileFromBundle(bunInst, 0, false);

            List<AssetFileInfo> texInfos = fileInst.file.GetAssetsOfType(AssetClassID.Texture2D);

            foreach (AssetFileInfo texInfo in texInfos)
            {
                AssetTypeValueField texBase = am.GetBaseField(fileInst, texInfo);
                int width = texBase["m_Width"].AsInt;
                int height = texBase["m_Height"].AsInt;
                int format = texBase["m_TextureFormat"].AsInt;
                byte[] imageBytes = GetImageData(texBase, fileInst, bunInst);

                if (IsSupportedBCnFormat((TextureFormat)format, out CompressionFormat bcnFormat))
                {
                    var decoder = new BcDecoder();
                    ColorRgba32[] decoded = decoder.DecodeRaw(imageBytes, width, height, CompressionFormat.Bc7);

                    byte[] rgbaBytes = new byte[decoded.Length * 4];
                    MemoryMarshal.Cast<ColorRgba32, byte>(decoded.AsSpan()).CopyTo(rgbaBytes);

                    Texture2D texture = new(width, height, TextureFormat.RGBA32, false);
                    texture.LoadRawTextureData(rgbaBytes);
                    texture.Apply();

                    // Now you can assign tex to a material or use it however you need
                    // Debug.Log($"Decoded BC7 texture: {width}x{height}");
                    result.Add(PadToSquare(texture));
                }
                else if (imageBytes.Length == width * height * format)
                {
                    TextureFormat unityFormat = TextureFormat.RGBA32;
                    Texture2D texture = new(width, height, unityFormat, false);
                    texture.LoadRawTextureData(imageBytes);
                    texture.Apply();
                    result.Add(PadToSquare(texture));
                }
                else
                {
                    Debug.LogError($"Expected {width * height * format} bytes, got {imageBytes.Length}");
                }
            }

            return result;
        }

        private static byte[] GetImageData(AssetTypeValueField texField, AssetsFileInstance fileInst, BundleFileInstance bunInst)
        {
            var imageDataField = texField["image data"];
            byte[] rawData = imageDataField?.Value?.AsByteArray ?? Array.Empty<byte>();

            var streamData = texField["m_StreamData"];
            uint offset = streamData["offset"].AsUInt;
            uint size = streamData["size"].AsUInt;
            string path = streamData["path"].AsString;

            if (!string.IsNullOrEmpty(path) && size > 0)
            {
                if (path.StartsWith("archive:/"))
                {
                    // Extract internal stream file from bundle
                    string internalFileName = Path.GetFileName(path); // e.g. "CAB-c8b157fca857626dbba75589e140a72a.resS"
                    string bundleFilePath = fileInst.path; // path to the .bundle file

                    byte[] internalFileData = ExtractFileManually(bunInst, internalFileName);

                    // Read the stream segment from the extracted bytes
                    byte[] buffer = new byte[size];
                    Array.Copy(internalFileData, offset, buffer, 0, size);
                    return buffer;
                }
                else
                {
                    // External file on disk (normal case)
                    string baseDir = Path.GetDirectoryName(fileInst.path);
                    string fullPath = Path.Combine(baseDir, path);

                    if (!File.Exists(fullPath))
                        throw new FileNotFoundException("Stream data file not found", fullPath);

                    byte[] buffer = new byte[size];
                    using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                    {
                        fs.Seek(offset, SeekOrigin.Begin);

                        int totalRead = 0;
                        while (totalRead < size)
                        {
                            int read = fs.Read(buffer, totalRead, (int)(size - totalRead));
                            if (read == 0)
                                throw new IOException("Unexpected end of stream while reading external data.");
                            totalRead += read;
                        }
                    }

                    return buffer;
                }
            }

            // No external stream, return rawData
            return rawData;
        }

        private static byte[] ExtractFileManually(BundleFileInstance bundle, string internalFileName)
        {
            var dirInfos = bundle.file.BlockAndDirInfo.DirectoryInfos;

            foreach (var dir in dirInfos)
            {
                if (dir.Name.Equals(internalFileName, StringComparison.OrdinalIgnoreCase))
                {
                    long offset = dir.Offset; // or use dir.OffsetInBundle if that’s the actual name
                    long size = dir.DecompressedSize; // or dir.Size or similar

                    // Make sure the stream is at the beginning of decompressed data
                    var stream = bundle.DataStream;
                    stream.Seek(offset, SeekOrigin.Begin);

                    byte[] buffer = new byte[size];
                    int totalRead = 0;

                    while (totalRead < size)
                    {
                        int read = stream.Read(buffer, totalRead, (int)(size - totalRead));
                        if (read == 0)
                            throw new IOException("Unexpected end of stream while reading internal bundle file.");
                        totalRead += read;
                    }

                    return buffer;
                }
            }

            throw new FileNotFoundException($"File '{internalFileName}' not found in bundle '{bundle.path}'");
        }

        public static Texture2D DecodeWithBCn(byte[] bc7Data, int width, int height)
        {
            var decoder = new BcDecoder();

            // Decode BC7 raw bytes to ColorRgba32[] array
            ColorRgba32[] decodedPixels = decoder.DecodeRaw(bc7Data, width, height, CompressionFormat.Bc7);

            // Convert ColorRgba32[] to byte[] for Texture2D
            byte[] rawBytes = ColorRgba32ArrayToByteArray(decodedPixels);

            // Create Texture2D with RGBA32 format and load raw pixel data
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
            tex.LoadRawTextureData(rawBytes);
            tex.Apply();

            return tex;
        }

        // Helper method to convert ColorRgba32[] to byte[]
        private static byte[] ColorRgba32ArrayToByteArray(ColorRgba32[] pixels)
        {
            int length = pixels.Length * 4; // 4 bytes per pixel
            byte[] bytes = new byte[length];

            // Use MemoryMarshal to reinterpret the ColorRgba32 array as bytes
            Span<ColorRgba32> pixelSpan = pixels.AsSpan();
            MemoryMarshal.Cast<ColorRgba32, byte>(pixelSpan).CopyTo(bytes);

            return bytes;
        }

        private static bool IsSupportedBCnFormat(TextureFormat unityFormat, out CompressionFormat format)
        {
            switch (unityFormat)
            {
                case TextureFormat.DXT1:
                    format = CompressionFormat.Bc1;
                    return true;
                case TextureFormat.DXT5:
                    format = CompressionFormat.Bc3;
                    return true;
                case TextureFormat.BC6H:
                    format = CompressionFormat.Bc6U; // assumes unsigned HDR, can adjust if needed
                    return true;
                case TextureFormat.BC7:
                    format = CompressionFormat.Bc7;
                    return true;
                default:
                    format = default;
                    return false;
            }
        }

        private static Texture2D PadToSquare(Texture2D original)
        {
            int size = Mathf.Max(original.width, original.height); // square dimension
            Texture2D square = new(size, size, TextureFormat.RGBA32, false);

            // Fill background with transparent or black
            Color32[] fill = new Color32[size * size];
            square.SetPixels32(fill);

            // Center original image
            int xOffset = (size - original.width) / 2;
            int yOffset = (size - original.height) / 2;
            square.SetPixels(xOffset, yOffset, original.width, original.height, original.GetPixels());

            square.Apply();
            return square;
        }
    }
}