using GameFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Try2
{
    public sealed class VarTileActionItemBase : Variable<TileActionItem>
    {
        public VarTileActionItemBase()
        {
        }

        /// <summary>
        /// 从 UnityEngine.Texture 到 UnityEngine.Texture 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarTileActionItemBase(TileActionItem value)
        {
            VarTileActionItemBase varValue = ReferencePool.Acquire<VarTileActionItemBase>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 UnityEngine.Texture 变量类到 UnityEngine.Texture 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator TileActionItem(VarTileActionItemBase value)
        {
            return value.Value;
        }
    }
}
