using GameFramework.Event;

namespace Try2
{
    public class TileSelectedEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(TileSelectedEventArgs).GetHashCode();

        private TileBase m_Tile;

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public TileBase Tile
        {
            get
            {
                return m_Tile;
            }
        }

        public static TileSelectedEventArgs Create(TileBase tile)
        {
            TileSelectedEventArgs args = new TileSelectedEventArgs();
            args.m_Tile = tile;
            return args;
        }

        public override void Clear()
        {

        }
    }
}
