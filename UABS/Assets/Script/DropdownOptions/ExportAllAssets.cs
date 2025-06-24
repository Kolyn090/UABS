using SFB;
using UABS.Assets.Script.Event;
using UABS.Assets.Script.Misc;
using UnityEngine;

namespace UABS.Assets.Script.DropdownOptions
{
    public class ExportAllAssets : MonoBehaviour, IAppEnvironment, IDropdownButton
    {
        private AppEnvironment _appEnvironment = null;
        public AppEnvironment AppEnvironment => _appEnvironment;

        public void Initialize(AppEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public void ClickButton()
        {
            string folderPath = PickFolder();
            if (folderPath == "")
            {
                Debug.Log("Couldn't find path to Folder.");
            }
            else
            {
                AppEnvironment.Dispatcher.Dispatch(new ExportAssetsEvent(new()
                {
                    exportType = DataStruct.ExportType.All,
                    destination = folderPath
                }));
            }
        }

        private string PickFolder()
        {
            var paths = StandaloneFileBrowser.OpenFolderPanel("Select Folder", "", false);
            if (paths.Length > 0)
            {
                return paths[0];
            }
            return "";
        }
    }
}