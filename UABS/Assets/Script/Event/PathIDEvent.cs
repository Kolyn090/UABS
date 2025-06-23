namespace UABS.Assets.Script.Event
{
    public class PathIDEvent : AppEvent // On focus item's path id. (Only applicable in AssetBundle view)
    {
        public long PathID { get; }

        public PathIDEvent(long pathID)
        {
            PathID = pathID;
        }
    }
}