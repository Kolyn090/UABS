using System;
using System.Collections.Generic;
using System.IO;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using UABS.Util;
using UABS.Wrapper;

namespace UABS.Data
{
    public class ImageReader
    {
        private readonly AssetsManager _assetsManager;
        private readonly ITextureDecoder _textureDecoder;

        private AssetsFileInstance? _currAssetsInst = null;
        private List<AtlasDumpProcessor>? _currAtlasDumpProcessors = null;
        private AssetClassID _lastReadType = AssetClassID.@void;
        private readonly DumpReader _dumpReader;
        private List<DumpInfo>? _currSpriteDumps = null;

        public ImageReader(AssetsManager assetsManager,
                            ITextureDecoder textureDecoder)
        {
            _assetsManager = assetsManager;
            _dumpReader = new(_assetsManager);
            _textureDecoder = textureDecoder;
            string astcDllPath = @"..\..\..\..\Libs\astc_decoder.dll";
            NativeLoader.Load(astcDllPath);
            Log.Info($"Loaded Astc decoder from {astcDllPath}");
        }

        public ImageAssetEntry? SpriteToImage(AssetEntry assetEntry)
        {
            if (assetEntry.AssetsInst is not {} assetsInst)
            {
                Log.Warn("Missing AssetsFileInstance in assetEntry, return null.");
                return null;
            }

            if (assetEntry.AssetFileInfo is not {} assetFileInfo)
            {
                Log.Warn("Missing AssetFileInfo in assetEntry, return null.");
                return null;
            }

            long pathID = assetEntry.PathID;

            AtlasDumpProcessor? GetAtlasDumpProcessorForSpriteDump(DumpInfo spriteDump)
            {
                if (_currAtlasDumpProcessors is not {} currAtlasDumpProcessors)
                {
                    Log.Warn("_currAtlasDumpProcessors is null, return null.");
                    return null;
                }

                foreach (AtlasDumpProcessor atlasDumpProcessor in currAtlasDumpProcessors)
                {
                    if (atlasDumpProcessor.spriteDumpInfos.Contains(spriteDump))
                    {
                        return atlasDumpProcessor;
                    }
                }

                Log.Warn("The AtlasDumpProcessor that contains _currAtlasDumpProcessors not found, return null.");
                return null;
            }

            int GetIndexInAssets()
            {
                if (_currSpriteDumps is not { } currSpriteDumps)
                {
                    Log.Warn("_currSpriteDumps is null, return -1.");
                    return -1;
                }

                for (int i = 0; i < currSpriteDumps.Count; i++)
                {
                    long infoPathID = currSpriteDumps[i].pathID;
                    if (infoPathID == pathID) return i;
                }

                Log.Warn($"Couldn't find currSpriteDump with pathID {pathID}, return -1.");
                return -1;
            }

            // 1. Check if the entry is really sprite
            if (assetEntry.ClassIDService.ClassID != AssetClassID.Sprite)
            {
                Log.Warn("assetEntry is not of type Sprite, return null.");
                return null;
            }

            // 2. Cache Sprite-Atlas connection
            if (_currAssetsInst != assetsInst || _lastReadType != AssetClassID.Sprite)
            {
                _currAssetsInst = assetsInst;
                List<DumpInfo> atlasDumps = _dumpReader.ReadSpriteAtlasDumps(assetsInst);
                _currSpriteDumps = _dumpReader.ReadSpriteDumps(assetsInst);
                _currAtlasDumpProcessors = AtlasDumpProcessor.DistributeProcessors(atlasDumps, _currSpriteDumps);
                _lastReadType = AssetClassID.Sprite;
            }

            int indexInAssets = GetIndexInAssets();
            if (indexInAssets == -1)
            {
                // Log.Warn($"The given path id {pathID} is not found in sprites.");  // * Silent Warning
                return null;
            }

            // 3. Actually getting the sprite based on whether Atlas is used.
            AssetFileInfo targetAsset = assetFileInfo;
            DumpInfo targetDump = _currSpriteDumps![indexInAssets];  // * Verified earlier
            AtlasDumpProcessor? _atlasDumpInfoForSprite = GetAtlasDumpProcessorForSpriteDump(targetDump);
            if (_atlasDumpInfoForSprite != null) // Has Atlas
            {
                AtlasDumpProcessor atlasDumpInfoForSprite = (AtlasDumpProcessor)_atlasDumpInfoForSprite;
                Dictionary<int, int> index2RenderDataKey = atlasDumpInfoForSprite.GetIndex2ActualRenderDataKeyIndex();
                Dictionary<long, int> pathID2Index = atlasDumpInfoForSprite.GetPathID2Index();
                AssetTypeValueField spriteBase = _assetsManager.GetBaseField(_currAssetsInst, targetAsset);
                AssetTypeValueField atlasRefField = spriteBase["m_SpriteAtlas"];
                if (GetExternalAsset(_assetsManager, _currAssetsInst, assetsInst.parentBundle, atlasRefField) is not {} atlasAsset)
                {
                    Log.Warn("Assumed has Atlas but couldn't find atlasAsset, return null.");
                    return null;
                }
                AssetTypeValueField atlasBase = _assetsManager.GetBaseField(atlasAsset.file, atlasAsset.info);
                AssetTypeValueField renderDataMap = atlasBase["m_RenderDataMap"];
                AssetTypeValueField dataArray = renderDataMap["Array"][index2RenderDataKey[pathID2Index[pathID]]]; // The true index in dict
                AssetTypeValueField firstEntry = dataArray["second"];
                AssetTypeValueField texturePtr = firstEntry["texture"];
                if (GetExternalAsset(_assetsManager, _currAssetsInst, assetsInst.parentBundle, texturePtr) is not {} texAsset)
                {
                    Log.Warn("texAsset not found, return null.");
                    return null;
                }
                AssetTypeValueField texBase = _assetsManager.GetBaseField(atlasAsset.file, texAsset.info);
                if (atlasDumpInfoForSprite.GetRectAtActualIndex(index2RenderDataKey[pathID2Index[pathID]]) is not {} spriteRect)
                {
                    Log.Warn("spriteRect not found, return null.");
                    return null;
                }

                return ExtractImage(texBase, assetsInst.parentBundle, spriteRect, assetEntry);
            }
            else // No Atlas
            {
                AssetTypeValueField spriteBase = _assetsManager.GetBaseField(_currAssetsInst, targetAsset);
                ImageRect spriteRect = new(
                    spriteBase["m_Rect"]["x"].AsFloat,
                    spriteBase["m_Rect"]["y"].AsFloat,
                    spriteBase["m_Rect"]["width"].AsFloat,
                    spriteBase["m_Rect"]["height"].AsFloat
                );

                if (spriteBase.Get("m_AtlasTags") is {} atlasTags)
                {
                    if (atlasTags["Array"].AsArray is {} arr && arr.size != 0)
                    {
                        Log.Warn("No SpriteAtlas file found in bundle but Sprite has atlas tag. Skip.");
                        return null;
                    }
                }

                AssetTypeValueField texRefField = spriteBase["m_RD"]["texture"];
                if (GetExternalAsset(_assetsManager, _currAssetsInst, assetsInst.parentBundle, texRefField) is not {} texAsset)
                {
                    Log.Warn("texAsset not found, return null.");
                    return null;
                }
                AssetTypeValueField texBase = _assetsManager.GetBaseField(texAsset.file, texAsset.info);

                return ExtractImage(texBase, assetsInst.parentBundle, spriteRect, assetEntry);
            }
        }

