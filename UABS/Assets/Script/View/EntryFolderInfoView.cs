using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AssetsTools.NET.Extra;
using UABS.Assets.Script.ScriptableObjects;
using UABS.Assets.Script.Event;
using UABS.Assets.Script.Misc;
using UABS.Assets.Script.Dispatcher;
using UABS.Assets.Script.EventListener;
using UABS.Assets.Script.DataStruct;

namespace UABS.Assets.Script.View
{
    public class EntryFolderInfoView : MonoBehaviour, IAppEventListener
    {
        [SerializeField]
        private Image _bg;

        [SerializeField]
        private Color _highlightColor;
        [SerializeField]
        private Color _alternateColor1;
        [SerializeField]
        private Color _alternateColor2;

        [SerializeField]
        private FolderViewType2IconData _folderViewType2IconData;  // Folder

        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TextMeshProUGUI _name;

        [SerializeField]
        private TextMeshProUGUI _path;  // Folder

        private FolderViewInfo _storedFolderViewInfo;  // Folder

        public EventDispatcher dispatcher = null;
        private Scrollbar _scrollbarRef;

        private int _index;
        private int _totalEntryNum;
        
        public void TriggerEvent()
        {
            if (dispatcher != null)
            {
                
            }
            else
            {
                throw new System.Exception("Entry Info View missing dispatcher. Please assign first.");
            }
        }

        public void AssignStuff(int index, int totalEntryNum, Scrollbar scrollbar)
        {
            _index = index;
            _totalEntryNum = totalEntryNum;
            _scrollbarRef = scrollbar;
        }

        public void Render(FolderViewInfo folderViewInfo)
        {
            _storedFolderViewInfo = folderViewInfo;
            _bg.color = _index % 2 == 0 ? _alternateColor1 : _alternateColor2;
            FolderViewType folderViewType = folderViewInfo.type;
            string name = folderViewInfo.name;
            string path = folderViewInfo.path;

            _icon.sprite = _folderViewType2IconData.GetIcon(folderViewType);
            _name.text = name;
            _path.text = path;
        }

        public void OnEvent(AppEvent e)
        {
            // if (e is SelectionEvent ase)
            // {
            //     if (_storedAssetInfo.pathID == ase.PathID)
            //     {
            //         if (_bg.color != _highlightColor)
            //         {
            //             _bg.color = _highlightColor;
            //         }
            //         else
            //         {
            //             _bg.color = _index % 2 == 0 ? _alternateColor1 : _alternateColor2;
            //         }
            //         if (ase.UseJump)
            //             Jump2Me();
            //         dispatcher.Dispatch(new AssetTextInfoEvent(_storedAssetInfo));
            //     }
            //     else
            //     {
            //         if (_bg != null)
            //         {
            //             if (_bg.color == _highlightColor)
            //             {
            //                 _bg.color = _index % 2 == 0 ? _alternateColor1 : _alternateColor2;
            //             }
            //         }
            //     }
            // }
        }

        private void Jump2Me()
        {
            float newScrollbarValue = 1 - _index / (float)_totalEntryNum;
            if (_index == _totalEntryNum - 1)
                newScrollbarValue = 0;
            _scrollbarRef.value = newScrollbarValue;
        }
    }
}