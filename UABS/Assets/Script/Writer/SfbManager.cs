using SFB;

namespace UABS.Assets.Script.Writer
{
    public class SfbManager
    {
        public string PickFolder(string title="Select Folder", string dir="")
        {
            var paths = StandaloneFileBrowser.OpenFolderPanel(title, dir, false);
            if (paths.Length > 0)
            {
                return paths[0];
            }
            return "";
        }
        
        public string PickFile(string title, string extension)
        {
            var paths = StandaloneFileBrowser.OpenFilePanel(title, "", extension, false);
            if (paths.Length > 0)
            {
                return paths[0];
            }
            return "";
        }
    }
}