using GameFramework.Event;
using MeaponUnity.Core.Entry;
using UnityEngine;

namespace Try2
{
    public class MouseOnDragingEventArgs : GameEventArgs
    {
        public static int EventId = typeof(MouseOnDragingEventArgs).GetHashCode();

        private Vector2 m_Pos;
        private Vector2 m_Vector;

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

        public Vector2 MouseVector
        {
            get
            {
                return m_Vector;
            }
        }

        public static MouseOnDragingEventArgs Create(Vector2 lastPos, Vector2 currentPos)
        {
            MouseOnDragingEventArgs args = new MouseOnDragingEventArgs();
            args.m_Pos = currentPos;
            args.m_Vector = currentPos - lastPos;
            return args;
        }

        public override void Clear()
        {

        }
    }
}
