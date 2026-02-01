using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UABS.Util;
using UABS.Wrapper;

namespace UABS.Data
{
    // * Manages SpriteAtlas & Sprites in a bundle.
    // * Keep in mind that a bundle can have multiple SpriteAtlas files.
    public struct AtlasDumpProcessor
    {
        public readonly struct RenderDataKey
        {
            readonly uint firstData0;
            readonly uint firstData1;
            readonly uint firstData2;
            readonly uint firstData3;
            readonly long second;
            public RenderDataKey(uint firstData0, uint firstData1, uint firstData2, uint firstData3, long second)
            {
                this.firstData0 = firstData0;
                this.firstData1 = firstData1;
                this.firstData2 = firstData2;
                this.firstData3 = firstData3;
                this.second = second;
            }

            public bool Compare(RenderDataKey other)
            {
                return firstData0 == other.firstData0 &&
                    firstData1 == other.firstData1 &&
                    firstData2 == other.firstData2 &&
                    firstData3 == other.firstData3 &&
                    second == other.second;
            }

            public override string ToString()
            {
                return $"\"first\"[{firstData0}, {firstData1}, {firstData2}, {firstData3}], \"second\": {second}";
            }
        }

        public DumpInfo atlasDumpInfo;
        public List<DumpInfo> spriteDumpInfos;
        public readonly IJsonObject AtlasJson
        {
            get => atlasDumpInfo.dumpJson;
        }

        private readonly Dictionary<int, long> GetIndex2PathID()
        {
            if (AtlasJson.GetObject("m_PackedSprites") is not {} packedSprites)
            {
                Log.Warn("Field m_PackedSprites not found, return empty Dict.");
                return new();
            }

            if (packedSprites.GetArray("Array") is not {} array)
            {
                Log.Warn("Field Array not found, return empty Dict.");
                return new();
            }

            var result = new Dictionary<int, long>(array.Count);

            for (int i = 0; i < array.Count; i++)
            {
                if (array[i].GetLong("m_PathID") is not {} pathID)
                {
                    Log.Warn($"Field m_PathID not found at index {i}, continue.");
                    continue;
                }
                result[i] = pathID;
            }

            return result;
        }

        public readonly Dictionary<long, int> GetPathID2Index()
        {
            Dictionary<int, long> index2PathID = GetIndex2PathID();
            return index2PathID.ToDictionary(pair => pair.Value, pair => pair.Key);
        }

        public readonly Dictionary<int, RenderDataKey> GetIndex2RenderDataKey()
        {
            Dictionary<long, int> pathID2Index = GetPathID2Index();
            Dictionary<int, RenderDataKey> result = new();

            foreach (DumpInfo spriteDumpInfo in spriteDumpInfos)
            {
                IJsonObject spriteDumpJson = spriteDumpInfo.dumpJson;
                long spritePathID = spriteDumpInfo.pathID;

                if (spriteDumpJson.GetObject("m_RenderDataKey") is not {} rdk)
                {
                    Log.Warn("Field m_RenderDataKey not found, continue.");
                    continue;
                }

                if (rdk.GetObject("first") is not {} first)
                {
                    Log.Warn("Field first not found, continue.");
                    continue;
                }

                uint? _firstData0 = first.GetUInt("data[0]");
                uint? _firstData1 = first.GetUInt("data[1]");
                uint? _firstData2 = first.GetUInt("data[2]");
                uint? _firstData3 = first.GetUInt("data[3]");
                long? _second = rdk.GetLong("second");

                if (_firstData0 is null || 
                    _firstData1 is null || 
                    _firstData2 is null || 
                    _firstData3 is null ||
                    _second is null)
                {
                    Log.Warn("At least one of the RDK data is null, continue.");
                    continue;
                }

                uint firstData0 = _firstData0.Value;
                uint firstData1 = _firstData1.Value;
                uint firstData2 = _firstData2.Value;
                uint firstData3 = _firstData3.Value;
                long second = _second.Value;

                RenderDataKey renderDataKey = new(firstData0, firstData1, firstData2, firstData3, second);
                result[pathID2Index[spritePathID]] = renderDataKey;
            }

            return result;
        }

        public readonly int SearchIndexOfRenderDataKey(List<RenderDataKey> lst, RenderDataKey target)
        {
            int counter = 0;
            foreach (RenderDataKey rdk in lst)
            {
                if (rdk.Compare(target))
                {
                    return counter;
                }
                counter++;
            }
            return -1;
        }

