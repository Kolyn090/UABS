namespace UABS.Assets.Script.DropdownOptions.Dependency
{
    public interface IMenuScrollEntry
    {
        public string ShortPath { get; set; }

        void AssignText(string newText);
    }
}