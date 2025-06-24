using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AssetsTools.NET.Extra;
using UABS.Assets.Script.DataStruct;
using UABS.Assets.Script.Event;
using UABS.Assets.Script.EventListener;
using UABS.Assets.Script.Misc;
using UABS.Assets.Script.Reader;
using UnityEngine;

namespace UABS.Assets.Script.DataSource
{
    public class FolderDataSource : MonoBehaviour, IAppEventListener, IAppEnvironment
    {
        private AppEnvironment _appEnvironment = null;
        public AppEnvironment AppEnvironment => _appEnvironment;

        private ReadFolderContent _readFolderContent = new();

        public void Initialize(AppEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public void OnEvent(AppEvent e)
        {
            if (e is FolderReadEvent fre)
            {
                List<FolderViewInfo> allReadable = _readFolderContent.ReadAllReadable(fre.FolderPath);
                AppEnvironment.Dispatcher.Dispatch(new FolderViewInfosEvent(allReadable));
            }
        }
    }
}