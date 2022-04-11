
using System.Collections.Generic;
using UnityEngine;

namespace Try2
{
    public class MapEditorTileWaitItemData : TileItemBaseData
    {
        private int m_TileId;
        private string m_ResName;

        public MapEditorTileWaitItemData(int tileId, string resName)
        {
            m_TileId = tileId;
            m_ResName = resName;
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
