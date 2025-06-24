using AssetsTools.NET.Extra;
using UABS.Assets.Script.Dispatcher;
using UABS.Assets.Script.Event;
using UABS.Assets.Script.Misc;

namespace UABS.Assets.Script.Reader
{
    public class BundleReader
    {
        private readonly AppEnvironment _appEnvironment;

        public BundleReader( AppEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public BundleFileInstance ReadBundle(string path)
        {
            BundleFileInstance bunInst = _appEnvironment.AssetsManager.LoadBundleFile(path, true);
            _appEnvironment.Dispatcher.Dispatch(new BundleReadEvent(bunInst, path));
            return bunInst;
        }
    }
}