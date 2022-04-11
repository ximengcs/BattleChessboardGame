using GameFramework;
using GameFramework.Event;

namespace Try2
{
    public class TileOnClickEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(TileOnClickEventArgs).GetHashCode();

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

        public static TileOnClickEventArgs Create(TileBase tile)
        {
            TileOnClickEventArgs args = ReferencePool.Acquire<TileOnClickEventArgs>();
            args.m_Tile = tile;
            return args;
        }

        public override void Clear()
        {
            m_Tile = null;
        }
    }
}
