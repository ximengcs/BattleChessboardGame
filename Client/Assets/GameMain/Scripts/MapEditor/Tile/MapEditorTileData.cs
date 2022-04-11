using System.Collections.Generic;
using UnityEngine;

namespace Try2
{
    public class MapEditorTileData : TileBaseData
    {
        private Vector2 m_Pos;
        private Vector2Int m_Index;
        private string m_ResName;

        public MapEditorTileData(string resName, Vector2 pos, Vector2Int index)
        {
            m_Pos = pos;
            m_Index = index;
            m_ResName = resName;
        }

        public string ResName
        { 
            get 
            { 
                return m_ResName; 
            } 
        }

        public override Vector2 Pos
        {
            get
            {
                return m_Pos;
            }
        }

        public override Vector2Int Index
        {
            get
            {
                return m_Index;
            }
        }

        public override Dictionary<PropType, int> Consume
        {
            get
            {
                return null;
            }
        }

    }
}
