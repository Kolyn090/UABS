using UnityEngine;
using UABS.Assets.Script.View;
using UABS.Assets.Script.EventListener;
using UABS.Assets.Script.Event;
using UABS.Assets.Script.Reader;
using UABS.Assets.Script.Misc;
using AssetsTools.NET.Extra;
using System.Collections.Generic;

namespace UABS.Assets.Script.Controller
{
    public class TextureDisplayForBundle : MonoBehaviour, IAppEventListener, IAppEnvironment
    {
        [SerializeField]
        private TextureView _textureView;
        private ReadTexturesFromBundle _readTexturesFromBundle;
        private BundleFileInstance _currBunInst;
        private Dictionary<long, Texture2D> _cacheTextureByPathID = new();

        private AppEnvironment _appEnvironment = null;
        public AppEnvironment AppEnvironment => _appEnvironment;

        public void Initialize(AppEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
            _textureView.dispatcher = appEnvironment.Dispatcher;
            _readTexturesFromBundle = new(appEnvironment.AssetsManager);
        }

        public void OnEvent(AppEvent e)
        {
            if (e is BundleReadEvent bre)
            {
                _currBunInst = bre.Bundle;
                _cacheTextureByPathID = new();
            }
            else if (e is PathIDEvent pie)
            {
                _textureView.Render(GetTextureByPathID(pie.PathID));
                _textureView.AssignIndexText($"{pie.CurrIndex+1} / {pie.TotalNumOfAssets}");
            }
        }

        private Texture2D GetTextureByPathID(long pathID)
        {
            if (!_cacheTextureByPathID.ContainsKey(pathID))
            {
                Texture2D readTexture = _readTexturesFromBundle.ReadSpriteByPathID(_currBunInst, pathID);
                if (readTexture == null)
                {
                    readTexture = _readTexturesFromBundle.ReadTexture2DByPathID(_currBunInst, pathID);
                }
                if (readTexture == null)
                {
                    Debug.LogError($"The given path id {pathID} is neither Texture2D nor Sprite.");
                }
                _cacheTextureByPathID[pathID] = readTexture;
                return readTexture;
            }
            return _cacheTextureByPathID[pathID];
        }
    }
}