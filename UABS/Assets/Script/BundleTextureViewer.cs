using System;
using UnityEngine;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using System.Linq;


public class BundleTextureViewer : MonoBehaviour
{
    private const string TestBundlePath = @"\\?\C:\Program Files (x86)\Steam\steamapps\common\Otherworld Legends\Otherworld Legends_Data\StreamingAssets\aa\StandaloneWindows64\graphiceffecttextureseparatelygroup_assets_assets\sprites\uniteffect_0.spriteatlas_66b2db9fb94b5bda5b7794c6ed82cf3f.bundle";
    public RawImage rawImage; // Drag a UI RawImage from the scene into the Inspector

    private void Start()
    {
        Texture2D tex = ReadTexture2D(TestBundlePath);
        rawImage.texture = tex;
        rawImage.SetNativeSize();
    }

    private Texture2D ReadTexture2D(string bundlePath)
    {
        AssetsManager am = new();
        BundleFileInstance bunInst = am.LoadBundleFile(bundlePath, true);
        AssetsFileInstance fileInst = am.LoadAssetsFileFromBundle(bunInst, 0, false);

        List<AssetFileInfo> textures = fileInst.file.GetAssetsOfType(AssetClassID.Texture2D);

        // Let's assume for now there is exactly one texture
        AssetFileInfo texInfo = textures[0];

        AssetTypeValueField texBase = am.GetBaseField(fileInst, texInfo);
        int width = texBase["m_Width"].AsInt;
        int height = texBase["m_Height"].AsInt;
        int format = texBase["m_TextureFormat"].AsInt;
        byte[] imageBytes = GetImageData(texBase, fileInst, bunInst);

        TextureFormat unityFormat = TextureFormat.RGBA32;
        Texture2D texture = new(width, height, unityFormat, false);
        texture.LoadRawTextureData(imageBytes);
        texture.Apply();

        return texture;
    }

    byte[] GetImageData(AssetTypeValueField texField, AssetsFileInstance fileInst, BundleFileInstance bunInst)
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

    byte[] ExtractFileManually(BundleFileInstance bundle, string internalFileName)
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
}
