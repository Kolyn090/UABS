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
            if (e is AssetTextInfoEvent assetTextInfoEvent)
            {
                _nameField.text = assetTextInfoEvent.Info.name;
                _pathIDField.text = assetTextInfoEvent.Info.pathID.ToString();
                _fileIDField.text = assetTextInfoEvent.Info.fileID.ToString();
                _sizeField.text = $"{assetTextInfoEvent.Info.compressedSize} ({assetTextInfoEvent.Info.uncompressedSize})";
                _pathField.text = assetTextInfoEvent.Info.path;
            }
        }
    }
}