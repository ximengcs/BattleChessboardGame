using GameFramework;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Try2
{
    public class GameAssets
    {
        public const string Name = nameof(GameAssets);

        private MapStruture m_MapTileStructure;
        private MapEntityStructure m_MapEntityStructure;

        public MapStruture MapTileStructure
        {
            get
            {
                return m_MapTileStructure;
            }
        }

        public MapEntityStructure MapEntityStructure
        {
            get
            {
                return m_MapEntityStructure;
            }
        }

        public void PutMapTileAssets(string jsonData)
        {
            m_MapTileStructure = JsonConvert.DeserializeObject<MapStruture>(jsonData);
        }

        public void PutMapItemAssets(string jsonData)
        {
            m_MapEntityStructure = JsonConvert.DeserializeObject<MapEntityStructure>(jsonData);
        }
    }

    public class MapStruture : Variable<string>
    {
        public int Width;
        public int Height;
        public List<MapItem> Data;

        public override void Clear()
        {
            base.Clear();
            Data = null;
        }
    }

    public class MapItem
    {
        public int ID;
    }

    public class MapEntityStructure : Variable<string>
    {
        public List<EntityDataItem> Data;
    }

    public class EntityDataItem
    {
        public int Id;
        public int X;
        public int Y;
    }
}
