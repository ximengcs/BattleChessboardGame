using MeaponUnity.Core.Entry;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Try2
{
    public class AttackAction : IItemAction
    {
        private AttackActionData m_Data;
        private List<TileItemBase> m_InAttack;

        private AttackCursorItem m_Cursor;
        private AttackArea m_Range;
        private Action m_CompleteCallback;

        public AttackAction()
        {

        }

        public void Initialize(IItemActionData userData)
        {
            m_Data = (AttackActionData)userData;
        }

        public void OnCancel(Action callback)
        {

        }

        public void OnComplete(Action callback)
        {
            m_CompleteCallback = callback;
        }

        public void Start()
        {
            Log.Debug("Attack Action Do.", Log.BC, "Action");

            List<TileBase> list = TileUtils.FindCurrentAttackPath(m_Data.Item);
            m_InAttack = new List<TileItemBase>(4);
            InternalInitInAttack(list);

            TileBase first = m_InAttack[0].In;
            AttackCursorItemData data = new AttackCursorItemData(first.Data.Pos);
            m_Cursor = first.PutItem<AttackCursorItem>(data);

            m_Range = new AttackArea(list);
            m_Range.OnSelectCanReach(InternalAttackCheck);
            m_Range.Show();
        }

        public void Update()
        {

        }

        private void InternalInitInAttack(List<TileBase> list)
        {
            foreach (TileBase tile in list)
            {
                List<TileItemBase> items = tile.GetItem(TileItemType.Character);
                if (items != null)
                {
                    foreach (TileItemBase item in items)
                    {
                        if (item != m_Data.Item)
                            m_InAttack.Add(item);
                    }
                }
            }
        }

        private void InternalAttackCheck(TileBase tile)
        {
            foreach (TileItemBase item in m_InAttack)
            {
                if (item.In == tile)
                {
                    InternalAttack(item);
                }
            }

        }

        private void InternalAttack(TileItemBase item)
        {
            item.In.PopItem(m_Cursor);
            m_Cursor.OnRecycle();

            m_Data.Item.Attack(item);
            m_Range.Hide();
            m_CompleteCallback?.Invoke();
        }
    }
}
