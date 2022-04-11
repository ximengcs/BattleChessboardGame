
using System;
using UnityEngine;

namespace MeaponUnity.Core.Com
{
    public class CharacterMovementData
    {
        private int m_PlatformMask;
        private int m_OneWayPlatformMask;
        private int m_InteractMask;
        private Action<RaycastHit2D> m_OnControllerCollidedEvent;
        private Action<Collider2D> m_OnTriggerEnterEvent;
        private Action<Collider2D> m_OnTriggerStayEvent;
        private Action<Collider2D> m_OnTriggerExitEvent;

        public CharacterMovementData(int platformMask, int oneWayPlatformMask, int interactMask, Action<RaycastHit2D> onControllerCollidedEvent, Action<Collider2D> onTriggerEnterEvent, Action<Collider2D> onTriggerStayEvent, Action<Collider2D> onTriggerExitEvent)
        {
            m_PlatformMask = platformMask;
            m_OneWayPlatformMask = oneWayPlatformMask;
            m_InteractMask = interactMask;
            m_OnControllerCollidedEvent = onControllerCollidedEvent;
            m_OnTriggerEnterEvent = onTriggerEnterEvent;
            m_OnTriggerStayEvent = onTriggerStayEvent;
            m_OnTriggerExitEvent = onTriggerExitEvent;
        }

        public int PlatformMask
        {
            get { return m_PlatformMask; }
        }

        public int OneWayPlatformMask
        {
            get { return m_OneWayPlatformMask; }
        }

        public int InteractMask
        {
            get { return m_InteractMask; }
        }

        public Action<RaycastHit2D> OnControllerCollidedEvent
        {
            get { return m_OnControllerCollidedEvent; }
        }

        public Action<Collider2D> OnTriggerEnterEvent
        {
            get { return m_OnTriggerEnterEvent; }
        }

        public Action<Collider2D> OnTriggerStayEvent
        {
            get { return m_OnTriggerStayEvent; }
        }

        public Action<Collider2D> OnTriggerExitEvent
        {
            get { return m_OnTriggerExitEvent; }
        }
    }
}
