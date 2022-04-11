//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine.U2D;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// UnityEngine.Texture 变量类。
    /// </summary>
    public sealed class VarSpriteAtlas : Variable<SpriteAtlas>
    {
        /// <summary>
        /// 初始化 UnityEngine.Texture 变量类的新实例。
        /// </summary>
        public VarSpriteAtlas()
        {
        }

        /// <summary>
        /// 从 UnityEngine.Texture 到 UnityEngine.Texture 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarSpriteAtlas(SpriteAtlas value)
        {
            VarSpriteAtlas varValue = ReferencePool.Acquire<VarSpriteAtlas>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 UnityEngine.Texture 变量类到 UnityEngine.Texture 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator SpriteAtlas(VarSpriteAtlas value)
        {
            return value.Value;
        }
    }
}
