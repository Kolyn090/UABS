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
    public class DependencyDataSource : MonoBehaviour, IAppEventListener, IAppEnvironment
    {
        private BundleFileInstance _currBunInst;
        private AppEnvironment _appEnvironment = null;
        public AppEnvironment AppEnvironment => _appEnvironment;
        private ReadDependencyInfo _readDependencyInfo;

        public void Initialize(AppEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
            _readDependencyInfo = new(AppEnvironment.AssetsManager);
        }

        public void OnEvent(AppEvent e)
        {
            if (e is BundleReadEvent bre)
            {
                _currBunInst = bre.Bundle;
            }
            else if (e is DependencyRequestEvent dre)
            {
                if (_currBunInst == null)
                {
                    Debug.LogWarning($"This is called earlier than BundleReadEvent, inspect a bundle first.");
                    return;
                }
                List<DependencyInfo> dependencyInfos = _readDependencyInfo.ReadInfoFor(_currBunInst, dre.ReadFromCachePath);
                foreach (DependencyInfo dependencyInfo in dependencyInfos)
                {
                    Debug.Log($"{dependencyInfo.name}, {dependencyInfo.cabCode}, {dependencyInfo.path}");
                }
            }
            else if (e is FolderReadEvent fre)
            {
                _currBunInst = null;
            }
        }
    }
}