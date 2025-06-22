using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UABS.Assets.Script.ScriptableObjects;
using AssetsTools.NET.Extra;

namespace UABS.Assets.Script.View
{
    public class EntryInfoView : MonoBehaviour
    {
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TextMeshProUGUI _name;

        [SerializeField]
        private TextMeshProUGUI _type;

        [SerializeField]
        private TextMeshProUGUI _pathID;

        public void Render(Sprite icon, string name, AssetClassID assetType, string pathID)
        {
            _icon.sprite = icon;
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
            _pathID.text = pathID;
        }
    }
}