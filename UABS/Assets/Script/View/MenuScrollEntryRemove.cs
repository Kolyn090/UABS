using System.IO;
using TMPro;
using UABS.Assets.Script.Dispatcher;
using UABS.Assets.Script.DropdownOptions.Dependency;
using UABS.Assets.Script.Event;
using UABS.Assets.Script.Misc;
using UnityEngine;

namespace UABS.Assets.Script.View
{
    public class MenuScrollEntryRemove : MonoBehaviour, IMenuScrollEntry
    {
        private EventDispatcher _dispatcher;
        private string _shortPath;
        public string ShortPath { get => _shortPath;
            set
            {
                _shortPath = value;
                _text.text = value;
            } }

        [SerializeField]
        private TextMeshProUGUI _text;

        public void ClickButton()
        {
            if (_dispatcher != null)
            {
                string fullRelPath = Path.Combine(PredefinedPaths.ExternalCache, ShortPath);
                Debug.Log($"Removed cache folder '{fullRelPath}'");
                Directory.Delete(fullRelPath);
                _dispatcher.Dispatch(new CacheRemoveEvent(ShortPath));
            }
            else
            {
                Debug.LogWarning("Event dispatcher not found. Please assign one first.");
            }
        }

        public void AssignDispatcher(EventDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
    }
}