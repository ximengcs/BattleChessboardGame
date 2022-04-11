using GameFramework.Event;
using MeaponUnity.Core.Com;
using MeaponUnity.Core.Entry;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Try2
{
    public class CameraComponent : GameFrameworkComponent
    {
        private Camera m_Camera;

        public Camera Camera
        {
            get
            {
                return m_Camera;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_Camera = Camera.main;
        }

        public void MouseCanDrag()
        {
            MeaponEntry.CoreCom.InputController.Input.RegisterOpInputCode(OperateCode.DragMap, MouseInputHelper.Code.OnLeftDraging);
            MeaponEntry.CoreCom.InputController.Input.RegisterOperateCallback(OperateCode.DragMap, InternalDraging);
        }

        public void To(Vector2 pos)
        {
            Vector3 tmp = m_Camera.transform.position;
            m_Camera.transform.position = new Vector3(pos.x, pos.y, tmp.z);
        }

        public void ToVector(Vector3 vector)
        {
            if (Mathf.Approximately(vector.x, 0) && Mathf.Approximately(vector.y, 0) && Mathf.Approximately(vector.z, 0))
                return;

            m_Camera.transform.position -= vector;
        }

        private void InternalDraging(object data)
        {
            MouseDragData drag = (MouseDragData)data;
            if (drag != null)
            {
                ToVector(drag.Power);
            }
        }
    }
}