        public ImageAssetEntry? Texture2DToImage(AssetEntry assetEntry)
        {
            if (assetEntry.ClassIDService.ClassID != AssetClassID.Texture2D)
            {
                Log.Warn("assetEntry is not of type Texture2D, return null.");
                return null;
            }

            if (assetEntry.AssetsInst is not {} assetsInst)
            {
                Log.Warn("Missing AssetsFileInstance in assetEntry, return null.");
                return null;
            }

            if (assetEntry.AssetFileInfo is not {} assetFileInfo)
            {
                Log.Warn("Missing AssetFileInfo in assetEntry, return null.");
                return null;
            }

            if (_currAssetsInst != assetsInst || _lastReadType != AssetClassID.Texture2D)
            {
                _currAssetsInst = assetsInst;
                _lastReadType = AssetClassID.Texture2D;
            }

            AssetTypeValueField texBase = _assetsManager.GetBaseField(_currAssetsInst, assetFileInfo);
            return ExtractImage(texBase, assetsInst.parentBundle, null, assetEntry);
        }

        private ImageAssetEntry? ExtractImage(AssetTypeValueField texBase, 
                                                BundleFileInstance bunInst, 
                                                ImageRect? cropRect,
                                                AssetEntry assetEntry)
        {
            int width = texBase["m_Width"].AsInt;
            int height = texBase["m_Height"].AsInt;
            int format = texBase["m_TextureFormat"].AsInt;
            UnityTextureFormat unityTextureFormat = (UnityTextureFormat) format;

            if (_currAssetsInst is not {} currFileInst)
            {
                Log.Warn("_currFileInst is null, return null.");
                return null;
            }

            if (GetImageData(texBase, currFileInst, bunInst) is not {} imageBytes)
            {
                Log.Warn("Failed to get image data, return null.");
                return null;
            }

            ImageAssetEntry LoadImage(byte[] bytes)
            {
                ImageAssetEntry imageAssetEntry = ImageAssetEntry.ConvertToImageAssetEntry(assetEntry);
                imageAssetEntry.ImageRect = cropRect ?? new(0, 0, width, height);
                imageAssetEntry.Image = new CommonImageResource(width, height, unityTextureFormat, bytes);

                return imageAssetEntry;
            }

            if (unityTextureFormat.SupportedCompression() is {} compressionFormat)
            {
                byte[] bytes = _textureDecoder.DecodeToBytes(imageBytes, width, height, compressionFormat);
                return LoadImage(bytes);
            }
            else if (unityTextureFormat.IsAndroid())
            {
                // TODO: Add ETC/unity crunched decoder
            }
            else if (unityTextureFormat.IsAstc())
            {
                byte[] rgbaBytes = new byte[width * height * 4];
                try
                {
                    (int blockX, int blockY) = GetBlock((UnityTextureFormat)format);
                    if (blockX == -1)
                    {
                        Log.Error($"Unsupported ASTC format: {(UnityTextureFormat)format}");
                        return null;
                    }
                    bool result = AstcDecoderNative.DecodeASTC(imageBytes, imageBytes.Length, width, height, rgbaBytes, blockX, blockY);
                    if (result) // assuming 0 means success
                    {
                        Log.Info("ASTC decode succeeded and texture applied.");
                        return LoadImage(rgbaBytes);
                    }
                    else
                    {
                        Log.Error("ASTC decode failed with error code " + result);
                        return null;
                    }
                }
                catch
                {
                    Log.Error("Something went wrong with ASTC decoder dll. Make sure your build work.");
                    return null;
                }
            }
            else if (unityTextureFormat == UnityTextureFormat.Alpha8)
            {
                int pixels = width * height;
                byte[] rgbaBytes = new byte[pixels * 4];
                for (int i = 0; i < pixels; i++)
                {
                    byte alpha = imageBytes[i];
                    rgbaBytes[i * 4 + 0] = 0; // R
                    rgbaBytes[i * 4 + 1] = 0; // G
                    rgbaBytes[i * 4 + 2] = 0; // B
                    rgbaBytes[i * 4 + 3] = alpha; // A
                }
                return LoadImage(rgbaBytes);
            }
            else
            {
                Log.Warn($"Expected {width * height * format} bytes, got {imageBytes.Length}, return null.");
                return null;
            }

            return null;
        }

