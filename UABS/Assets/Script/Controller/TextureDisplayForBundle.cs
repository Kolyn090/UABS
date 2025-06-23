using UnityEngine;
using UABS.Assets.Script.Reader;
using UABS.Assets.Script.View;
using System.Collections.Generic;
using UABS.Assets.Script.EventListener;
using UABS.Assets.Script.Event;
using UABS.Assets.Script.Misc;
using AssetsTools.NET.Extra;

namespace UABS.Assets.Script.Controller
{
    public class TextureDisplayForBundle : MonoBehaviour, IAppEventListener, IAppEnvironment
    {
        [SerializeField]
        private TextureView _textureView;

        private ReadTexturesFromBundle _readTexturesFromBundle;

        private AppEnvironment _appEnvironment = null;
        public AppEnvironment AppEnvironment => _appEnvironment;

        private BundleFileInstance _currBunInst = null;

        private Dictionary<long, Texture2D> _storedTexture2D = new();

        public void Initialize(AppEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
            _readTexturesFromBundle = new(_appEnvironment.AssetsManager);
        }

        public void OnEvent(AppEvent e)
        {
            if (e is BundleReadEvent bre)
            {
                _currBunInst = bre.Bundle;
                _storedTexture2D = new();
                // List<Texture2D> textures = _readTexturesFromBundle.ReadSpritesInAtlas(bre.Bundle);
                // _textureView.AssignTextures(textures);
            }
            else if (e is AssetDisplayInfoEvent assetDisplayInfoEvent)
            {
                long pathID = assetDisplayInfoEvent.Info.pathID;
                Texture2D readTexture = _readTexturesFromBundle.ReadSpriteByPathID(_currBunInst, pathID);
                if (readTexture == null)
                {
                    readTexture = _readTexturesFromBundle.ReadTexture2DByPathID(_currBunInst, pathID);
                }
                if (readTexture == null)
                {
                    Debug.LogError($"The given path id {pathID} is neither Texture2D nor Sprite.");
                }
                _storedTexture2D[pathID] = readTexture;
                _textureView.AssignTextureToImage(readTexture);
            }
        }
    }
}