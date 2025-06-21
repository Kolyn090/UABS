using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UABS.Assets.Script.View
{
    public class TextureView : MonoBehaviour
    {
        [SerializeField]
        private RawImage _rawImage; // Drag a UI RawImage from the scene into the Inspector
        private List<Texture2D> _textures = new();
        private int _index;

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                if (_index >= 0 && _index < _textures.Count)
                {
                    AssignTextureToImage(_textures[_index]);
                }
            }
        }

        public void AssignTextures(List<Texture2D> textures)
        {
            _textures = textures;
            Index = 0;
        }

        private void AssignTextureToImage(Texture2D texture)
        {
            _rawImage.texture = texture;
            // _rawImage.SetNativeSize();
        }

        public void Prev()
        {
            Index = StayInRange(Index-1);
        }

        public void Next()
        {
            Index = StayInRange(Index+1);
        }

        private int StayInRange(int index)
        {
            if (index < 0)
                return _textures.Count - 1;
            if (index >= _textures.Count)
                return 0;
            return index;
        }
    }
}