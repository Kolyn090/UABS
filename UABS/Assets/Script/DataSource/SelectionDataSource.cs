using System.Collections.Generic;
using System.Linq;
using UABS.Assets.Script.Event;
using UABS.Assets.Script.EventListener;
using UABS.Assets.Script.Misc;
using UnityEngine;

namespace UABS.Assets.Script.DataSource
{
    public class SelectionDataSource : MonoBehaviour, IAppEventListener, IAppEnvironment
    {
        private AppEnvironment _appEnvironment = null;
        public AppEnvironment AppEnvironment => _appEnvironment;
        private long _lastPathID;
        private List<long> _currBunPathIDs;

        public void Initialize(AppEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public void OnEvent(AppEvent e)
        {
            if (e is AssetSelectionEvent ase)
            {
                _lastPathID = ase.PathID;
            }
            else if (e is AssetsDisplayInfoEvent adie)
            {
                _currBunPathIDs = adie.AssetsDisplayInfo.Select(x => x.assetTextInfo).Select(x => x.pathID).ToList();
            }
        }

        public void Prev()
        {
            int index = StayInRange(FindIndexOfLastPathID() - 1);
            AppEnvironment.Dispatcher.Dispatch(new AssetSelectionEvent(_currBunPathIDs[index], index, _currBunPathIDs.Count, true));
        }

        public void Next()
        {
            int index = StayInRange(FindIndexOfLastPathID() + 1);
            AppEnvironment.Dispatcher.Dispatch(new AssetSelectionEvent(_currBunPathIDs[index], index, _currBunPathIDs.Count, true));
        }

        private int FindIndexOfLastPathID()
        {
            if (_currBunPathIDs == null)
            {
                return -1;
            }
            for (int i = 0; i < _currBunPathIDs.Count; i++)
            {
                if (_currBunPathIDs[i] == _lastPathID)
                {
                    return i;
                }
            }
            return -1;
        }

        private int StayInRange(int input)
        {
            if (input < 0)
                return _currBunPathIDs.Count - 1;
            else if (input >= _currBunPathIDs.Count)
                return 0;
            return input;
        }
    }
}