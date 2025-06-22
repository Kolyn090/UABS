using UnityEngine;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System.Collections.Generic;
using UABS.Assets.Script.Misc;

namespace UABS.Assets.Script.Reader
{
    public class ReadBasicInfoFromBundle
    {
        public static List<AssetBasicInfo> ReadSpritesBasicInfo(string bundlePath)
        {
            List<AssetBasicInfo> result = new();

            AssetsManager am = new();
            BundleFileInstance bunInst = am.LoadBundleFile(bundlePath, true);
            AssetsFileInstance fileInst = am.LoadAssetsFileFromBundle(bunInst, 0, false);

            List<AssetFileInfo> assets = fileInst.file.GetAssetsOfType(AssetClassID.Sprite);

            foreach (AssetFileInfo asset in assets)
            {
                long pathId = asset.PathId;
                int fileId = fileInst.parentBundle?.name.GetHashCode() ?? 0; // or any way to uniquely ID the file
                string filePath = fileInst.path;
                int size = (int)asset.ByteSize;

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
                    type=AssetClassID.Sprite
                });
            }

            return result;
        }

        public static List<AssetBasicInfo> ReadTexture2DBasicInfo(string bundlePath)
        {
            List<AssetBasicInfo> result = new();
            
            AssetsManager am = new();
            BundleFileInstance bunInst = am.LoadBundleFile(bundlePath, true);
            AssetsFileInstance fileInst = am.LoadAssetsFileFromBundle(bunInst, 0, false);
            List<AssetFileInfo> assets = fileInst.file.GetAssetsOfType(AssetClassID.Texture2D);

            foreach (AssetFileInfo asset in assets)
            {
                long pathId = asset.PathId;
                int fileId = fileInst.parentBundle?.name.GetHashCode() ?? 0; // or any way to uniquely ID the file
                string filePath = fileInst.path;
                int size = (int)asset.ByteSize;

                // To get the asset name:
                AssetTypeValueField baseField = am.GetBaseField(fileInst, asset);
                string name = baseField["m_Name"].AsString;
                result.Add(new()
                {
                    name = name,
                    pathID = pathId,
                    fileID = fileId,
                    path = filePath,
                    size=size
                });
            }

            return result;
        }
    }
}
