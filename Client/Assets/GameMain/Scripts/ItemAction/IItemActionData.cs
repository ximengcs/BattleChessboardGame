
namespace Try2
{
    public interface IItemActionData
    {
        ActionType Type { get; }
        void Initialize(TileItemBase item);
    }
}
