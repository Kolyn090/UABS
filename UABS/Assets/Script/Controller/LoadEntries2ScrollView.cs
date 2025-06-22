using UnityEngine;
using UABS.Assets.Script.Reader;
using System.Collections.Generic;
using UABS.Assets.Script.Misc;
using UABS.Assets.Script.View;
using UABS.Assets.Script.ScriptableObjects;

namespace UABS.Assets.Script.Controller
{
    public class LoadEntries2ScrollView : MonoBehaviour
    {
        private const string TestBundlePath = @"\\?\C:\Program Files (x86)\Steam\steamapps\common\Otherworld Legends\Otherworld Legends_Data\StreamingAssets\aa\StandaloneWindows64\graphiceffecttextureseparatelygroup_assets_assets\sprites\unit\unit_other_pile.psd_0678876b821c494df01ee1384bec84f2.bundle";

        [SerializeField]
        private AssetType2IconData _assetType2IconData;

        [SerializeField]
        private GameObject _content;

        [SerializeField]
        private GameObject _entryPrefab;

        private void Start()
        {
            List<AssetBasicInfo> spriteBasicInfos = ReadBasicInfoFromBundle.ReadAllBasicInfo(TestBundlePath);
            ClearAndLoadBundle(spriteBasicInfos);
        }

        public void ClearAndLoadFolder()
        {
            ClearContentChildren();
        }

        public void ClearAndLoadBundle(List<AssetBasicInfo> assetBasicInfos)
        {
            ClearContentChildren();
            foreach (AssetBasicInfo assetBasicInfo in assetBasicInfos)
            {
                GameObject entryObj = CreateEntry();
                entryObj.transform.SetParent(_content.transform, worldPositionStays: false);

                EntryInfoView entryInfoView = entryObj.GetComponentInChildren<EntryInfoView>();
                entryInfoView.Render(
                    _assetType2IconData.GetIcon(assetBasicInfo.type),
                    assetBasicInfo.name,
                    assetBasicInfo.type,
                    assetBasicInfo.pathID.ToString());
            }
        }

        private GameObject CreateEntry()
        {
            return Instantiate(_entryPrefab);
        }

        private void ClearContentChildren()
        {
            Transform parentTransform = _content.transform;

            for (int i = parentTransform.childCount - 1; i >= 0; i--)
            {
                GameObject child = parentTransform.GetChild(i).gameObject;
                Destroy(child);
            }
        }
    }
}