using System;

namespace Meapon.Input
{
    public interface IInputManager
    {
        void AddInputHelper(IInputControllerHelper helper);
        void RemoveInputHelper(IInputControllerHelper helper);
        void RegisterOperateCallback(int opCode, Action<object> callback);
        void UnRegisterOperateCallback(int opCode, Action<object> callback);
        void RegisterOpInputCode(int opCode, int inputCode);
        void UnRegisterOpInputCode(int opCode, int inputCode);
    }
}