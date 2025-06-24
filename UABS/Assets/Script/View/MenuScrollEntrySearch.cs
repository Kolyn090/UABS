using TMPro;
using UABS.Assets.Script.DropdownOptions.Dependency;
using UnityEngine;

namespace UABS.Assets.Script.View
{
    public class MenuScrollEntrySearch : MonoBehaviour, IMenuScrollEntry
    {
        private string _shortPath;
        public string ShortPath { get => _shortPath; set => _shortPath = value; }

        [SerializeField]
        private TextMeshProUGUI _text;

        public void ClickButton()
        {

        }

        public void AssignText(string newText)
        {
            _text.text = newText;
        }
    }
}