using System.Collections.Generic;
using System.Windows.Input;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using UABS.Misc;
using UABS.Service;
using UABS.Util;
using UABS.ViewModel;

namespace UABS.Data
{
    public class AssetEntry : ObservableObject
    {
        public static readonly List<string> Alternative_Row_Background_Colors = new()
        {
            "#d6ffd7",
            "#FFFFFF"
        };

        public static readonly string Selected_Row_Background_Color = "#00FF00";

        public string Name { get; set; } = string.Empty;
        public AssetClassIDService ClassIDService { get; set; } = new(AssetClassID.@void);
        public long PathID { get; set; }
        public uint UnCompressedSize { get; set; }
        public uint CompressedSize { get; set; }
        public string OriginalPath { get; set; } = string.Empty;
        public string CachedPath { get; set; } = string.Empty;
        public long FileID { get; set; }
        public string Memo { get; set; } = string.Empty;
        public AssetFileInfo? AssetFileInfo { get; set; }
        public AssetsFileInstance? AssetsInst { get; set; }

        public AssetPreviewType PreviewType { get; protected set; } = AssetPreviewType.None;

        private string _rowBackground = "#FFFFFF";

        public string RowBackground
        {
            get => _rowBackground;
            set => SetProperty(ref _rowBackground, value);
        }

        public AssetEntry()
        {

        }

        public static TDerived ConvertToDerived<TDerived>(AssetEntry baseObj) 
            where TDerived : AssetEntry, new()
        {
            TDerived derived = new();
            foreach (var prop in typeof(AssetEntry).GetProperties())
            {
                if (prop.CanRead && prop.CanWrite)
                {
                    prop.SetValue(derived, prop.GetValue(baseObj));
                }
            }
            return derived;
        }
    }
}