using GameFramework.Event;
using UnityEngine;

namespace Try2
{
    public class MouseOnClickEventArgs : GameEventArgs
    {
        public static int EventId = typeof(MouseOnClickEventArgs).GetHashCode();

        private Vector2 m_Pos;

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public Vector2 MousePos
        {
            get
            {
                return m_Pos;
            }
        }

        public static MouseOnClickEventArgs Create(Vector2 pos)
        {
            MouseOnClickEventArgs args = new MouseOnClickEventArgs();
            args.m_Pos = pos;
            return args;
        }

        public override void Clear()
        {

        }
    }
}
