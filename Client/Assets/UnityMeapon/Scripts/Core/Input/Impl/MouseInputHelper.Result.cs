using Meapon.Input;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace MeaponUnity.Core.Com
{
    public class InputResult : IInputResult
    {
        public bool IsSuccess { get; }

        public object Data { get; }

        public InputResult(bool isSuccess, object data)
        {
            IsSuccess = isSuccess;
            Data = data;
        }
    }

    public class MouseClickData
    {
        public Vector3 Pos;
        public Vector3 WorldPos;
        public EntityLogic Entity;

        public MouseClickData(Collider2D col, Vector3 pos, Vector3 worldPos)
        {
            Pos = pos;
            WorldPos = worldPos;
            Entity = col != null ? col.GetComponent<EntityLogic>() : null;
        }
    }

    public class MouseEntityData
    {
        public EntityLogic Entity;

        public MouseEntityData(EntityLogic entity)
        {
            Entity = entity;
        }
    }

    public class MouseDragData
    {
        public Vector3 LastPos;
        public Vector3 Pos;
        public Vector3 Power;
        public Vector3 LastWorldPos;
        public Vector3 WorldPos;
        public Vector3 WorldPower;

        public MouseDragData(Vector3 lastPos, Vector3 pos, Vector3 lastWorldPos, Vector3 worldPos)
        {
            LastPos = lastPos;
            Pos = pos;
            Power = Pos - lastPos;
            LastWorldPos = lastWorldPos;
            WorldPos = worldPos;
            Power = worldPos - lastWorldPos;
        }
    }
}
