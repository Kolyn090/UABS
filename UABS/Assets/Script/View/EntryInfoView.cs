using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AssetsTools.NET.Extra;
using UABS.Assets.Script.ScriptableObjects;
using UABS.Assets.Script.Event;
using UABS.Assets.Script.Misc;
using UABS.Assets.Script.Dispatcher;

namespace UABS.Assets.Script.View
{
    public class EntryInfoView : MonoBehaviour
    {
        [SerializeField]
        private AssetType2IconData _assetType2IconData;
        
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TextMeshProUGUI _name;

        [SerializeField]
        private TextMeshProUGUI _type;

        [SerializeField]
        private TextMeshProUGUI _pathID;
        public long PathID => long.Parse(_pathID.text);

        private AssetTextInfo _storedInfo;

        public EventDispatcher dispatcher = null;

        public void TriggerEvent()
        {
            if (dispatcher != null)
            {
                dispatcher.Dispatch(new AssetTextInfoEvent(_storedInfo));
                dispatcher.Dispatch(new PathIDEvent(PathID));
            }
            else
            {
                throw new System.Exception("Entry Info View missing dispatcher. Please assign first.");
            }
        }

        public void Render(AssetTextInfo assetTextInfo)
        {
            _storedInfo = assetTextInfo;
            AssetClassID assetType = assetTextInfo.type;
            long pathID = assetTextInfo.pathID;
            string name = assetTextInfo.name;

            _icon.sprite = _assetType2IconData.GetIcon(assetType);
            _name.text = name;

            int classIdInt = (int)assetType;
            string className = "";

            if (System.Enum.IsDefined(typeof(AssetClassID), classIdInt))
            {
                className = ((AssetClassID)classIdInt).ToString();
            }
            else
            {
                Debug.LogWarning($"Unknown AssetClassID: {classIdInt}");
            }

            _type.text = className;
            _pathID.text = pathID.ToString();
            Debug.Log($"{_name.text}");
        }
    }
}