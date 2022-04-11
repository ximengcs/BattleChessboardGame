using GameFramework.DataNode;
using GameFramework.Event;
using MeaponUnity.Core.Entry;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Try2
{
    public class RangeArea
    {
        private IDataNode m_Path;
        private List<Range> m_Range;
        private PropType m_Type;

        private Action<TileBase, IDataNode> m_OnSelectCanReachCallback;
        private Action<TileBase> m_OnSelectNotReachCallback;
        private Action<TileBase, IDataNode> m_OnSelectChangeCallback;

        public RangeArea(IDataNode path, PropType type)
        {
            m_Type = type;
            m_Path = path;
            m_Range = new List<Range>(10);
        }

        public void OnSelectCanReach(Action<TileBase, IDataNode> callback)
        {
            m_OnSelectCanReachCallback = callback;
        }

        public void OnSelectNotReach(Action<TileBase> callback)
        {
            m_OnSelectNotReachCallback = callback;
        }

        public void OnSelectChange(Action<TileBase, IDataNode> callback)
        {
            m_OnSelectChangeCallback = callback;
        }

        public void Show()
        {
            InternalIterate(m_Path);
            MeaponEntry.Event.Subscribe(TileSelectedEventArgs.EventId, InternalOnTileSelect);
        }

        public void Hide()
        {
            InternalClearRange();
            MeaponEntry.Event.Unsubscribe(TileSelectedEventArgs.EventId, InternalOnTileSelect);
        }

        private void InternalIterate(IDataNode node)
        {
            TileBase tile = node.GetData<VarTileBase>();
            InternalShowRange(tile);
            for (int i = 0; i < node.ChildCount; i++)
            {
                InternalIterate(node.GetChild(i));
            }
        }

        private void InternalShowRange(TileBase tile)
        {
            string resName = m_Type == PropType.MovePower ? "move_range" : "attack_range";
            RangeData data = new RangeData(resName, tile.Data.Pos);
            m_Range.Add(tile.PutItem<Range>(data));
        }

        private void InternalOnTileSelect(object sender, GameEventArgs args)
        {
            TileSelectedEventArgs e = (TileSelectedEventArgs)args;

            if (InternalCheck(m_Path, e.Tile))  //点击了可以到达的区域
            {
                IDataNode node = InternalGetPath(m_Path, e.Tile);
                IDataNode result = MeaponEntry.DataNode.GetOrAddNode(e.Tile.Name);
                result.Clear();
                InternalGetPathList(node, e.Tile, result);
                result = InternalGetPath(result);
                m_OnSelectCanReachCallback?.Invoke(e.Tile, result);
            }
            else  //点击了不可到达的区域
            {
                m_OnSelectNotReachCallback?.Invoke(e.Tile);
            }
        }

        private IDataNode InternalGetPath(IDataNode node, TileBase tile)
        {
            TileBase nodeTile = node.GetData<VarTileBase>();
            if (nodeTile == tile)
                return node;

            for (int i = 0; i < node.ChildCount; i++)
            {
                IDataNode child = InternalGetPath(node.GetChild(i), tile);
                if (child != null)
                    return child;
            }

            return null;
        }

        private void InternalGetPathList(IDataNode node, TileBase tile, IDataNode result)
        {
            TileBase nodeTile = node.GetData<VarTileBase>();

            result.SetData<VarTileBase>(nodeTile);
            if (node.Parent != null && node.Parent != MeaponEntry.DataNode.Root)
            {
                IDataNode child = result.GetOrAddChild(nodeTile.Name);
                InternalGetPathList(node.Parent, tile, child);
            }
        }

        private IDataNode InternalGetPath(IDataNode node)
        {
            if (node.ChildCount == 0)
                return node;
            return InternalGetPath(node.GetChild(0));
        }

        private void InternalClearRange()
        {
            foreach (Range range in m_Range)
            {
                range.In.PopItem(range);
                range.OnRecycle();
            }
        }

        private bool InternalCheck(IDataNode node, TileBase target)
        {
            TileBase current = node.GetData<VarTileBase>();
            if (target == current)
                return true;

            for (int i = 0; i < node.ChildCount; i++)
            {
                if (InternalCheck(node.GetChild(i), target))
                    return true;
            }

            return false;
        }
    }
}