        private (int, int) GetBlock(UnityTextureFormat astcFormat)
        {
            return astcFormat switch
            {
                UnityTextureFormat.ASTC_4x4 or UnityTextureFormat.ASTC_HDR_4x4 => (4, 4),
                UnityTextureFormat.ASTC_5x5 or UnityTextureFormat.ASTC_HDR_5x5 => (5, 5),
                UnityTextureFormat.ASTC_6x6 or UnityTextureFormat.ASTC_HDR_6x6 => (6, 6),
                UnityTextureFormat.ASTC_8x8 or UnityTextureFormat.ASTC_HDR_8x8 => (8, 8),
                UnityTextureFormat.ASTC_10x10 or UnityTextureFormat.ASTC_HDR_10x10 => (10, 10),
                UnityTextureFormat.ASTC_12x12 or UnityTextureFormat.ASTC_HDR_12x12 => (12, 12),
                _ => (-1, -1),
            };
        }

        private static byte[]? GetImageData(AssetTypeValueField texField, 
                                            AssetsFileInstance fileInst, 
                                            BundleFileInstance bunInst)
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

                    if (ExtractFileManually(bunInst, internalFileName) is not {} internalFileData)
                    {
                        Log.Warn($"Extracting internal stream from bundle failed, return null;");
                        return null;
                    }

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
                    {
                        Log.Warn($"Stream data file not found: {fullPath}, return null;");
                        return null;
                    }

