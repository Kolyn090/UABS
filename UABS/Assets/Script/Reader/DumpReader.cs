using AssetsTools.NET;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using AssetsTools.NET.Extra;

namespace UABS.Assets.Script.Reader
{
    public class DumpReader
    {
        public struct DumpInfo
        {
            public JObject dumpJson;
            public long pathID;
        }

        public static List<DumpInfo> ReadSpriteAtlasDumps(string bundlePath)
        {
            return ReadDumps(bundlePath, AssetClassID.SpriteAtlas);
        }

        public static List<DumpInfo> ReadSpriteDumps(string bundlePath)
        {
            return ReadDumps(bundlePath, AssetClassID.Sprite);
        }

        public static List<DumpInfo> ReadTexture2DDumps(string bundlePath)
        {
            return ReadDumps(bundlePath, AssetClassID.Texture2D);
        }

        private static List<DumpInfo> ReadDumps(string bundlePath, AssetClassID assetType)
        {
            List<DumpInfo> result = new();
            AssetsManager am = new();
            BundleFileInstance bunInst = am.LoadBundleFile(bundlePath, true);
            AssetsFileInstance fileInst = am.LoadAssetsFileFromBundle(bunInst, 0, false);

            List<AssetFileInfo> assetInfos = fileInst.file.GetAssetsOfType(assetType);
            if (assetInfos.Count == 0)
                return result;

            foreach (var assetInfo in assetInfos)
            {
                AssetTypeValueField assetBase = am.GetBaseField(fileInst, assetInfo);
                result.Add(new()
                {
                    dumpJson = (JObject)JsonDumper.RecurseJsonDump(assetBase, true),
                    pathID=assetInfo.PathId
                });
            }

            return result;
        }
    }
}