using System.Collections.Generic;
using System.IO;
using System.Linq;
using SFB;
using UABS.Assets.Script.DropdownOptions.Dependency;
using UABS.Assets.Script.Event;
using UABS.Assets.Script.Misc;
using UABS.Assets.Script.Reader;
using UABS.Assets.Script.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UABS.Assets.Script.DropdownOptions
{
    public class HoverSearchBundleDep : HoverArea, IAppEnvironment
    {
        private AppEnvironment _appEnvironment = null;
        public AppEnvironment AppEnvironment => _appEnvironment;

        [SerializeField]
        private GameObject _entryPrefab;

        [SerializeField]
        private RectTransform _content;

        private ReadExternalCache _readExternalCache;

        private List<IMenuScrollEntry> _menuScrollEntries = new();

        public void Initialize(AppEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
            _readExternalCache = new();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (_menuScrollEntries.Count != 0)
                return;
            // Search paths and create prefabs
            List<string> paths = _readExternalCache.GetCacheFoldersInExternal();
            foreach (string path in paths)
            {
                (IMenuScrollEntry, GameObject) pair = CreateScrollEntry(Path.GetFileName(path));
                _menuScrollEntries.Add(pair.Item1);
                pair.Item2.GetComponent<RectTransform>().SetParent(_content.transform, worldPositionStays: false);
            }
        }

        private (IMenuScrollEntry, GameObject) CreateScrollEntry(string path)
        {
            GameObject entry = Instantiate(_entryPrefab);
            IMenuScrollEntry menuScrollEntry = entry.GetComponentsInChildren<MonoBehaviour>(true)
                                                .OfType<IMenuScrollEntry>()
                                                .FirstOrDefault();
            menuScrollEntry.AssignText(path);
            return (menuScrollEntry, entry);
        }
    }
}