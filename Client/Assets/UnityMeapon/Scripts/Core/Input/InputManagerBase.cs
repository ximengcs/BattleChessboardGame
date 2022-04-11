using System;
using System.Collections.Generic;
using UnityEngine;

namespace Meapon.Input
{
    public partial class InputManagerBase : IInputManager
    {
        private Dictionary<int, OperateInfo> m_Inputs;
        private List<IInputControllerHelper> m_InputHelper;

        public InputManagerBase()
        {
            m_Inputs = new Dictionary<int, OperateInfo>();
            m_InputHelper = new List<IInputControllerHelper>();
        }

        public void AddInputHelper(IInputControllerHelper helper)
        {
            m_InputHelper.Add(helper);
        }

        public void RemoveInputHelper(IInputControllerHelper helper)
        {
            m_InputHelper.Remove(helper);
        }

        public void RegisterOperateCallback(int opCode, Action<object> callback)
        {
            OperateInfo info;
            if (!m_Inputs.TryGetValue(opCode, out info))
            {
                info = new OperateInfo(opCode);
                m_Inputs[opCode] = info;
            }
            info.Callback += callback;
        }

        public void UnRegisterOperateCallback(int opCode, Action<object> callback)
        {
            OperateInfo info;
            if (!m_Inputs.TryGetValue(opCode, out info))
            {
                return;
            }
            info.Callback -= callback;
        }

        public void RegisterOpInputCode(int opCode, int inputCode)
        {
            OperateInfo info;
            if (!m_Inputs.TryGetValue(opCode, out info))
            {
                info = new OperateInfo(opCode);
                info.InputCode = new List<int>();
                m_Inputs[opCode] = info;
            }

            info.InputCode.Add(inputCode);
        }

        public void UnRegisterOpInputCode(int opCode, int inputCode)
        {
            OperateInfo info;
            if (!m_Inputs.TryGetValue(opCode, out info))
            {
                return;
            }
            info.InputCode.Remove(inputCode);
        }

        public void Update()
        {
            foreach (IInputControllerHelper helper in m_InputHelper)
            {
                helper.Update();
            }

            foreach (KeyValuePair<int, OperateInfo> item in m_Inputs)
            {
                foreach (IInputControllerHelper helper in m_InputHelper)
                {
                    foreach (int inputCode in item.Value.InputCode)
                    {
                        IInputResult result = helper.Check(inputCode);
                        if (result.IsSuccess)
                        {
                            item.Value.Callback?.Invoke(result.Data);
                        }
                    }
                }
            }
        }
    }
}
