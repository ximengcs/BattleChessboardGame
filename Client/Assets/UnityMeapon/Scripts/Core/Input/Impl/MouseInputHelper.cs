using UnityEngine;
using UnityGameFramework.Runtime;
using Meapon.Input;
using System.Collections.Generic;
using FairyGUI;

namespace MeaponUnity.Core.Com
{
    public partial class MouseInputHelper : IInputControllerHelper
    {
        private InputBuffer m_Buffer;
        private MouseAreaCollection m_MouseAreaEntites;

        private Dictionary<int, IInputResult> m_CurrentInfos;

        public bool Active
        {
            get;
            set;
        }

        public Camera Camera
        {
            get;
            set;
        }

        public MouseInputHelper()
        {
            m_CurrentInfos = new Dictionary<int, IInputResult>(10);
            m_MouseAreaEntites = new MouseAreaCollection();
            Camera = Camera.main;
            Active = true;
        }

        public IInputResult Check(int inputCode)
        {
            if (m_CurrentInfos.ContainsKey(inputCode))
            {
                return m_CurrentInfos[inputCode];
            }
            return new InputResult(false, null);
        }

        public void Update()
        {
            m_CurrentInfos.Clear();
            m_Buffer.MouseLeftLastState = m_Buffer.MouseLeftState;
            if (Input.GetMouseButtonDown(0))
                m_Buffer.MouseLeftState = MousePressState.Down;
            else if (Input.GetMouseButton(0))
                m_Buffer.MouseLeftState = MousePressState.Press;
            else if (Input.GetMouseButtonUp(0))
                m_Buffer.MouseLeftState = MousePressState.Up;
            else
                m_Buffer.MouseLeftState = MousePressState.None;

            m_Buffer.LastPosition = m_Buffer.Position;
            m_Buffer.Position = Input.mousePosition;
            m_Buffer.LastWorldPosition = Camera.ScreenToWorldPoint(m_Buffer.LastPosition);
            m_Buffer.WorldPosition = Camera.ScreenToWorldPoint(m_Buffer.Position);
            m_Buffer.IsUIClick = Stage.isTouchOnUI;
            CheckCollider();
            switch (m_Buffer.MouseLeftState)
            {
                case MousePressState.None:
                    m_Buffer.State = MouseState.None;
                    m_Buffer.LeftPressTime = 0;
                    break;

                case MousePressState.Down:
                    CheckDownState();
                    break;

                case MousePressState.Press:
                    m_Buffer.LeftPressTime += Time.deltaTime;
                    CheckDragState();
                    break;

                case MousePressState.Up:
                    CheckClickState();
                    CheckDragState();
                    CheckUpState();
                    break;
            }

            CheckMouseEnterArea();
        }

        private void CheckCollider()
        {
            m_Buffer.EntityCollider = null;
            Collider2D col = Physics2D.OverlapPoint(m_Buffer.WorldPosition);
            if (col)
            {
                EntityLogic entity = col.GetComponent<EntityLogic>();
                object[] atr = entity.GetType().GetCustomAttributes(typeof(MouseInputAttribute), true);
                if (atr.Length > 0)
                {
                    m_Buffer.EntityCollider = col;
                }
            }
        }

        private void CheckDownState()
        {
            if (m_Buffer.IsUIClick)
                return;

            if (m_Buffer.MouseLeftLastState != MousePressState.None)
                return;

            m_Buffer.State = MouseState.Click;
            m_CurrentInfos.Add(Code.OnLeftDown, new InputResult(true, new MouseClickData(m_Buffer.EntityCollider, m_Buffer.Position, m_Buffer.WorldPosition)));
            m_Buffer.MouseLeftStartPosition = m_Buffer.Position;
        }

        private void CheckClickState()
        {
            if (m_Buffer.MouseLeftState != MousePressState.Up)
                return;

            if (m_Buffer.State != MouseState.Click)
                return;

            CheckClickLeftButton();
        }

        private void CheckUpState()
        {
            if (m_Buffer.MouseLeftLastState != MousePressState.Press)
                return;

            m_CurrentInfos.Add(Code.OnLeftUp, new InputResult(true, new MouseClickData(m_Buffer.EntityCollider, m_Buffer.Position, m_Buffer.WorldPosition)));
        }

        private void CheckDragState()
        {
            if (m_Buffer.MouseLeftState != MousePressState.Press)
            {
                if (m_Buffer.State == MouseState.Drag)
                {
                    m_CurrentInfos.Add(Code.OnLeftDragEnd, new InputResult(true, new MouseClickData(m_Buffer.EntityCollider, m_Buffer.Position, m_Buffer.WorldPosition)));
                }
                return;
            }

            switch (m_Buffer.State)
            {
                case MouseState.Click:
                    if (m_Buffer.LeftPressTime >= InputBuffer.DragGap)
                    {
                        Vector2 deltaPos = m_Buffer.Position - m_Buffer.LastPosition;
                        if (Mathf.Abs(deltaPos.x) >= 1 || Mathf.Abs(deltaPos.y) >= 1)
                        {
                            m_CurrentInfos.Add(Code.OnLeftDragStart, new InputResult(true, new MouseClickData(m_Buffer.EntityCollider, m_Buffer.Position, m_Buffer.WorldPosition)));
                            m_Buffer.State = MouseState.Drag;
                        }
                    }

                    break;

                case MouseState.Drag:
                    MouseDragData data = new MouseDragData(m_Buffer.LastPosition, m_Buffer.Position, m_Buffer.LastWorldPosition, m_Buffer.WorldPosition);
                    m_CurrentInfos.Add(Code.OnLeftDraging, new InputResult(true, data));
                    break;
            }
        }

        private void CheckClickLeftButton()
        {
            if (m_Buffer.EntityCollider)
            {
                m_CurrentInfos.Add(Code.OnLeftClick, new InputResult(true, new MouseClickData(m_Buffer.EntityCollider, m_Buffer.Position, m_Buffer.WorldPosition)));
            }
        }

        private void CheckMouseEnterArea()
        {
            if (m_Buffer.EntityCollider)
            {
                EntityLogic entity = m_Buffer.EntityCollider.GetComponent<EntityLogic>();
                m_MouseAreaEntites.Add(entity);
            }

            m_MouseAreaEntites.Update(m_CurrentInfos);
        }
    }
}
