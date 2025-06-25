using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using Newtonsoft.Json.Linq;
using UABS.Assets.Script.DataStruct;
using UnityEngine;

namespace UABS.Assets.Script.Reader
{
    public class ReadDependencyInfo
    {
        private AssetsManager _assetsManager;
        public ReadDependencyInfo(AssetsManager assetsManager)
        {
            _assetsManager = assetsManager;
        }

        public List<DependencyInfo> ReadInfoFor(BundleFileInstance bunInst, string fromCache)
        {
            List<DependencyInfo> result = new();
            AssetsFileInstance fileInst = _assetsManager.LoadAssetsFileFromBundle(bunInst, 0, false);
            List<AssetFileInfo> assets = fileInst.file.GetAssetsOfType(AssetClassID.AssetBundle);
            AssetFileInfo assetBundleInfo = assets[0];
            // Get the base field of the AssetBundle
            AssetTypeValueField baseField = _assetsManager.GetBaseField(fileInst, assetBundleInfo);

            // Navigate to m_Dependencies array
            AssetTypeValueField dependenciesArray = baseField["m_Dependencies"][0];

            int depCount = dependenciesArray.Children.Count;
            Debug.Log($"Number of dependencies: {depCount}");
            for (int i = 0; i < depCount; i++)
            {
                string dependencyCabCode = dependenciesArray[i].Value.AsString;
                Debug.Log($"Dependency {i}: {dependencyCabCode}");

                DependencyInfo? _dependencyInfo = SearchInCache(fromCache, dependencyCabCode);
                if (_dependencyInfo != null)
                {
                    DependencyInfo dependencyInfo = (DependencyInfo)_dependencyInfo;
                    result.Add(dependencyInfo);
                }
            }

            return result;
        }

        private DependencyInfo? SearchInCache(string cachePath, string cabCode)
        {
            string[] jsonFiles = Directory.GetFiles(cachePath, "*.json", SearchOption.AllDirectories);
            foreach (string filePath in jsonFiles)
            {
                try
                {
                    JArray arr = JArray.Parse(File.ReadAllText(filePath));
                    foreach (var item in arr)
                    {
                        if (item["CabCode"].ToString().ToLower() == cabCode.ToLower())
                        {
                            return new()
                            {
                                name = item["Name"].ToString(),
                                path = item["Path"].ToString(),
                                cabCode = cabCode
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to read {filePath}: {ex.Message}");
                }
            }
            return null;
        }
        
        public static string GetAssetTypeValueFieldString(AssetTypeValueField field, int indentLevel = 0)
        {
            if (field == null) return "<null>";

            StringBuilder sb = new StringBuilder();
            string indent = new string(' ', indentLevel * 2);

            // Field name and type
            sb.Append(indent);
            sb.Append(field.FieldName);
            sb.Append(" (");
            sb.Append(field.TypeName);
            sb.Append(")");

            // Field value (if any)
            if (field.Value != null)
            {
                sb.Append(" : ");
                sb.Append(field.Value);
            }
            sb.AppendLine();

            // Recursively append children
            if (field.Children != null)
            {
                foreach (var child in field.Children)
                {
                    sb.Append(GetAssetTypeValueFieldString(child, indentLevel + 1));
                }
            }

            return sb.ToString();
        }
    }
}