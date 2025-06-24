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
        private List<EntryInfoView> _currEntryInfoViews = new();

        public void ClearAndLoadFolder()
        {
            ClearContentChildren();
        }

        public void LoadBundle(List<AssetTextInfo> assetTextInfos)
        {
            for (int i = 0; i < _currEntryInfoViews.Count; i++)
            {
                _currEntryInfoViews[i].AssignStuff(i, assetTextInfos.Count, _scrollbarRef);
                _currEntryInfoViews[i].Render(assetTextInfos[i]);
            }
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
            for (int i = 0; i < _currEntryInfoViews.Count; i++)
            {
                AppEnvironment.Dispatcher.Unregister(_currEntryInfoViews[i]);
            }

            Transform parentTransform = _content.transform;

            for (int i = parentTransform.childCount - 1; i >= 0; i--)
            {
                GameObject child = parentTransform.GetChild(i).gameObject;
                Destroy(child);
                child = null;
            }
        }

        public void OnEvent(AppEvent e)
        {
            if (e is AssetsDisplayInfoEvent adie)
            {
                if (adie.ClearCurrEntries)
                {
                    ClearAndLoadBundle(adie.AssetsDisplayInfo.Select(x => x.assetTextInfo).ToList());
                }
                else
                {
                    LoadBundle(adie.AssetsDisplayInfo.Select(x => x.assetTextInfo).ToList());
                }
            }
        }

        public void Initialize(AppEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }
    }
}