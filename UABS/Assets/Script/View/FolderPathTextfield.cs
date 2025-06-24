using TMPro;
using UABS.Assets.Script.Event;
using UABS.Assets.Script.EventListener;
using UnityEngine;

namespace UABS.Assets.Script.View
{
    public class FolderPathTextfield : MonoBehaviour, IAppEventListener
    {
        [SerializeField]
        private TMP_InputField _pathTextfield;

        private void Start()
        {
            _pathTextfield.readOnly = true;
        }

        public void OnEvent(AppEvent e)
        {
            if (e is FolderReadEvent fre)
            {
                _pathTextfield.text = fre.FolderPath;
            }
        }
    }
}