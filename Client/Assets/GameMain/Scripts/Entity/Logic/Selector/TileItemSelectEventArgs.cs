using GameFramework.Event;

namespace Try2
{
    public class TileItemSelectEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(TileItemSelectEventArgs).GetHashCode();

        private TileItemBase m_Item;
        private TileBase m_Tile;

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

        public TileBase Tile
        {
            get
            {
                return m_Tile;
            }
        }

        public static TileItemSelectEventArgs Create(TileItemBase item, TileBase tile)
        {
            TileItemSelectEventArgs args = new TileItemSelectEventArgs();
            args.m_Item = item;
            args.m_Tile = tile;
            return args;
        }

        public override void Clear()
        {
            m_Item = null;
            m_Tile = null;
        }
    }
}
