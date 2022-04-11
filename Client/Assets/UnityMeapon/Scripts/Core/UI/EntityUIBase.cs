using UnityEngine;
using UnityGameFramework.Runtime;

namespace Try2
{
    public abstract class EntityUIBase
    {
        private Entity m_Entity;
        private GameObject m_UIGameObject;

        protected EntityUIBase(Entity entity, object userData)
        {
            m_Entity = entity;
            m_UIGameObject = new GameObject("UI");
            m_UIGameObject.transform.parent = m_Entity.transform;
        }

        public Entity Entity
        {
            get
            {
                return m_Entity;
            }
        }

        public GameObject UIEntity
        {
            get
            {
                return m_UIGameObject;
            }
        }
    }
}