        private static List<RenderDataKey> GetRenderDataKeysFromJObject(IJsonObject jObject)
        {
            if (jObject.GetObject("m_RenderDataMap") is not {} renderDataMap)
            {
                Log.Warn($"Field m_RenderDataMap not found, return empty List.");
                return new();
            }

            if (renderDataMap.GetArray("Array") is not {} array)
            {
                Log.Warn($"Field Array not found, return empty List.");
                return new();
            }

            // var renderDataMap = jObject["m_RenderDataMap"]["Array"];

            List<RenderDataKey> renderDataKeys = new();
            for (int i = 0; i < array.Count(); i++)
            {
                var rdk = array[i];

                if (rdk.GetObject("first") is not {} outerFirst)
                {
                    Log.Warn($"Field outerFirst not found at index {i}, continue;");
                    continue;
                }

                if (outerFirst.GetObject("first") is not {} innerFirst)
                {
                    Log.Warn($"Field innerFirst not found at index {i}, continue;");
                    continue;
                }
                
                uint? _firstData0 = innerFirst.GetUInt("data[0]");
                uint? _firstData1 = innerFirst.GetUInt("data[1]");
                uint? _firstData2 = innerFirst.GetUInt("data[2]");
                uint? _firstData3 = innerFirst.GetUInt("data[3]");
                long? _second = outerFirst.GetLong("second");

                if (_firstData0 is null || 
                    _firstData1 is null || 
                    _firstData2 is null || 
                    _firstData3 is null ||
                    _second is null)
                {
                    Log.Warn("At least one of the RDK data is null, continue.");
                    continue;
                }

                uint firstData0 = _firstData0.Value;
                uint firstData1 = _firstData1.Value;
                uint firstData2 = _firstData2.Value;
                uint firstData3 = _firstData3.Value;
                long second = _second.Value;

                RenderDataKey renderDataKey = new(firstData0, firstData1, firstData2, firstData3, second);
                renderDataKeys.Add(renderDataKey);
            }
            return renderDataKeys;
        }

        public readonly Dictionary<int, int> GetIndex2ActualRenderDataKeyIndex()
        {
            Dictionary<int, int> result = new();
            Dictionary<int, RenderDataKey> index2RenderDataKey = GetIndex2RenderDataKey();
            List<RenderDataKey> renderDataKeys = GetRenderDataKeysFromJObject(AtlasJson);

            foreach (int index in index2RenderDataKey.Keys)
            {
                if (index2RenderDataKey.TryGetValue(index, out RenderDataKey rdk))
                {
                    result[index] = SearchIndexOfRenderDataKey(renderDataKeys, rdk);
                }
            }

            return result;
        }

        public ImageRect? GetRectAtActualIndex(int actualIndex)
        {
            var renderDataMap = AtlasJson.GetObject("m_RenderDataMap");
            if (renderDataMap is null) 
            { 
                Log.Warn("Field m_RenderDataMap not found, return null."); 
                return null; 
            }

            var array = renderDataMap.GetArray("Array");
            if (array is null) 
            { 
                Log.Warn("Field Array not found, return null."); 
                return null; 
            }

            if ((uint)actualIndex >= (uint)array.Count)
            {
                Log.Warn($"Index {actualIndex} out of range, return null.");
                return null;
            }

            var second = array[actualIndex].GetObject("second");
            if (second is null) 
            { 
                Log.Warn("Field Missing second not found, return null."); 
                return null; 
            }

            var textureRect = second.GetObject("textureRect");
            if (textureRect is null) 
            { 
                Log.Warn("Field Missing textureRect not found, return null."); 
                return null; 
            }

            if (textureRect.GetFloat("x")     is not float x ||
                textureRect.GetFloat("y")     is not float y ||
                textureRect.GetFloat("width") is not float w ||
                textureRect.GetFloat("height")is not float h)
            {
                Log.Warn("Invalid textureRect values, return null.");
                return null;
            }

            return new ImageRect(x, y, w, h);
        }

        public static List<AtlasDumpProcessor> DistributeProcessors(List<DumpInfo> atlasDumpInfos,
                                                                    List<DumpInfo> spriteDumpInfos)
        {
            List<AtlasDumpProcessor> result = new();
            foreach (DumpInfo atlasDumpInfo in atlasDumpInfos)
            {
                long atlasPathID = atlasDumpInfo.pathID;
                List<DumpInfo> spriteDumpInfosInAtlas = new();
                foreach (DumpInfo spriteDumpInfo in spriteDumpInfos)
                {
                    IJsonObject spriteJson = spriteDumpInfo.dumpJson;

                    if (spriteJson.GetObject("m_SpriteAtlas") is not {} spriteAtlas)
                    {
                        Log.Warn("Field m_SpriteAtlas not found, continue");
                        continue;
                    }

                    if (spriteAtlas.GetLong("m_PathID") is not {} pathID)
                    {
                        Log.Warn("Field m_PathID not found, continue");
                        continue;
                    }
                    
                    long atlasPathIDInSprite = pathID;
                    // long atlasPathIDInSprite = long.Parse(spriteJson["m_SpriteAtlas"]["m_PathID"].ToString());
                    if (atlasPathIDInSprite == atlasPathID)
                    {
                        spriteDumpInfosInAtlas.Add(spriteDumpInfo);
                    }
                }
                result.Add(new()
                {
                    atlasDumpInfo = atlasDumpInfo,
                    spriteDumpInfos = spriteDumpInfosInAtlas
                });
            }

            return result;
        }
    }
}