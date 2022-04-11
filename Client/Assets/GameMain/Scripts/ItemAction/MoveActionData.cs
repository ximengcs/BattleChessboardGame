
namespace Try2
{
    public class MoveActionData : IItemActionData
    {
        private TileItemBase m_Item;
        private TileBase m_Tile;

        public ActionType Type
        {
            get
            {
                return ActionType.Move;
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

        public void Initialize(TileItemBase item)
        {
            m_Item = item;
            m_Tile = item.In;
        }
    }
}
