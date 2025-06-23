namespace UABS.Assets.Script.Event
{
    public class PathIDEvent : AppEvent // On focus item's path id. (Only applicable in AssetBundle view)
    {
        public long PathID { get; }

        public int CurrIndex { get; }

        public int TotalNumOfAssets { get; }

        public PathIDEvent(long pathID, int currIndex = 0, int totalNumOfAssets = 0)
        {
            PathID = pathID;
            CurrIndex = currIndex;
            TotalNumOfAssets = totalNumOfAssets;
        }
    }
}