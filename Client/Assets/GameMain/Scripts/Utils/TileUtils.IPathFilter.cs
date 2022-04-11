using System.Collections.Generic;
using UnityEngine;

namespace Try2
{
    public partial class TileUtils
    {
        private interface IPathFilter
        {
            bool Can(TileItemBase item, TileBase to);
        }

        private interface IPathListFilter
        {
            bool Can(TileItemBase item, TileBase to, int suplus);
        }

        private class CampPathFilter : IPathFilter
        {
            public bool Can(TileItemBase item, TileBase to)
            {
                if (item.In != to)
                {
                    List<TileItemBase> list = to.GetItem(TileItemType.Character);
                    if (list != null && list.Count > 0)
                        return false;
                }
                return true;
            }
        }

        private class MinMaxAttackRangeHelper : IPathListFilter
        {
            public bool Can(TileItemBase item, TileBase to, int suplus)
            {
                int min, max;
                if (!item.Data.Props.TryGetValue(PropType.MinAttackRange, out min))
                {
                    return false;
                }
                if (!item.Data.Props.TryGetValue(PropType.MaxAttackRange, out max))
                {
                    return false;
                }

                return suplus <= max - min;
            }
        }
    }
}
