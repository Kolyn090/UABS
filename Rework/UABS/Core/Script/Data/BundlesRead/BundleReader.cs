using System.Collections.Generic;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using UABS.Util;
using UABS.Wrapper;

namespace UABS.Data
{
    public static class BundleReader
    {
        public static (List<AssetsFileInstance>?, List<AssetEntry>?) ReadFromPath(string originalPath, 
                                                                                    AssetsManager assetsManager,
                                                                                    ITextureDecoder textureDecoder)
        {
            ImageReader imageReader = new(assetsManager, textureDecoder);
            FileInstanceLike fileInst = NextInstance.LoadAnyFile(assetsManager, originalPath);
            if (fileInst.IsAssetsFileInstance)
            {
                AssetsFileInstance assetsInst = fileInst.AsAssetsFileInstance!;
                return (new(){assetsInst}, GetAssets(originalPath, assetsManager, assetsInst, imageReader));
            }
            else if (fileInst.IsBundleFileInstance)
            {
                BundleFileInstance bunInst = fileInst.AsBundleFileInstace!;
                List<AssetsFileInstance> assetsInsts = bunInst.loadedAssetsFiles;
                List<AssetEntry> result = new();
                foreach (AssetsFileInstance assetsInst in assetsInsts)
                {
                    result.AddRange(GetAssets(originalPath, assetsManager, assetsInst, imageReader));
                }
                return (assetsInsts, result);
            }
            return (null, null);
        }

        private static List<AssetEntry> GetAssets(string originalPath, 
                                                    AssetsManager assetsManager, 
                                                    AssetsFileInstance assetsInst,
                                                    ImageReader imageReader)
        {
            List<AssetEntry> result = new();
            NextInstance nextInstance = new(assetsManager, assetsInst);
            IList<AssetFileInfo> assetFileInfos = assetsInst.file.AssetInfos;
            foreach (var assetFileInfo in assetFileInfos)
            {
                (string assetName, AssetClassID classID) = nextInstance.GetDisplayNameFast(assetFileInfo);
                AssetEntry assetEntry = new()
                {
                    Name = assetName,
                    ClassIDService = new(classID),
                    PathID = assetFileInfo.PathId,
                    AssetFileInfo = assetFileInfo,
                    AssetsInst = assetsInst,
                    OriginalPath = originalPath
                };
                if (classID == AssetClassID.Sprite || classID == AssetClassID.Texture2D)
                {
                    if (ProcessImageAsset(assetEntry, classID, imageReader) is {} imageAssetEntry)
                    {
                        result.Add(imageAssetEntry);
                    }
                    else
                    {
                        Log.Warn("Failed to read Sprite or Texture2D from an asset of type Image.");
                    }
                }
                else  // Unidentified case
                {
                    result.Add(assetEntry);
                }
            }

            return result;
        }

        private static ImageAssetEntry? ProcessImageAsset(AssetEntry assetEntry, 
                                                        AssetClassID classID, 
                                                        ImageReader imageReader)
        {
            assetEntry = ImageAssetEntry.ConvertToImageAssetEntry(assetEntry);
            if (classID == AssetClassID.Sprite)
            {
                return imageReader.SpriteToImage(assetEntry);
            }
            else  // Texture2D
            {
                return imageReader.Texture2DToImage(assetEntry);
            }
        }
    }
}