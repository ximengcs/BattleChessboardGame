using System.Collections.Generic;
using Meapon.Input;
using UnityGameFramework.Runtime;

namespace MeaponUnity.Core.Com
{
    public partial class MouseInputHelper
    {
        private class MouseAreaCollection
        {
            private List<MouseAreaInfo> m_EnterTargets;

            public MouseAreaCollection()
            {
                m_EnterTargets = new List<MouseAreaInfo>();
            }

            public void Add(EntityLogic target)
            {
                MouseAreaInfo info = Get(target);
                if (info == null)
                    m_EnterTargets.Add(new MouseAreaInfo(target, MouseAreaState.ReadyIn));
                else
                {
                    info.State = MouseAreaState.In;
                }
            }

            public MouseAreaInfo Get(EntityLogic target)
            {
                foreach (MouseAreaInfo info in m_EnterTargets)
                {
                    if (info.Target == target)
                        return info;
                }
                return null;
            }

            public void Update(Dictionary<int, IInputResult> result)
            {
                for (int i = m_EnterTargets.Count - 1; i >= 0; i--)
                {
                    MouseAreaInfo info = m_EnterTargets[i];
                    if (info.State == MouseAreaState.ReadyOut)
                    {
                        result.Add(Code.OnLeaveArea, new InputResult(true, new MouseEntityData(info.Target)));
                        info.State = MouseAreaState.Out;
                        m_EnterTargets.RemoveAt(i);
                    }
                }

                for (int i = m_EnterTargets.Count - 1; i >= 0; i--)
                {
                    MouseAreaInfo info = m_EnterTargets[i];
                    switch (info.State)
                    {
                        case MouseAreaState.ReadyIn:
                            result.Add(Code.OnEnterArea, new InputResult(true, new MouseEntityData(info.Target)));
                            info.State = MouseAreaState.ReadyOut;
                            break;

                        case MouseAreaState.In:
                            info.State = MouseAreaState.ReadyOut;
                            break;
                    }
                }
            }
        }

        private class MouseAreaInfo
        {
            public EntityLogic Target;
            public MouseAreaState State;

            public MouseAreaInfo(EntityLogic target, MouseAreaState state)
            {
                Target = target;
                State = state;
            }
        }

        private enum MouseAreaState
        {
            ReadyIn,
            In,
            ReadyOut,
            Out
        }
    }
}
