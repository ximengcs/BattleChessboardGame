using GameFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Try2
{
    public sealed class VarGameAssets : Variable<GameAssets>
    {
        public VarGameAssets()
        {
        }

        /// <summary>
        /// 从 UnityEngine.Texture 到 UnityEngine.Texture 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarGameAssets(GameAssets value)
        {
            VarGameAssets varValue = ReferencePool.Acquire<VarGameAssets>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 UnityEngine.Texture 变量类到 UnityEngine.Texture 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator GameAssets(VarGameAssets value)
        {
            return value.Value;
        }
    }
}
