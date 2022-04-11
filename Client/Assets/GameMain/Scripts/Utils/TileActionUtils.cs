using Dream.DataTable;
using GameFramework.DataTable;
using MeaponUnity.Core.Entry;
using System.Collections.Generic;

namespace Try2
{
    public class TileActionUtils
    {
        public static bool CheckCanDo(ActionType type, TileItemBase item)
        {
            switch (type)
            {
                case ActionType.Move: return CheckCanMove(item);
                case ActionType.Attack: return CheckCanAttack(item);
                case ActionType.Rest: return CheckCanRest(item);
                case ActionType.Recruit: return CheckCanRecruit(item);
                default: return false;
            }
        }

        public static bool CheckCanMove(TileItemBase item)
        {
            return true;
        }

        public static bool CheckCanAttack(TileItemBase item)
        {
            List<TileBase> list = TileUtils.FindCurrentAttackPath(item);
            if (list == null || list.Count == 0)
                return false;

            foreach (TileBase tile in list)
            {
                if (tile.HasItem(item.Type))
                    return true;
            }

            return false;
        }

        public static bool CheckCanRest(TileItemBase item)
        {
            return true;
        }

        public static bool CheckCanRecruit(TileItemBase item)
        {
            return true;
        }

        public static DRAction GetDR(ActionType type)
        {
            IDataTable<DRAction> data = MeaponEntry.DataTable.GetDataTable<DRAction>();
            return data.GetDataRow((int)type);
        }
    }
}
