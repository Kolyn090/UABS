using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UABS.Assets.Script.Misc;

namespace UABS.Assets.Script.Reader
{
    public class ReadDisplayInfoFromBundle
    {
        private AssetsManager AssetsManager { get; }

        public ReadDisplayInfoFromBundle(AssetsManager am)
        {
            AssetsManager = am;
        }

        public List<AssetDisplayInfo> ReadAllBasicInfo(BundleFileInstance bunInst)
        {
            List<AssetDisplayInfo> result = new();
            result.AddRange(ReadSpritesBasicInfo(bunInst));
            result.AddRange(ReadTexture2DBasicInfo(bunInst));
            return result;
        }

        public List<AssetDisplayInfo> ReadSpritesBasicInfo(BundleFileInstance bunInst)
        {
            AssetsFileInstance fileInst = AssetsManager.LoadAssetsFileFromBundle(bunInst, 0, false);
            return ReadBasicInfoOf(AssetClassID.Sprite, fileInst, AssetsManager);
        }

        public List<AssetDisplayInfo> ReadTexture2DBasicInfo(BundleFileInstance bunInst)
        {
            AssetsFileInstance fileInst = AssetsManager.LoadAssetsFileFromBundle(bunInst, 0, false);
            return ReadBasicInfoOf(AssetClassID.Texture2D, fileInst, AssetsManager);
        }

        private static List<AssetDisplayInfo> ReadBasicInfoOf(AssetClassID assetType,
                                                                AssetsFileInstance fileInst,
                                                                AssetsManager am)
        {
            int StableStringHash(string input)
            {
                using var sha = SHA256.Create();
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToInt32(hash, 0); // deterministic but may collide
            }

            int GetAssetDataSize(AssetsManager am, AssetsFileInstance fileInst, AssetFileInfo assetInfo)
            {
                if (assetType == AssetClassID.Texture2D)
                {
                    try
                    {
                        AssetTypeValueField baseField = am.GetBaseField(fileInst, assetInfo);
                        if (baseField == null) return (int)assetInfo.ByteSize;

                        // Texture2D specific data
                        int width = baseField["m_Width"].AsInt;
                        int height = baseField["m_Height"].AsInt;
                        int format = baseField["m_TextureFormat"].AsInt;

                        var streamData = baseField["m_StreamData"];
                        uint streamSize = streamData["size"].AsUInt;
                        string streamPath = streamData["path"].AsString;

                        if (streamSize > 0 && !string.IsNullOrEmpty(streamPath))
                        {
                            // Data is stored in external .resS file
                            return (int)streamSize;
                        }

                        // Inline image data
                        byte[] imageBytes = baseField["image data"].AsByteArray;
                        return imageBytes?.Length ?? 0;
                    }
                    catch
                    {
                        return (int)assetInfo.ByteSize;
                    }
                }
                else
                {
                    // Fallback: use serialized byte size
                    return (int)assetInfo.ByteSize;
                }
            }

            List<AssetDisplayInfo> result = new();
            List<AssetFileInfo> assets = fileInst.file.GetAssetsOfType(assetType);
            foreach (AssetFileInfo asset in assets)
            {
                long pathId = asset.PathId;
                int fileId = fileInst.parentBundle != null
                                ? StableStringHash(fileInst.parentBundle.name)
                                : 0; // or any way to uniquely ID the file
                string filePath = fileInst.path;
                long size = GetAssetDataSize(am, fileInst, asset);

                // To get the asset name:
                AssetTypeValueField baseField = am.GetBaseField(fileInst, asset);
                string name = baseField["m_Name"].AsString;
                result.Add(new()
                {
                    name = name,
                    pathID = pathId,
                    fileID = fileId,
                    path = filePath,
                    size = size,
                    type = assetType
                });
            }

            return result;
        }
    }
}
