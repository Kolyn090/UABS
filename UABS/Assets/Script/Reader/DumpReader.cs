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

        private AssetsManager AssetsManager { get; }

        public DumpReader(AssetsManager am)
        {
            AssetsManager = am;
        }

        public List<DumpInfo> ReadSpriteAtlasDumps(BundleFileInstance bunInst)
        {
            return ReadDumps(bunInst, AssetClassID.SpriteAtlas);
        }

        public List<DumpInfo> ReadSpriteDumps(BundleFileInstance bunInst)
        {
            return ReadDumps(bunInst, AssetClassID.Sprite);
        }

        public List<DumpInfo> ReadTexture2DDumps(BundleFileInstance bunInst)
        {
            return ReadDumps(bunInst, AssetClassID.Texture2D);
        }

        private List<DumpInfo> ReadDumps(BundleFileInstance bunInst, AssetClassID assetType)
        {
            List<DumpInfo> result = new();
            AssetsFileInstance fileInst = AssetsManager.LoadAssetsFileFromBundle(bunInst, 0, false);

            List<AssetFileInfo> assetInfos = fileInst.file.GetAssetsOfType(assetType);
            if (assetInfos.Count == 0)
                return result;

            foreach (var assetInfo in assetInfos)
            {
                AssetTypeValueField assetBase = AssetsManager.GetBaseField(fileInst, assetInfo);
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