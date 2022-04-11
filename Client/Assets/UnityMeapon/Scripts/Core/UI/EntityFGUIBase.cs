
using FairyGUI;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Try2
{
    public abstract class EntityFGUIBase : EntityUIBase
    {
        private EntityFGUIBaseData m_Data;
        private UIPanel m_Panel;

        protected EntityFGUIBase(Entity entity, object userData) : base(entity, userData)
        {
            m_Data = (EntityFGUIBaseData)userData;
            m_Panel = UIEntity.GetOrAddComponent<UIPanel>();
            m_Panel.container.renderMode = RenderMode.WorldSpace;
            m_Panel.container.scale = Stage.inst.scale * GRoot.inst.scale * 3f;
            m_Panel.container.pixelPerfect = true;

            Vector3 pos = m_Panel.gameObject.transform.position;
            pos.z = Dream.Definition.Constant.Layer.EntityUI;
            SetPos(pos);

            GObject obj = UIPackage.CreateObject("Try2", m_Data.ComponentName);
            m_Panel.container.AddChild(obj.displayObject);

            OnInit(obj.asCom);
        }

        protected abstract void OnInit(GComponent view);

        public void SetPos(Vector2 pos)
        {
            m_Panel.gameObject.transform.position = pos;
        }

        public virtual void Show(object userData)
        {

        }

        public virtual void Hide()
        {

        }
    }
}
