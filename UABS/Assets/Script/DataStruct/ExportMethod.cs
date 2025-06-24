namespace UABS.Assets.Script.DataStruct
{
    public enum ExportType
    {
        All,
        Filtered,
        Selected
    }
    public struct ExportMethod
    {
        public ExportType exportType;
        public string destination;
    }
}