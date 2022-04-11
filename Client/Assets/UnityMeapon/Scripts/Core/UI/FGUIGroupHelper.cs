using FairyGUI;
using UnityGameFramework.Runtime;

namespace MeaponUnity.Core.UI
{
    public class FGUIGroupHelper : UIGroupHelperBase
    {
        private GRoot m_Root;
        private int m_Depth = 0;

        private void Start()
        {
            Stage.inst.gameObject.transform.parent = transform;
        }

        /// <summary>
        /// 设置界面组深度。
        /// </summary>
        /// <param name="depth">界面组深度。</param>
        public override void SetDepth(int depth)
        {
            m_Depth = depth;
        }
    }
}
