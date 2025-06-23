using System.Collections.Generic;
using AssetsTools.NET.Extra;
using UABS.Assets.Script.DataStruct;
using UABS.Assets.Script.Event;
using UABS.Assets.Script.EventListener;
using UABS.Assets.Script.Misc;
using UABS.Assets.Script.Reader;
using UnityEngine;

namespace UABS.Assets.Script.DataSource
{
    public class AssetsDataSource : MonoBehaviour, IAppEventListener, IAppEnvironment
    {
        private BundleFileInstance _currBunInst;
        private List<AssetDisplayInfo> _assetsDisplayInfo = new();
        private ReadTextInfoFromBundle _readTextInfoFromBundle;
        private AppEnvironment _appEnvironment = null;
        public AppEnvironment AppEnvironment => _appEnvironment;

        public void OnEvent(AppEvent e)
        {
            if (e is BundleReadEvent bre)
            {
                _currBunInst = bre.Bundle;
                _assetsDisplayInfo = new();
                List<AssetTextInfo> assetsTextInfo = _readTextInfoFromBundle.ReadAllBasicInfo(_currBunInst);
                foreach (AssetTextInfo assetTextInfo in assetsTextInfo)
                {
                    _assetsDisplayInfo.Add(new()
                    {
                        assetTextInfo = assetTextInfo
                    });
                }
                _appEnvironment.Dispatcher.Dispatch(new AssetsDisplayInfoEvent(_assetsDisplayInfo));
            }
        }

        public void Initialize(AppEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
            _readTextInfoFromBundle = new(_appEnvironment.AssetsManager);
        }

        public AssetDisplayInfo GetDisplayInfoAtIndex(int index)
        {
            return _assetsDisplayInfo[index];
        }
    }
}