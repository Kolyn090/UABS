using System.Collections.Generic;
using System.IO;
using System.Linq;
using UABS.Assets.Script.DropdownOptions.Dependency;
using UABS.Assets.Script.Event;
using UABS.Assets.Script.Misc;
using UABS.Assets.Script.Reader;
using UABS.Assets.Script.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UABS.Assets.Script.EventListener;

namespace UABS.Assets.Script.DropdownOptions
{
    public class HoverCacheDepDerive : HoverArea, IAppEnvironment, IAppEventListener
    {
        private AppEnvironment _appEnvironment = null;
        public AppEnvironment AppEnvironment => _appEnvironment;

        [SerializeField]
        private GameObject _entryPrefab;

        [SerializeField]
        private RectTransform _content;

        private ReadExternalCache _readExternalCache;

        private List<(string, GameObject)> _entries = new();

        [SerializeField]
        private Color _hoverColor;

        [SerializeField]
        private Color _normalColor;

        [SerializeField]
        private Image _bgImage;

        public void Initialize(AppEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
            _readExternalCache = new();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            _bgImage.color = _hoverColor;
            if (_entries.Count != 0)
                return;
            // Search paths and create prefabs
            List<string> paths = _readExternalCache.GetCacheFoldersInExternal();
            foreach (string path in paths)
            {
                (IMenuScrollEntry, GameObject) pair = CreateScrollEntry(Path.GetFileName(path));
                _entries.Add((pair.Item1.ShortPath, pair.Item2));
                pair.Item2.GetComponent<RectTransform>().SetParent(_content.transform, worldPositionStays: false);
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            _bgImage.color = _normalColor;
        }

        private (IMenuScrollEntry, GameObject) CreateScrollEntry(string path)
        {
            GameObject entry = Instantiate(_entryPrefab);
            IMenuScrollEntry menuScrollEntry = entry.GetComponentsInChildren<MonoBehaviour>(true)
                                                .OfType<IMenuScrollEntry>()
                                                .FirstOrDefault();
            menuScrollEntry.ShortPath = path;
            menuScrollEntry.AssignDispatcher(AppEnvironment.Dispatcher);
            return (menuScrollEntry, entry);
        }

        public void OnEvent(AppEvent e)
        {
            if (e is CacheRemoveEvent cre)
            {
                GameObject entryToRemove = null;
                foreach (var pair in _entries)
                {
                    if (pair.Item1 == cre.RemovedPath)
                    {
                        entryToRemove = pair.Item2;
                    }
                }
                if (entryToRemove == null)
                    return;

                Destroy(entryToRemove);
                entryToRemove = null;
            }
        }
    }
}