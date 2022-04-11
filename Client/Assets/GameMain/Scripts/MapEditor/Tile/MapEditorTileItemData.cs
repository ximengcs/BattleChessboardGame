using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Try2
{
    public class MapEditorTileItemData : TileItemBaseData
    {
        private string m_ResName;
        private int m_TileId;

        public MapEditorTileItemData(int tileId, string resName)
        {
            m_ResName = resName;
            m_TileId = tileId;
        }

        public int TileId
        {
            get
            {
                return m_TileId;
            }
        }

        public string ResName
        {
            get
            {
                return m_ResName;
            }
        }

        public override ItemActionList Actions
        {
            get
            {
                return null;
            }
        }

        public override Dictionary<PropType, int> Props
        {
            get
            {
                return null;
            }
        }
    }
}
