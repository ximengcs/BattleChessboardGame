using MeaponUnity.Core.Com;
using UnityEngine;

namespace Meapon.Input
{
    public partial class KeyboardInputHelper : IInputControllerHelper
    {
        public IInputResult Check(int inputCode)
        {
            switch (inputCode)
            {
                case Code.W:
                    if (UnityEngine.Input.GetKey(KeyCode.W))
                        return new InputResult(true, null);
                    break;
            }

            return new InputResult(false, null);
        }

        public void Update()
        {

        }
    }
}
