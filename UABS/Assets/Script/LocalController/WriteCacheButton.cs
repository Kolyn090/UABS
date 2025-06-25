using System.IO;
using UABS.Assets.Script.Event;
using UABS.Assets.Script.Misc;
using UABS.Assets.Script.Writer;
using UnityEngine;

namespace UABS.Assets.Script.LocalController
{
    public class WriteCacheButton : MonoBehaviour, IAppEnvironment
    {
        private AppEnvironment _appEnvironment = null;
        public AppEnvironment AppEnvironment => _appEnvironment;
        private WriteCache _writeCache;
        private SfbManager _sfbManager = new();

        public void Initialize(AppEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
            _writeCache = new(AppEnvironment.AssetsManager);
        }

        public void ClickButton()
        {
            string gameDataPath = _sfbManager.PickFolder("Select the game data folder (such as StandaloneWindows64)");
            Debug.Log(gameDataPath);
            string newSavePath = _sfbManager.PickFolderSuggestion("Select Folder to Save New Cache", PredefinedPaths.ExternalCache, GetDefaultName());
            _writeCache.CreateAndSaveNewCache(gameDataPath, newSavePath);
            AppEnvironment.Dispatcher.Dispatch(new CacheCreateEvent(newSavePath));
        }

        private string GetDefaultName()
        {
            int counter = 0;
            string baseName = "Default";
            string newName = baseName;
            while (Directory.Exists(Path.Combine(PredefinedPaths.ExternalCache, newName)))
            {
                counter++;
                newName = $"{baseName}_{counter}";
            }
            return counter == 0 ? baseName : newName;
        }
    }
}