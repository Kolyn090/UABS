using UnityEngine;

namespace UABS.Assets.Script.DataStruct
{
    public struct Texture2DWithMeta
    {
        public Texture2D texture2D;
        public Rect rect;
        public TextureFormat compressionFormat;
    }
}