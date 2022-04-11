
using System.Collections.Generic;
using UnityEngine;

namespace Try2
{
    public abstract class TileBaseData
    {
        public abstract Vector2 Pos { get; }
        public abstract Vector2Int Index { get; }
        public abstract Dictionary<PropType, int> Consume { get; }
    }
}
