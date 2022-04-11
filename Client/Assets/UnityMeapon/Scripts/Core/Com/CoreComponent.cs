using Meapon.Input;
using Try2;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace MeaponUnity.Core.Com
{
    public class CoreComponent : GameFrameworkComponent
    {
        private InputManager m_InputController;
        public CameraComponent Camera
        {
            get;
            private set;
        }

        public InputManager InputController
        {
            get { return m_InputController; }
        }

        protected override void Awake()
        {
            base.Awake();
            m_InputController = gameObject.AddComponent<InputManager>();
            Camera = gameObject.AddComponent<CameraComponent>();
        }
    }
}
