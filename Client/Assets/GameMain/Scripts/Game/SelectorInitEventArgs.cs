using GameFramework.Event;
using System;

namespace Try2
{
    public class SelectorInitEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(SelectorInitEventArgs).GetHashCode();

        private Selector m_Selector;

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public Selector Selector
        {
            get
            {
                return m_Selector;
            }
        }

        public static SelectorInitEventArgs Create(Selector selector)
        {
            SelectorInitEventArgs args = new SelectorInitEventArgs();
            args.m_Selector = selector;
            return args;
        }

        public override void Clear()
        {

        }
    }
}
