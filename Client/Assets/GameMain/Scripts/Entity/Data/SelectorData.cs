
namespace Try2
{
    public class SelectorData
    {
        private TileBase m_Tile;
        private string m_ResName;

        public SelectorData(TileBase tile, string resName)
        {
            m_Tile = tile;
            m_ResName = resName;
        }

        public TileBase Tile
        {
            get
            {
                return m_Tile;
            }
        }

        public string ResName
        {
            get
            {
                return m_ResName;
            }
        }
    }
}
