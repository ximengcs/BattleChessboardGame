using Dream.DataTable;
using System.Collections.Generic;
using UnityEngine;

namespace Try2
{
    public class CommonTileData : TileBaseData
    {
        private Vector2 m_Pos;
        private Vector2Int m_Index;
        private DRTile m_DRData;
        private Dictionary<PropType, int> m_Consume;

        public CommonTileData(DRTile data, int x, int y)
        {
            m_DRData = data;
            m_Index = new Vector2Int(x, y);
            m_Pos = m_Index;

            m_Consume = new Dictionary<PropType, int>(data.Consume.Count);
            foreach (KeyValuePair<int, int> item in data.Consume)
            {
                m_Consume[(PropType)item.Key] = item.Value;
            }
        }

        public override Dictionary<PropType, int> Consume
        {
            get
            {
                return m_Consume;
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

        public DRTile DRData
        {
            get
            {
                return m_DRData;
            }
        }
    }
}
