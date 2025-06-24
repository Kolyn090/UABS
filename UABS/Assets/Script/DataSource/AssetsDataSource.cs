using System.Collections.Generic;
using System.Linq;
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
            else if (e is SortScrollViewEvent ssve)
            {
                SortByType sortByType = ssve.SortProp.sortByType;
                SortOrder sortOrder = ssve.SortProp.sortOrder;
                _assetsDisplayInfo = SortedAssetsDisplayInfo(sortByType, sortOrder);
                _appEnvironment.Dispatcher.Dispatch(new AssetsDisplayInfoEvent(_assetsDisplayInfo));
            }
        }

        private List<AssetDisplayInfo> SortedAssetsDisplayInfo(SortByType sortByType, SortOrder sortOrder)
        {
            if (sortByType == SortByType.Name)
            {
                if (sortOrder == SortOrder.Down)
                {
                    return _assetsDisplayInfo.OrderBy(x => x.assetTextInfo.name).ToList();
                }
                else if (sortOrder == SortOrder.Up)
                {
                    return _assetsDisplayInfo.OrderByDescending(x => x.assetTextInfo.name).ToList();
                }
            }
            else if (sortByType == SortByType.Type)
            {
                if (sortOrder == SortOrder.Down)
                {
                    return _assetsDisplayInfo.OrderBy(x => x.assetTextInfo.type).ToList();
                }
                else if (sortOrder == SortOrder.Up)
                {
                    return _assetsDisplayInfo.OrderByDescending(x => x.assetTextInfo.type).ToList();
                }
            }
            else if (sortByType == SortByType.PathID)
            {
                if (sortOrder == SortOrder.Down)
                {
                    return _assetsDisplayInfo.OrderBy(x => x.assetTextInfo.pathID).ToList();
                }
                else if (sortOrder == SortOrder.Up)
                {
                    return _assetsDisplayInfo.OrderByDescending(x => x.assetTextInfo.pathID).ToList();
                }
            }
            return _assetsDisplayInfo;
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