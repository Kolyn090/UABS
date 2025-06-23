using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UABS.Assets.Script.Dispatcher;

namespace UABS.Assets.Script.View
{
    public class TextureView : MonoBehaviour
    {
        [SerializeField]
        private RawImage _rawImage;
        [SerializeField]
        private TextMeshProUGUI _indexText;
        public EventDispatcher dispatcher;

        public void Render(Texture2D texture)
        {
            _rawImage.texture = texture;
            texture.filterMode = FilterMode.Point;
        }

        public void AssignIndexText(string text)
        {
            _indexText.text = text;
        }
    }
}