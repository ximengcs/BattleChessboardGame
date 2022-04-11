using GameFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Try2
{
    public sealed class VarTileItemsBase : Variable<List<TileItemBase>>
    {
        public VarTileItemsBase()
        {
        }

        /// <summary>
        /// 从 UnityEngine.Texture 到 UnityEngine.Texture 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarTileItemsBase(List<TileItemBase> value)
        {
            VarTileItemsBase varValue = ReferencePool.Acquire<VarTileItemsBase>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 UnityEngine.Texture 变量类到 UnityEngine.Texture 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator List<TileItemBase>(VarTileItemsBase value)
        {
            return value.Value;
        }
    }
}
