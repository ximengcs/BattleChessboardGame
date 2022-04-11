using GameFramework.Event;
using UnityEngine;

namespace Try2
{
    public class MouseOnDragEndEventArgs : GameEventArgs
    {
        public static int EventId = typeof(MouseOnDragEndEventArgs).GetHashCode();

        private Vector2 m_Pos;

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public Vector2 EndPos
        {
            get
            {
                return m_Pos;
            }
        }

        public static MouseOnDragEndEventArgs Create(Vector2 pos)
        {
            MouseOnDragEndEventArgs args = new MouseOnDragEndEventArgs();
            args.m_Pos = pos;
            return args;
        }

        public override void Clear()
        {

        }
    }
}
