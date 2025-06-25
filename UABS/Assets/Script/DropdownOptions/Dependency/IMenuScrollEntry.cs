using UABS.Assets.Script.Dispatcher;

namespace UABS.Assets.Script.DropdownOptions.Dependency
{
    public interface IMenuScrollEntry
    {
        public string ShortPath { get; set; }

        void AssignDispatcher(EventDispatcher dispatcher);
    }
}