using UnityEngine;
using System.Collections.Generic;
using UABS.Assets.Script.Misc;
using UABS.Assets.Script.View;
using UABS.Assets.Script.EventListener;
using UABS.Assets.Script.Event;
using System.Linq;
using UnityEngine.UI;


namespace UABS.Assets.Script.Controller
{
    public class LoadEntries2ScrollView : MonoBehaviour, IAppEventListener, IAppEnvironment
    {
        [SerializeField]
        private GameObject _content;

        [SerializeField]
        private Scrollbar _scrollbarRef;

        [SerializeField]
        private GameObject _entryPrefab;

        private AppEnvironment _appEnvironment = null;
        public AppEnvironment AppEnvironment => _appEnvironment;
        List<EntryInfoView> _currEntryInfoViews = new();


        public void ClearAndLoadFolder()
        {
            ClearContentChildren();
        }

        public void LoadBundle()
        {
            
        }

        public void ClearAndLoadBundle(List<AssetTextInfo> assetTextInfos)
        {
            ClearContentChildren();

            _currEntryInfoViews = new();
            for (int i = 0; i < assetTextInfos.Count; i++)
            {
                GameObject entryObj = CreateEntry();
                entryObj.transform.SetParent(_content.transform, worldPositionStays: false);
                _currEntryInfoViews.Add(entryObj.GetComponentInChildren<EntryInfoView>());
            }

            for (int i = 0; i < assetTextInfos.Count; i++)
            {
                AssetTextInfo assetTextInfo = assetTextInfos[i];
                EntryInfoView entryInfoView = _currEntryInfoViews[i];
                entryInfoView.dispatcher = _appEnvironment.Dispatcher;
                _appEnvironment.Dispatcher.Register(entryInfoView);
                entryInfoView.AssignStuff(i, assetTextInfos.Count, _scrollbarRef);
                entryInfoView.Render(assetTextInfo);
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
            if (e is AssetsDisplayInfoEvent adie)
            {
                ClearAndLoadBundle(adie.AssetsDisplayInfo.Select(x => x.assetTextInfo).ToList());
            }
        }

        public void Initialize(AppEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }
    }
}