using MeaponUnity.Core.Com;
using UnityGameFramework.Runtime;

namespace Meapon.Input
{
    public class InputManager : GameFrameworkComponent
    {
        private InputManagerBase m_InputManager;

        public IInputManager Input
        {
            get
            {
                return m_InputManager;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            m_InputManager = new InputManagerBase();
            m_InputManager.AddInputHelper(new MouseInputHelper());
        }

        private void Update()
        {
            m_InputManager.Update();
        }
    }
}
