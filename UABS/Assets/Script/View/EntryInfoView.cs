using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AssetsTools.NET.Extra;
using UABS.Assets.Script.ScriptableObjects;
using UABS.Assets.Script.Event;
using UABS.Assets.Script.Misc;

namespace UABS.Assets.Script.View
{
    public class EntryInfoView : MonoBehaviour, IAppEnvironment
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

        private AssetDisplayInfo _storedInfo;

        private AppEnvironment _appEnvironment = null;

        public AppEnvironment AppEnvironment => _appEnvironment;

        public void TriggerEvent()
        {
            _appEnvironment.Dispatcher.Dispatch(new AssetDisplayInfoEvent(_storedInfo));
        }

        public void Render(AssetDisplayInfo assetDisplayInfo)
        {
            _storedInfo = assetDisplayInfo;
            AssetClassID assetType = assetDisplayInfo.type;
            long pathID = assetDisplayInfo.pathID;
            string name = assetDisplayInfo.name;

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

        public void Initialize(AppEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }
    }
}