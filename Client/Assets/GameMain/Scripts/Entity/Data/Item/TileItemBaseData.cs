using System.Collections.Generic;

namespace Try2
{
    public abstract class TileItemBaseData
    {
        public TileItemBaseData()
        {

        }
        public abstract ItemActionList Actions { get; }
        public abstract Dictionary<PropType, int> Props { get; }
    }
}
