using System;

namespace Meapon.Input
{
    public interface IInputControllerHelper
    {
        void Update();
        IInputResult Check(int inputCode);
    }
}