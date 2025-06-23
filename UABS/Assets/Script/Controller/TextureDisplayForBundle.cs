using UnityEngine;
using UABS.Assets.Script.Reader;
using UABS.Assets.Script.View;
using System.Collections.Generic;
using UABS.Assets.Script.EventListener;
using UABS.Assets.Script.Event;
using UABS.Assets.Script.Misc;

namespace UABS.Assets.Script.Controller
{
    public class TextureDisplayForBundle : MonoBehaviour, IAppEventListener, IAppEnvironment
    {
        [SerializeField]
        private TextureView _textureView;

        private ReadTexturesFromBundle _readTexturesFromBundle;

        private AppEnvironment _appEnvironment = null;
        public AppEnvironment AppEnvironment => _appEnvironment;

        public void Initialize(AppEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
            _readTexturesFromBundle = new(_appEnvironment.AssetsManager);
        }

        public void OnEvent(AppEvent e)
        {
            if (e is BundleReadEvent bre)
            {
                List<Texture2D> textures = _readTexturesFromBundle.ReadSpritesInAtlas(bre.Bundle);
                _textureView.AssignTextures(textures);
            }
        }
    }
}