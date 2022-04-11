using DG.Tweening;
using GameFramework.DataNode;
using MeaponUnity.Core.Entry;
using System;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Try2
{
    public class MoveAction : IItemAction
    {
        private MoveActionData m_Data;
        private RangeArea m_Range;
        private Transform m_ItemTf;

        private Action m_CompleteCallback;
        private Action m_CancelCallback;

        private TileBase m_Target;
        private IDataNode m_Current;
        private Vector2 m_CurrentPos;

        public void Initialize(IItemActionData userData)
        {
            m_Data = userData as MoveActionData;
        }

        public void OnCancel(Action callback)
        {
            m_CancelCallback = callback;
        }

        public void OnComplete(Action callback)
        {
            m_CompleteCallback = callback;
        }

        public void Start()
        {
            Log.Debug("Move Action Do.", Log.BC, "Action");

            m_ItemTf = m_Data.Item.Object.transform;
            IDataNode path = TileUtils.FindItemPath(m_Data.Item);
            m_Range = new RangeArea(path, PropType.MovePower);
            m_Range.OnSelectCanReach(InternalMove);
            m_Range.Show();
        }

        public void Update()
        {
            if (m_Current == null)
                return;

            if (m_Current == MeaponEntry.DataNode.Root)
            {
                m_Current = null;
                InternalMoveEnd();
            }
            else
            {
                DoMove(m_Current);
                m_Current = null;
            }
        }

        private void InternalMove(TileBase tile, IDataNode node)
        {
            m_Current = node;
            m_CurrentPos = m_Data.Tile.Data.Pos;
            m_Target = tile;
        }

        private void InternalMoveEnd()
        {
            TileItemBase item = m_Data.Item;
            TileBase src = item.In;
            src.PopItem(item);
            m_Target.MoveItem(item);
            m_Range.Hide();
            m_CompleteCallback?.Invoke();
        }

        private void DoMove(IDataNode node)
        {
            TileBase tile = node.GetData<VarTileBase>();
            if (tile == m_Data.Tile)
            {
                DoMove(node.Parent);
                return;
            }

            if (node != null && tile != m_Data.Tile)
            {
                Vector2 pos = tile.Data.Pos;
                float x = pos.x - m_CurrentPos.x;
                float y = pos.y - m_CurrentPos.y;
                m_CurrentPos = pos;

                if (x != 0)
                {
                    m_ItemTf.DOMoveX(m_CurrentPos.x, 0.15f)
                        .SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            m_Current = node.Parent;
                        });
                }
                else if (y != 0)
                {
                    m_ItemTf.DOMoveY(m_CurrentPos.y, 0.15f)
                        .SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            m_Current = node.Parent;
                        });
                }
            }
        }
    }
}
