using UABS.Assets.Script.Misc;
using UnityEngine;

namespace UABS.Assets.Script.DropdownOptions
{
    public class OpenFolder : MonoBehaviour, IAppEnvironment
    {
        private AppEnvironment _appEnvironment = null;
        public AppEnvironment AppEnvironment => _appEnvironment;

        public void Initialize(AppEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public void ClickButton()
        {
            
        }
    }
}