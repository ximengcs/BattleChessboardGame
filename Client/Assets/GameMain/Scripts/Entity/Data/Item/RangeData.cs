using System.Collections.Generic;
using UnityEngine;

namespace Try2
{
    public class RangeData : TileItemBaseData
    {
        private string m_ResName;
        public Vector2 m_Pos;

        public RangeData(string resName, Vector2 pos)
        {
            m_ResName = resName;
            m_Pos = pos;
        }

        public string ResName
        {
            get
            {
                return m_ResName;
            }
        }

        public Vector2 Pos
        {
            get
            {
                return m_Pos;
            }
        }

        public override ItemActionList Actions
        {
            get;
        }

        public override Dictionary<PropType, int> Props
        {
            get;
        }
    }
}
