using AssetsTools.NET.Extra;
using UABS.Assets.Script.Dispatcher;

namespace UABS.Assets.Script.Misc
{
    public class AppEnvironment
    {
        public EventDispatcher Dispatcher { get; } = new();
        public AssetsManager AssetsManager { get; } = new();
    }
}