using GameFramework.Event;

namespace Try2
{
    public class TileItemStartActionEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(TileItemStartActionEventArgs).GetHashCode();

        private TileItemBase m_Item;

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public TileItemBase Item
        {
            get
            {
                return m_Item;
            }
        }

        public static TileItemStartActionEventArgs Create(TileItemBase item)
        {
            TileItemStartActionEventArgs args = new TileItemStartActionEventArgs();
            args.m_Item = item;
            return args;
        }

        public override void Clear()
        {

        }
    }
}
