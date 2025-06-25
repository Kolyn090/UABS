using UABS.Assets.Script.Event;
using UABS.Assets.Script.Misc;
using UABS.Assets.Script.Writer;
using UnityEngine;

namespace UABS.Assets.Script.DropdownOptions
{
    public class OpenFolder : MonoBehaviour, IAppEnvironment, IDropdownButton
    {
        private AppEnvironment _appEnvironment = null;
        public AppEnvironment AppEnvironment => _appEnvironment;
        private SfbManager _sfbManager = new();

        public void Initialize(AppEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public void ClickButton()
        {
            string folderPath = _sfbManager.PickFolder();
            if (folderPath == "")
            {
                Debug.Log("Couldn't find path to Folder.");
            }
            else
            {
                AppEnvironment.Dispatcher.Dispatch(new FolderReadEvent(folderPath));
            }
        }
    }
}