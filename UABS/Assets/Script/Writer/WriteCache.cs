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

        public void CreateAndSaveNewCache()
        {
            static bool IsDirectoryPath(string path)
            {
                // Ends with slash, or has no file extension
                return path.EndsWith("/") || path.EndsWith("\\") || string.IsNullOrEmpty(Path.GetExtension(path));
            }
            string gameDataPath = _sfbManager.PickFolder("Select the game data folder (such as StandaloneWindows64)");
            string newSavePath = _sfbManager.PickFolder("Select Folder to Save New Cache", PredefinedPaths.ExternalCache);
            List<CacheInfo> cache = _readNewCache.BuildCache(gameDataPath, newSavePath);
            foreach (CacheInfo cacheInfo in cache)
            {
                string path = cacheInfo.path;
                string content = cacheInfo.jsonContent;
                if (IsDirectoryPath(path) && !Directory.Exists(path))
                {
                    Debug.Log($"Creating path {path}");
                    Directory.CreateDirectory(path);
                }

                File.WriteAllText(Path.Combine(path, "index.json"), content);
            }
        }
    }
}