                    byte[] buffer = new byte[size];
                    using (FileStream fs = new(fullPath, FileMode.Open, FileAccess.Read))
                    {
                        fs.Seek(offset, SeekOrigin.Begin);

                        int totalRead = 0;
                        while (totalRead < size)
                        {
                            int read = fs.Read(buffer, totalRead, (int)(size - totalRead));
                            if (read == 0)
                            {
                                Log.Error("IOException: Unexpected end of stream while reading internal bundle file.");
                                throw new IOException("Unexpected end of stream while reading internal bundle file.");
                            }
                            totalRead += read;
                        }
                    }

                    return buffer;
                }
            }

            // No external stream, return rawData
            return rawData;
        }

        private static byte[]? ExtractFileManually(BundleFileInstance bundle, string internalFileName)
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
                        {
                            Log.Error("IOException: Unexpected end of stream while reading internal bundle file.");
                            throw new IOException("Unexpected end of stream while reading internal bundle file.");
                        }
                        totalRead += read;
                    }

                    return buffer;
                }
            }

            Log.Warn($"File '{internalFileName}' not found in bundle '{bundle.path}', return null.");
            return null;
        }

        private static AssetExternal? GetExternalAsset(AssetsManager assetsManager,
                                                        AssetsFileInstance currentFile,
                                                        BundleFileInstance bundleFile,
                                                        AssetTypeValueField pptr)
        {
            int fileId = pptr["m_FileID"].AsInt;
            long pathId = pptr["m_PathID"].AsLong;

            AssetsFileInstance targetFile = (fileId == 0)
                ? currentFile
                : assetsManager.LoadAssetsFileFromBundle(bundleFile, fileId, false);  // use fileId as index

            AssetFileInfo targetInfo = targetFile.file.GetAssetInfo(pathId);
            if (targetFile == null)
            {
                Log.Warn("targetFile is null. Failed to resolve fileId, return null.");
                return null;
            }

            if (targetInfo == null)
            {
                Log.Warn($"Asset with pathId {pathId} not found in file, return null.");
                return null;
            }

            var baseField = assetsManager.GetBaseField(targetFile, targetInfo);
            if (baseField == null)
            {
                Log.Warn($"GetBaseField returned null., return null.");
                return null;
            }

            return new AssetExternal
            {
                file = targetFile,
                info = targetInfo,
                baseField = assetsManager.GetBaseField(targetFile, targetInfo)
            };
        }
    }
}