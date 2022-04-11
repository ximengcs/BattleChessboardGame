using GameFramework.DataNode;
using GameFramework.Event;
using GameFramework.Fsm;
using MeaponUnity.Core.Entry;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Try2
{
    public partial class Selector
    {
        /// <summary>
        /// Item等待操作的状态
        /// </summary>
        public class WaitOperateState : FsmState<Selector>
        {
            private IFsm<Selector> m_Fsm;
            private List<TileItemBase> m_Items;

            protected override void OnInit(IFsm<Selector> fsm)
            {
                base.OnInit(fsm);
                m_Fsm = fsm;
            }

            protected override void OnEnter(IFsm<Selector> fsm)
            {
                base.OnEnter(fsm);
                Log.Debug("Enter WaitOp", Log.MC, "Selector");

                //清空数据
                m_Fsm.SetData<VarTileBase>("Tile", null);
                m_Fsm.SetData<VarTileItemsBase>("Items", null);
                m_Fsm.SetData<VarInt32>("ActionIndex", 0);
                if (m_Items == null)
                    m_Items = new List<TileItemBase>();
                m_Items.Clear();

                MeaponEntry.Event.Subscribe(TileSelectedEventArgs.EventId, OnTileSelect);
                MeaponEntry.Event.Subscribe(TileItemStartActionEventArgs.EventId, OnItemActionStart);
            }

            protected override void OnLeave(IFsm<Selector> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);
                MeaponEntry.Event.Unsubscribe(TileSelectedEventArgs.EventId, OnTileSelect);
                MeaponEntry.Event.Unsubscribe(TileItemStartActionEventArgs.EventId, OnItemActionStart);
            }

            private void OnTileSelect(object sender, GameEventArgs args)
            {
                TileSelectedEventArgs e = (TileSelectedEventArgs)args;
                if (e.Tile.GetItems(m_Items))
                {
                    m_Fsm.SetData<VarTileBase>("Tile", e.Tile);
                    m_Fsm.SetData<VarTileItemsBase>("Items", m_Items);
                    m_Fsm.SetData<VarInt32>("ActionIndex", 0);
                    ChangeState<StartSelectState>(m_Fsm);
                }
            }

            private void OnItemActionStart(object sender, GameEventArgs args)
            {
                TileItemStartActionEventArgs e = (TileItemStartActionEventArgs)args;
                m_Items.Add(e.Item);
                m_Fsm.SetData<VarTileBase>("Tile", e.Item.In);
                m_Fsm.SetData<VarTileItemsBase>("Items", m_Items);
                m_Fsm.SetData<VarInt32>("ActionIndex", 0);
                ChangeState<StartSelectState>(m_Fsm);
            }
        }
    }
}
