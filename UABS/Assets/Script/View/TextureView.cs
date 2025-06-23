using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UABS.Assets.Script.Misc;
using UABS.Assets.Script.Event;

namespace UABS.Assets.Script.View
{
    public class TextureView : MonoBehaviour, IAppEnvironment
    {
        [SerializeField]
        private RawImage _rawImage;
        [SerializeField]
        private TextMeshProUGUI _indexText;

        private AppEnvironment _appEnvironment = null;

        public AppEnvironment AppEnvironment => _appEnvironment;

        // private List<Texture2D> _textures = new();
        // private int _index;

        // public int Index
        // {
        //     get { return _index; }
        //     set
        //     {
        //         _index = value;
        //         if (_index >= 0 && _index < _textures.Count)
        //         {
        //             AssignTextureToImage(_textures[_index]);
        //             _indexText.text = $"{_index+1}/{_textures.Count}";
        //         }
        //     }
        // }

        // public void AssignTextures(List<Texture2D> textures)
        // {
        //     _textures = textures;
        //     Index = 0;
        // }

        public void AssignTextureToImage(Texture2D texture)
        {
            _rawImage.texture = texture;
            texture.filterMode = FilterMode.Point;
            // _rawImage.SetNativeSize();
        }

        public void Prev()
        {
            // TODO need focus system
            // _appEnvironment.Dispatcher.Dispatch(new AssetDisplayInfoEvent(_storedInfo));
        }

        public void Next()
        {
            // _appEnvironment.Dispatcher.Dispatch(new AssetDisplayInfoEvent(_storedInfo));
        }

        public void Initialize(AppEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        // private int StayInRange(int index)
        // {
        //     if (index < 0)
        //         return _textures.Count - 1;
        //     if (index >= _textures.Count)
        //         return 0;
        //     return index;
        // }
    }
}