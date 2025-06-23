using UnityEngine;
using UABS.Assets.Script.Event;
using TMPro;

namespace UABS.Assets.Script.EventListener
{
    public class DisplayInfoPanel : MonoBehaviour, IAppEventListener
    {
        [SerializeField]
        private TMP_InputField  _nameField;

        [SerializeField]
        private TMP_InputField  _pathIDField;

        [SerializeField]
        private TMP_InputField  _fileIDField;

        [SerializeField]
        private TMP_InputField  _sizeField;

        [SerializeField]
        private TMP_InputField  _pathField;

        public void OnEvent(AppEvent e)
        {
            if (e is AssetDisplayInfoEvent assetDisplayInfoEvent)
            {
                _nameField.text = assetDisplayInfoEvent.Info.name;
                _pathIDField.text = assetDisplayInfoEvent.Info.pathID.ToString();
                _fileIDField.text = assetDisplayInfoEvent.Info.fileID.ToString();
                _sizeField.text = assetDisplayInfoEvent.Info.size.ToString();
                _pathField.text = assetDisplayInfoEvent.Info.path;
            }
        }
    }
}