
namespace Try2
{
    public partial class Utils
    {
        public class ItemActionUtils
        {
            public static IItemAction CreateTileAction(IItemActionData data)
            {
                IItemAction action;
                switch (data.Type)
                {
                    case ActionType.Attack: action = new AttackAction(); break;
                    case ActionType.Move: action = new MoveAction(); break;
                    case ActionType.Rest: action = new RestAction(); break;
                    case ActionType.Recruit: action = new RecruitAction(); break;
                    default: action = null; break;
                }

                if (action != null)
                    action.Initialize(data);
                return action;
            }

            public static IItemActionData CreateTileActioData(TileItemBase item, ActionType type)
            {
                IItemActionData data;
                switch (type)
                {
                    case ActionType.Attack: data = new AttackActionData(); break;
                    case ActionType.Move: data = new MoveActionData(); break;
                    case ActionType.Rest: data = new RestActionData(); break;
                    case ActionType.Recruit: data = new RecruitActionData(); break;
                    default: data = null; break;
                }

                if (data != null)
                    data.Initialize(item);
                return data;
            }
        }
    }
}
