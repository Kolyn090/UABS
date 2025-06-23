using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UABS.Assets.Script.Dispatcher;
using UABS.Assets.Script.Event;

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

        public void Prev()
        {
            if (dispatcher != null)
            {
                dispatcher.Dispatch(new TexturePrevEvent());
            }
            else
            {
                throw new System.Exception("Texture view missing dispatcher. Please assign first.");
            }
        }

        public void Next()
        {
            if (dispatcher != null)
            {
                dispatcher.Dispatch(new TextureNextEvent());
            }
            else
            {
                throw new System.Exception("Texture view missing dispatcher. Please assign first.");
            }
        }
    }
}