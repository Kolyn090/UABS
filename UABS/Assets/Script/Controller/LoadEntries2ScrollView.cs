using UnityEngine;
using UABS.Assets.Script.Reader;
using System.Collections.Generic;
using UABS.Assets.Script.Misc;
using UABS.Assets.Script.View;
using UABS.Assets.Script.EventListener;
using UABS.Assets.Script.Event;


namespace UABS.Assets.Script.Controller
{
    public class LoadEntries2ScrollView : MonoBehaviour, IAppEventListener, IAppEnvironment
    {
        [SerializeField]
        private GameObject _content;

        [SerializeField]
        private GameObject _entryPrefab;

        private ReadDisplayInfoFromBundle _readDisplayInfoFromBundle;

        private AppEnvironment _appEnvironment = null;
        public AppEnvironment AppEnvironment => _appEnvironment;

        public void ClearAndLoadFolder()
        {
            ClearContentChildren();
        }

        public void ClearAndLoadBundle(List<AssetDisplayInfo> assetDisplayInfos)
        {
            ClearContentChildren();

            List<EntryInfoView> entryInfoViews = new();
            for (int i = 0; i < assetDisplayInfos.Count - 1; i++)
            {
                GameObject entryObj = CreateEntry();
                entryObj.transform.SetParent(_content.transform, worldPositionStays: false);
                entryInfoViews.Add(entryObj.GetComponentInChildren<EntryInfoView>());
            }

            App.Instance.InitializeAllAppEnvironment();

            for (int i = 0; i < assetDisplayInfos.Count-1; i++)
            {
                AssetDisplayInfo assetDisplayInfo = assetDisplayInfos[i];
                EntryInfoView entryInfoView = entryInfoViews[i];
                entryInfoView.Render(assetDisplayInfo);
            }
        }

        private GameObject CreateEntry()
        {
            return Instantiate(_entryPrefab);
        }

        private void ClearContentChildren()
        {
            Transform parentTransform = _content.transform;

            for (int i = parentTransform.childCount-1; i >= 0; i--)
            {
                GameObject child = parentTransform.GetChild(i).gameObject;
                Destroy(child);
            }
        }

        public void OnEvent(AppEvent e)
        {
            if (e is BundleReadEvent bre)
            {
                ClearAndLoadBundle(_readDisplayInfoFromBundle.ReadAllBasicInfo(bre.Bundle));
            }
        }

        public void Initialize(AppEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
            _readDisplayInfoFromBundle = new(_appEnvironment.AssetsManager);
        }
    }
}