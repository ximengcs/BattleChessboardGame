using GameFramework;
using GameFramework.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Try2
{
    public class TileMapInitEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(TileMapInitEventArgs).GetHashCode();

        private TileMap m_Map;

        public override int Id
        {
            get { return EventId; }
        }

        public TileMap Map
        {
            get { return m_Map; }
        }

        public static TileMapInitEventArgs Create(TileMap map)
        {
            TileMapInitEventArgs args = ReferencePool.Acquire<TileMapInitEventArgs>();
            args.m_Map = map;
            return args;
        }

        public override void Clear()
        {
            m_Map = null;
        }
    }
}
