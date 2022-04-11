using System;
using System.Collections.Generic;
using UnityEngine;

namespace Try2
{
    public class AttackCursorItemData : TileItemBaseData
    {
        private Vector2 m_Pos;

        public AttackCursorItemData(Vector2 pos) : base()
        {
            m_Pos = pos;
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
