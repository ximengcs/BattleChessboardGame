using UnityEngine;

namespace MeaponUnity.Core.Com
{
    public partial class MouseInputHelper
    {
        private struct InputBuffer
        {
            public const float DragGap = 0.1f;  //拖拽阈值

            public MouseState State;

            public MousePressState MouseLeftState;
            public MousePressState MouseLeftLastState;
            public Vector3 MouseLeftStartPosition;
            public float LeftPressTime;

            public bool IsUIClick;

            public Vector3 Position;
            public Vector3 LastPosition;
            public Vector3 WorldPosition;
            public Vector3 LastWorldPosition;

            public Collider2D EntityCollider;
        }

        private enum MouseState
        {
            None,
            Click,
            Drag
        }

        private enum MousePressState
        {
            None,
            Down,
            Press,
            Up
        }
    }
}
