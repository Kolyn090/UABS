using SFB;

namespace UABS.Assets.Script.Writer
{
    public class SfbManager
    {
        public string PickFolderSuggestion(string title = "Create Folder", string dir = "", string suggestion = "")
        {
            string suggestedPath = StandaloneFileBrowser.SaveFilePanel(
                title,
                dir,         // Base directory
                suggestion,  // Suggested folder name
                ""           // No extension
            );
            return @$"\\?\{suggestedPath}";
        }

        public string PickFolder(string title = "Select Folder", string dir = "")
        {
            var paths = StandaloneFileBrowser.OpenFolderPanel(title, dir, false);
            if (paths.Length > 0)
            {
                return @$"\\?\{paths[0]}";
            }
            return "";
        }
        
        public string PickFile(string title, string extension)
        {
            var paths = StandaloneFileBrowser.OpenFilePanel(title, "", extension, false);
            if (paths.Length > 0)
            {
                return @$"\\?\{paths[0]}";
            }
            return "";
        }
    }
}