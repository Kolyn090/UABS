using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UABS.Assets.Script.Reader
{
    public class ReadExternalCache
    {
        private const string ExternalCache = "External/UABS_Cache";

        public List<string> GetCacheFoldersInExternal()
        {
            string[] allFolders = Directory.GetDirectories(ExternalCache, "*", SearchOption.TopDirectoryOnly);
            return allFolders.ToList();
        }
    }
}