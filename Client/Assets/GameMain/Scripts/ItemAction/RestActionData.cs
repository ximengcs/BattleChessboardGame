
namespace Try2
{
    public class RestActionData : IItemActionData
    {
        private TileItemBase m_Item;

        public ActionType Type
        {
            get
            {
                return ActionType.Rest;
            }
        }

        public TileItemBase Item
        {
            get
            {
                return m_Item;
            }
        }

        public void Initialize(TileItemBase item)
        {
            m_Item = item;
        }
    }
}
