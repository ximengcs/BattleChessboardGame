using GameFramework.DataNode;
using GameFramework.Event;
using MeaponUnity.Core.Entry;
using System;
using System.Collections.Generic;

namespace Try2
{
    public class AttackArea
    {
        private List<TileBase> m_List;
        private List<Range> m_Range;

        private Action<TileBase> m_OnSelectCanReachCallback;
        private Action<TileBase> m_OnSelectNotReachCallback;
        private Action<TileBase> m_OnSelectChangeCallback;

        public AttackArea(List<TileBase> list)
        {
            m_List = list;
            m_Range = new List<Range>(10);
        }

        public void OnSelectCanReach(Action<TileBase> callback)
        {
            m_OnSelectCanReachCallback = callback;
        }

        public void OnSelectNotReach(Action<TileBase> callback)
        {
            m_OnSelectNotReachCallback = callback;
        }

        public void OnSelectChange(Action<TileBase> callback)
        {
            m_OnSelectChangeCallback = callback;
        }

        public void Show()
        {
            InternalIterate();
            MeaponEntry.Event.Subscribe(TileSelectedEventArgs.EventId, InternalOnTileSelect);
        }

        public void Hide()
        {
            InternalClearRange();
            MeaponEntry.Event.Unsubscribe(TileSelectedEventArgs.EventId, InternalOnTileSelect);
        }

        private void InternalIterate()
        {
            foreach (TileBase tile in m_List)
            {
                InternalShowRange(tile);
            }
        }

        private void InternalShowRange(TileBase tile)
        {
            RangeData data = new RangeData("attack_range", tile.Data.Pos);
            m_Range.Add(tile.PutItem<Range>(data));
        }

        private void InternalOnTileSelect(object sender, GameEventArgs args)
        {
            TileSelectedEventArgs e = (TileSelectedEventArgs)args;

            if (InternalCheck(e.Tile))  //点击了可以到达的区域
            {
                m_OnSelectCanReachCallback?.Invoke(e.Tile);
            }
            else  //点击了不可到达的区域
            {
                m_OnSelectNotReachCallback?.Invoke(e.Tile);
            }
        }

        private void InternalClearRange()
        {
            foreach (Range range in m_Range)
            {
                range.In.PopItem(range);
                range.OnRecycle();
            }
        }

        private bool InternalCheck(TileBase target)
        {
            foreach (TileBase tile in m_List)
            {
                if (tile == target)
                    return true;
            }

            return false;
        }
    }
}
