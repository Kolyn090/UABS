using AssetsTools.NET.Extra;
using UABS.Assets.Script.Dispatcher;
using UABS.Assets.Script.Event;

namespace UABS.Assets.Script.Reader
{
    public class BundleReader
    {
        private readonly EventDispatcher _dispatcher;
        public BundleReader(EventDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public BundleFileInstance ReadBundle(string path, AssetsManager am)
        {
            BundleFileInstance bunInst = am.LoadBundleFile(path, true);
            _dispatcher.Dispatch(new BundleReadEvent(bunInst, path));
            return bunInst;
        }
    }
}