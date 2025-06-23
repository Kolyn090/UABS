using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AssetsTools.NET.Extra;
using UABS.Assets.Script.ScriptableObjects;
using UABS.Assets.Script.Event;
using UABS.Assets.Script.Misc;
using UABS.Assets.Script.Dispatcher;
using UABS.Assets.Script.EventListener;

namespace UABS.Assets.Script.View
{
    public class EntryInfoView : MonoBehaviour, IAppEventListener
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
        private AssetType2IconData _assetType2IconData;

        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TextMeshProUGUI _name;

        [SerializeField]
        private TextMeshProUGUI _type;

        [SerializeField]
        private TextMeshProUGUI _pathID;

        private AssetTextInfo _storedInfo;

        public EventDispatcher dispatcher = null;
        private Scrollbar _scrollbarRef;

        private int _index;
        private int _totalEntryNum;

        public void TriggerEvent()
        {
            if (dispatcher != null)
            {
                dispatcher.Dispatch(new AssetTextInfoEvent(_storedInfo));
                dispatcher.Dispatch(new SelectionEvent(_storedInfo.pathID, _index, _totalEntryNum));
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

        public void Render(AssetTextInfo assetTextInfo)
        {
            _storedInfo = assetTextInfo;
            _bg.color = _index % 2 == 0 ? _alternateColor1 : _alternateColor2;
            AssetClassID assetType = assetTextInfo.type;
            long pathID = assetTextInfo.pathID;
            string name = assetTextInfo.name;

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

        public void OnEvent(AppEvent e)
        {
            if (e is SelectionEvent pie)
            {
                if (_storedInfo.pathID == pie.PathID)
                {
                    if (_bg.color != _highlightColor)
                    {
                        _bg.color = _highlightColor;
                    }
                    else
                    {
                        _bg.color = _index % 2 == 0 ? _alternateColor1 : _alternateColor2;
                    }
                    if (pie.UseJump)
                        Jump2Me();
                    dispatcher.Dispatch(new AssetTextInfoEvent(_storedInfo));
                }
                else
                {
                    if (_bg != null)
                    {
                        if (_bg.color == _highlightColor)
                        {
                            _bg.color = _index % 2 == 0 ? _alternateColor1 : _alternateColor2;
                        }
                    }
                }
            }
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