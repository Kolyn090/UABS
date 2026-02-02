using System.Collections.Generic;
using System.IO;
using System.Text;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using UABS.Data;
using UABS.Service;
using UABS.Util;
using UABS.Wrapper;

namespace UABS.__Test__
{
    public static class ImageReaderTester
    {
        public const string TestBundlePath = @"\\?\C:\Program Files (x86)\Steam\steamapps\common\Otherworld Legends\Otherworld Legends_Data\StreamingAssets\aa\StandaloneWindows64\spriteassetgroup_assets_assets\needdynamicloadresources\spritereference\bartenddropicons.asset_c45e557ea71096bab67c3b66775dee8b.bundle";

        public static void Test()
        {
            AssetsManagerService assetsManagerService = new();
            AssetsManager assetsManager = assetsManagerService.AssetsManager;
            BcDecoderWrapper decoder = new();
            ImageReader imageReader = new(assetsManagerService, decoder);

            AssetEntry assetEntry = new();

            Log.Info("LoadClassPackage Loading...");
            var assembly = typeof(ClassDataLoader).Assembly;
            using var stream = assembly.GetManifestResourceStream(
                "Core.Script.Data.ClassData.classdata.tpk");
            if (stream == null)
                Log.Error("classdata.tpk not found", "FileWindow.cs");
            assetsManager.LoadClassPackage(stream);
            Log.Info("LoadClassPackage Complete");

            if (File.Exists(TestBundlePath))
            {
                Log.Info($"Success: file exists");
            }
            else
            {
                Log.Error($"Error: file doesn't exist");
            }

            var readResult = BundleReader.ReadFromPath(TestBundlePath, assetsManager);
            if (readResult.Item1 is not {} assetsInsts ||
                readResult.Item2 is not {} assetEntries)
            {
                Log.Error("Failed to unpack the bundle.");
                return;
            }

            Log.Info($"Length of assetEntries: {assetEntries.Count}");

            if (imageReader.SpriteToImage(assetEntries[0]) is not {} imageAssetEntry)
            {
                Log.Error("Failed to read imageAssetEntry.");
                return;
            }

            if (imageAssetEntry.Image is not {} image)
            {
                Log.Error("Failed to read image.");
                return;
            }

            if (image.RawImageBytes is not {} rawImageBytes)
            {
                Log.Error("Failed to read raw image bytes.");
                return;
            }

            Log.Info(image.ImagePixelFormat.ToString());

            SkiaPngWriter.SaveRgbaAsPng(
                rawImageBytes,
                image.Width,
                image.Height,
                "output.png",
                flipVertically: true
            );
        }

        // ! For debug
        public static string GetAssetTypeValueFieldString(AssetTypeValueField field, int indentLevel = 0)
        {
            if (field == null) return "<null>";

            StringBuilder sb = new();
            string indent = new(' ', indentLevel * 2);

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