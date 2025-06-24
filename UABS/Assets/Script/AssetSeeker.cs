using System;
using UnityEngine;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;


public class AssetSeeker : MonoBehaviour
{
    public class SpriteInfo
    {
        public string Name { get; set; }
        public long PathId { get; set; }
    }

    public class Bundle
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string CABcode { get; set; }
        public List<SpriteInfo> SpriteInfos { get; set; }
    }

    private const string GameData = @"\\?\C:\Program Files (x86)\Steam\steamapps\common\Otherworld Legends\Otherworld Legends_Data\StreamingAssets\aa\StandaloneWindows64";
    private const string SurfCache = "External/UABS_Cache";
    private void Start()
    {
        // List<string> paths = SurfFilesUnderDirExcludeMeta("Assets/TestBundles");
        // foreach (string path in paths)
        // {
        //     Debug.Log(ReadCABCode(path));
        //     List<(long, string)> pathIDNameOfSprites = GetPathIDNameOfSpritesInBundle(path);
        //     foreach (var item in pathIDNameOfSprites)
        //     {
        //         Debug.Log(item);
        //     }
        // }

        if (!Directory.Exists(SurfCache))
        {
            Directory.CreateDirectory(SurfCache);
        }
        BuildCache(SurfCache);
    }

    private void BuildCache(string cacheFolder)
    {
        List<string> paths = SurfFoldersUnderDirExcludeMeta(GameData);
        foreach (string path in paths)
        {
            string basePath = path.Replace(GameData, "");
            string newPath = cacheFolder + "\\" + basePath;
            if (!Directory.Exists(newPath))
            {
                Debug.Log($"Creating path {newPath}");
                Directory.CreateDirectory(newPath);
            }
            List<Bundle> writeToIndex = new();
            List<string> filesInPath = SurfFilesUnderDirExcludeMetaTopDirOnly(path);
            // Create Bundle objects
            foreach (string fileInPath in filesInPath)
            {
                Bundle bundle = new();
                bundle.Path = fileInPath;
                bundle.Name = Path.GetFileName(fileInPath);
                bundle.CABcode = ReadCABCode(fileInPath);

                // --- Sprites ---
                // Debug.Log(fileInPath);
                bundle.SpriteInfos= GetSpriteInfoInBundle(fileInPath);
                writeToIndex.Add(bundle);
            }

            string indexFile = newPath + "\\index.json";
            File.WriteAllText(indexFile, JsonConvert.SerializeObject(writeToIndex, Formatting.Indented));
        }
    }

    private List<string> SurfFoldersUnderDirExcludeMeta(string directory)
    {
        List<string> result = new();
        string[] allFolders = Directory.GetDirectories(directory, "*", SearchOption.AllDirectories);

        foreach (string folder in allFolders)
        {
            result.Add(folder);
        }
        result.Add(directory);

        return result;
    }

    private List<string> SurfFilesUnderDirExcludeMeta(string directory)
    {
        List<string> result = new();
        string[] allFiles = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

        foreach (string file in allFiles)
        {
            if (file.EndsWith(".bundle"))
                result.Add(file);
        }

        return result;
    }

    private List<string> SurfFilesUnderDirExcludeMetaTopDirOnly(string directory)
    {
        List<string> result = new();
        string[] allFiles = Directory.GetFiles(directory, "*.*", SearchOption.TopDirectoryOnly);

        foreach (string file in allFiles)
        {
            if (file.EndsWith(".bundle"))
                result.Add(file);
        }

        return result;
    }

    private List<SpriteInfo> GetSpriteInfoInBundle(string bundlePath)
    {
        List<SpriteInfo> result = new();
        AssetsManager am = new();
        BundleFileInstance bunInst = am.LoadBundleFile(bundlePath, true);
        AssetsFileInstance fileInst = am.LoadAssetsFileFromBundle(bunInst, 0, false);

        AssetsFile afile = fileInst.file;
        List<AssetFileInfo> spriteAssets = afile.GetAssetsOfType(AssetClassID.Sprite);

        if (spriteAssets.Count == 0)
        {
            Debug.Log("No Sprite found.");
            return null;
        }

        foreach (AssetFileInfo targetAsset in spriteAssets)
        {
            AssetTypeValueField spriteBase = am.GetBaseField(fileInst, targetAsset);
            long pathId = long.Parse(targetAsset.PathId.ToString());
            string name = spriteBase["m_Name"].AsString;
            result.Add(new() {Name=name, PathId=pathId});
        }

        return result;
    }

    public static string ReadCABCode(string bundlePath)
    {
        using var fs = new FileStream(bundlePath, FileMode.Open, FileAccess.Read);
        using var br = new BinaryReader(fs);

        byte[] buffer = new byte[fs.Length];
        br.Read(buffer, 0, buffer.Length);

        byte[] cabPrefix = Encoding.ASCII.GetBytes("CAB-");

        for (int i = 0; i <= buffer.Length - cabPrefix.Length; i++)
        {
            bool match = true;
            for (int j = 0; j < cabPrefix.Length; j++)
            {
                if (buffer[i + j] != cabPrefix[j])
                {
                    match = false;
                    break;
                }
            }

            if (match)
            {
                int start = i;
                int end = start;

                // Read until whitespace or non-printable ASCII
                while (end < buffer.Length)
                {
                    byte b = buffer[end];
                    if (b < 0x21 || b > 0x7E) // non-printable ASCII
                        break;
                    end++;
                }

                string cab = Encoding.ASCII.GetString(buffer, start, end - start - 1);
                return cab;
            }
        }

        return "";
    }
}
