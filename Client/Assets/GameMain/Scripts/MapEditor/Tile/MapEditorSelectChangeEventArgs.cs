using GameFramework.Event;

namespace Try2
{
    public class MapEditorSelectChangeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(MapEditorSelectChangeEventArgs).GetHashCode();

        private int m_TileId;

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int TileId
        {
            get
            {
                return m_TileId;
            }
        }

        public static MapEditorSelectChangeEventArgs Create(int tileId)
        {
            MapEditorSelectChangeEventArgs args = new MapEditorSelectChangeEventArgs();
            args.m_TileId = tileId;
            return args;
        }

        public override void Clear()
        {

        }
    }

}
