namespace UABS.Assets.Script.DataStruct
{
    public enum FolderViewType
    {
        Folder,
        Bundle
    }

    public struct FolderViewInfo
    {
        public FolderViewType type;
        public string name;
        public string path;
    }
}