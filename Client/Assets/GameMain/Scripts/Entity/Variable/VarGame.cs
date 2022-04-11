using GameFramework;

namespace Try2
{
    public sealed class VarGame : Variable<Game>
    {
        public VarGame()
        {
        }

        /// <summary>
        /// 从 UnityEngine.Texture 到 UnityEngine.Texture 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarGame(Game value)
        {
            VarGame varValue = ReferencePool.Acquire<VarGame>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 UnityEngine.Texture 变量类到 UnityEngine.Texture 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Game(VarGame value)
        {
            return value.Value;
        }
    }
}
