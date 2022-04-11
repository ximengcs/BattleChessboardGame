using GameFramework;
using GameFramework.Event;

namespace Try2
{
    public class TileInitEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(TileInitEventArgs).GetHashCode();

        private TileBase m_Tile;

        public override int Id
        {
            get { return EventId; }
        }

        public TileBase Tile
        {
            get { return m_Tile; }
        }

        public static TileInitEventArgs Create(TileBase tile)
        {
            TileInitEventArgs args = ReferencePool.Acquire<TileInitEventArgs>();
            args.m_Tile = tile;
            return args;
        }

        public override void Clear()
        {
            m_Tile = null;
        }
    }
}
