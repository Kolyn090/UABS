using System.Collections.Generic;
using System.IO;
using System.Linq;
using UABS.Assets.Script.Misc;

namespace UABS.Assets.Script.Reader
{
    public class ReadExternalCache
    {
        public List<string> GetCacheFoldersInExternal()
        {
            string[] allFolders = Directory.GetDirectories(PredefinedPaths.ExternalCache, "*", SearchOption.TopDirectoryOnly);
            return allFolders.ToList();
        }
    }
}