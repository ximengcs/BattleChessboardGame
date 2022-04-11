using GameFramework.Event;
using UnityEngine;

namespace Try2
{
    public class MouseOnDragStartEventArgs : GameEventArgs
    {
        public static int EventId = typeof(MouseOnDragStartEventArgs).GetHashCode();

        private Vector2 m_Pos;

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public Vector2 StartPos
        {
            get
            {
                return m_Pos;
            }
        }

        public static MouseOnDragStartEventArgs Create(Vector2 pos)
        {
            MouseOnDragStartEventArgs args = new MouseOnDragStartEventArgs();
            args.m_Pos = pos;
            return args;
        }

        public override void Clear()
        {

        }
    }
}
