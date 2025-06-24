using SFB;
using UABS.Assets.Script.Event;
using UABS.Assets.Script.Misc;
using UABS.Assets.Script.Reader;
using UnityEngine;

namespace UABS.Assets.Script.DropdownOptions
{
    public class OpenFile : MonoBehaviour, IAppEnvironment, IDropdownButton
    {
        private AppEnvironment _appEnvironment = null;
        public AppEnvironment AppEnvironment => _appEnvironment;

        public void Initialize(AppEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public void ClickButton()
        {
            string filePath = PickFile();
            if (filePath == "")
            {
                Debug.Log("Couldn't find path to Folder.");
            }
            else
            {
                BundleReader bundleReader = new(AppEnvironment);
                // Debug.Log(filePath);
                bundleReader.ReadBundle(filePath);
            }
        }

        private string PickFile()
        {
            var paths = StandaloneFileBrowser.OpenFilePanel("Select .bundle File", "", "bundle", false);
            if (paths.Length > 0)
            {
                return paths[0];
            }
            return "";
        }
    }
}