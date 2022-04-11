using GameFramework.Fsm;
using MeaponUnity.Core.Entry;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Try2
{
    public abstract partial class TileItemBase
    {
        private ItemState m_State;
        private TileBase m_InTile;
        private TileItemBaseData m_Data;
        private GameObject m_GameObject;

        public abstract TileItemType Type { get; }

        public TileItemBaseData Data
        {
            get
            {
                return m_Data;
            }
        }

        public TileBase In
        {
            get
            {
                return m_InTile;
            }
        }

        public ItemState State
        {
            get
            {
                return m_State;
            }
        }

        public GameObject Object
        {
            get
            {
                return m_GameObject;
            }
        }

        #region 接口
        public virtual void Attack(TileItemBase other)
        {

        }

        public virtual void Complete()
        {
            Log.Debug("TileItem Complete ", Log.GR, "TileItem");
            m_State = ItemState.Complete;
        }
        #endregion

        #region 生命周期
        public virtual void OnInit(GameObject gameObject, TileBase tile, object userData)
        {
            m_Data = (TileItemBaseData)userData;
            m_InTile = tile;
            m_GameObject = gameObject;
            m_State = ItemState.Free;
        }

        public virtual void OnShow()
        {
            m_GameObject.transform.position = m_InTile.Data.Pos;
        }

        public virtual void OnSelect()
        {
            Log.Debug("Select " + m_GameObject.transform.position, Log.CY, "TileBaseItem");
            MeaponEntry.Event.FireNow(TileItemSelectEventArgs.EventId, TileItemSelectEventArgs.Create(this, m_InTile));
        }

        public virtual void OnHide()
        {

        }

        public virtual void OnUpdate()
        {

        }

        public virtual void OnEnterTile(TileBase tile)
        {
            m_InTile = tile;
        }

        public virtual void OnLeaveTile(TileBase tile)
        {
            m_InTile = null;
        }

        public virtual void OnRecycle()
        {

        }
        #endregion
    }
}
