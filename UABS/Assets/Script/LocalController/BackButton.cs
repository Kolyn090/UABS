using System.IO;
using UABS.Assets.Script.Event;
using UABS.Assets.Script.EventListener;
using UABS.Assets.Script.Misc;
using UnityEngine;

namespace UABS.Assets.Script.View
{
    public class BackButton : MonoBehaviour, IAppEventListener, IAppEnvironment
    {
        private AppEnvironment _appEnvironment = null;
        public AppEnvironment AppEnvironment => _appEnvironment;
        private string _currPath;

        public void Initialize(AppEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public void ClickButton()
        {
            AppEnvironment.Dispatcher.Dispatch(new FolderReadEvent(Path.GetDirectoryName(_currPath)));
        }

        public void OnEvent(AppEvent e)
        {
            if (e is FolderReadEvent fre)
            {
                _currPath = fre.FolderPath;
            }
        }
    }
}