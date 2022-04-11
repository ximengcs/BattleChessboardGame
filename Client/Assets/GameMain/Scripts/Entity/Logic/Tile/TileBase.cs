using GameFramework.Event;
using MeaponUnity.Core.Com;
using MeaponUnity.Core.Entry;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Try2
{
    /// <summary>
    /// Tile基类
    /// </summary>
    [MouseInput]
    public abstract class TileBase : EntityLogic
    {
        #region 成员变量
        private TileMap m_Map;
        private TileBaseData m_Data;
        private int m_Count;
        #endregion

        #region 继承变量
        protected Dictionary<TileItemType, List<TileItemBase>> m_Items;
        #endregion

        #region 属性
        public abstract TileType Type { get; }

        public int Count
        {
            get
            {
                return m_Count;
            }
        }

        public TileBaseData Data
        {
            get
            {
                return m_Data;
            }
        }

        public TileMap Map
        {
            get
            {
                return m_Map;
            }
        }

        public TileBase Top
        {
            get { return m_Map.Top(this); }
        }

        public TileBase Bottom
        {
            get { return m_Map.Bottom(this); }
        }

        public TileBase Left
        {
            get { return m_Map.Left(this); }
        }

        public TileBase Right
        {
            get { return m_Map.Right(this); }
        }
        #endregion

        #region 生命周期
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_Items = new Dictionary<TileItemType, List<TileItemBase>>();
            m_Data = userData as TileBaseData;

            //监听TileMap初始化完成事件
            MeaponEntry.Event.Subscribe(TileMapInitEventArgs.EventId, InternalOnTileMapInit);
            //触发Tile初始化完成事件
            MeaponEntry.Event.Fire(TileInitEventArgs.EventId, TileInitEventArgs.Create(this));

            MeaponEntry.CoreCom.InputController.Input.RegisterOpInputCode(OperateCode.EnterTile, MouseInputHelper.Code.OnEnterArea);
            MeaponEntry.CoreCom.InputController.Input.RegisterOpInputCode(OperateCode.LeaveTile, MouseInputHelper.Code.OnLeaveArea);
            MeaponEntry.CoreCom.InputController.Input.RegisterOperateCallback(OperateCode.EnterTile, InternalOnMouseEnterArea);
            MeaponEntry.CoreCom.InputController.Input.RegisterOperateCallback(OperateCode.LeaveTile, InternalOnMouseLeaveArea);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            CachedTransform.position = m_Data.Pos;
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            List<TileItemBase> allItems = new List<TileItemBase>(m_Count);
            foreach (List<TileItemBase> itemList in m_Items.Values)
            {
                allItems.AddRange(itemList);
            }

            foreach (TileItemBase item in allItems)
            {
                item.OnHide();
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            List<TileItemBase> allItems = new List<TileItemBase>(m_Count);
            foreach (List<TileItemBase> itemList in m_Items.Values)
            {
                allItems.AddRange(itemList);
            }

            foreach (TileItemBase item in allItems)
            {
                item.OnUpdate();
            }
        }
        #endregion

        #region 接口
        public bool HasItem(TileItemType type)
        {
            if (m_Items.TryGetValue(type, out List<TileItemBase> list))
            {
                if (list.Count > 0)
                    return true;
            }

            return false;
        }

        public bool GetItems(List<TileItemBase> list)
        {
            if (list == null)
                return false;

            int old = list.Count;
            foreach (List<TileItemBase> items in m_Items.Values)
                list.AddRange(items);
            return list.Count > old;
        }

        public List<TileItemBase> GetItem(TileItemType type)
        {
            if (m_Items.TryGetValue(type, out List<TileItemBase> list))
            {
                return list;
            }

            return null;
        }

        public T PutItem<T>(object userData) where T : TileItemBase, new()
        {
            T item = new T();
            GameObject obj = new GameObject();
            obj.transform.parent = CachedTransform;
            obj.AddComponent<SpriteRenderer>();

            item.OnInit(obj, this, userData);
            item.OnEnterTile(this);
            item.OnShow();
            if (InternalPutItem(item))
            {
                return item;
            }
            else
            {
                return default;
            }
        }

        public bool MoveItem(TileItemBase item)
        {
            InternalPutItem(item);
            item.OnEnterTile(this);
            item.OnShow();
            return true;
        }

        public void PopItem(TileItemBase item)
        {
            List<TileItemBase> tileList;
            TileItemType type = item.Type;
            if (m_Items.TryGetValue(type, out tileList))
            {
                foreach (TileItemBase i in tileList)
                {
                    if (i == item)
                    {
                        tileList.Remove(i);
                        item.OnLeaveTile(this);
                        return;
                    }
                }
            }
        }

        public void Select()
        {
            InternalOnSelected();
        }
        #endregion

        #region 事件
        private void InternalOnMouseEnterArea(object userData)
        {
            MouseEntityData data = userData as MouseEntityData;
            TileBase tile = data.Entity as TileBase;
            if (!tile.Equals(this))
                return;
            OnMouseEnterArea();
        }

        private void InternalOnMouseLeaveArea(object userData)
        {
            MouseEntityData data = userData as MouseEntityData;
            TileBase tile = data.Entity as TileBase;
            if (!tile.Equals(this))
                return;
            OnMouseLeaveArea();
        }

        private void InternalOnSelected()
        {
            List<TileItemBase> allList = new List<TileItemBase>();
            foreach (List<TileItemBase> itemList in m_Items.Values)
            {
                foreach (TileItemBase item in itemList)
                {
                    allList.Add(item);
                }
            }

            
            foreach (TileItemBase item in allList)
            {
                item.OnSelect();
            }

            OnSelected();
            MeaponEntry.Event.Fire(TileSelectedEventArgs.EventId, TileSelectedEventArgs.Create(this));
        }

        protected virtual void OnSelected() { }
        protected virtual void OnMouseEnterArea() { }
        protected virtual void OnMouseLeaveArea() { }
        #endregion

        #region 内部处理
        public bool InternalPutItem(TileItemBase item)
        {
            List<TileItemBase> tileList;
            TileItemType type = item.Type;
            if (!m_Items.TryGetValue(type, out tileList))
            {
                tileList = new List<TileItemBase>();
                m_Items[type] = tileList;
            }
            tileList.Add(item);
            return true;
        }

        private void InternalOnTileMapInit(object sendre, GameEventArgs args)
        {
            TileMapInitEventArgs e = (TileMapInitEventArgs)args;
            m_Map = e.Map;

            MeaponEntry.Event.Unsubscribe(TileMapInitEventArgs.EventId, InternalOnTileMapInit);
        }
        #endregion
    }
}
