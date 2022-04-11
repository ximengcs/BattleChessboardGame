using System.Collections.Generic;

namespace Try2
{
    public partial class TileUtils
    {
        private interface IPropConsumeHelper
        {
            int Comsume(TileBase tile, TileItemBase item, int srcValue);
        }

        private static IPropConsumeHelper Get(PropType prop)
        {
            switch(prop)
            {
                case PropType.MovePower: return new PropMoveHelper();
                default: return null;
            }
        }

        private class PropMoveHelper : IPropConsumeHelper
        {
            public int Comsume(TileBase tile, TileItemBase item, int srcValue)
            {
                Dictionary<PropType, int> props = tile.Data.Consume;
                if (props.TryGetValue(PropType.MovePower, out int require))
                    return srcValue - require;
                else
                    return 0;
            }
        }

        private class PropMoveOneGridHelper : IPropConsumeHelper
        {
            public int Comsume(TileBase tile, TileItemBase item, int srcValue)
            {
                return srcValue - 1;
            }
        }
    }
}
