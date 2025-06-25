using System.Collections.Generic;
using System.IO;
using AssetsTools.NET.Extra;
using UABS.Assets.Script.Misc;
using UABS.Assets.Script.Reader;
using UnityEngine;

namespace UABS.Assets.Script.Writer
{
    public class WriteCache
    {
        private SfbManager _sfbManager = new();
        private ReadNewCache _readNewCache;

        public WriteCache(AssetsManager assetsManager)
        {
            _readNewCache = new(assetsManager);
        }

        public void CreateAndSaveNewCache(string dataPath, string savePath)
        {
            if (string.IsNullOrWhiteSpace(dataPath))
            {
                Debug.LogWarning("Failed to read Game Data Folder. Abort.");
                return;
            }

            if (string.IsNullOrWhiteSpace(savePath))
            {
                Debug.LogWarning("Failed to read New Save Folder. Abort.");
                return;
            }

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            List<CacheInfo> cache = _readNewCache.BuildCache(dataPath, savePath);
            foreach (CacheInfo cacheInfo in cache)
            {
                string path = cacheInfo.path;
                string content = cacheInfo.jsonContent;
                string dir = Path.GetDirectoryName(path);
                if (!Directory.Exists(dir))
                {
                    Debug.Log($"Creating path {dir}");
                    Directory.CreateDirectory(dir);
                }

                File.WriteAllText(path, content);
            }
        }
    }
}