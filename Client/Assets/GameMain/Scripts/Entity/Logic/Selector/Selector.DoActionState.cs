using GameFramework.Fsm;
using MeaponUnity.Core.Entry;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Try2
{
    public partial class Selector
    {
        public class DoActionState : FsmState<Selector>
        {
            private IFsm<Selector> m_Fsm;
            private TileItemBase m_Item;
            private IItemAction m_ItemAction;
            private TileActionItem m_Current;

            protected override void OnInit(IFsm<Selector> fsm)
            {
                base.OnInit(fsm);
                m_Fsm = fsm;
            }

            protected override void OnEnter(IFsm<Selector> fsm)
            {
                base.OnEnter(fsm);
                Log.Debug("Enter DO Action", Log.MC, "Selector");
                m_Item = fsm.GetData<VarTileItemBase>("Item");
                m_Current = fsm.GetData<VarTileActionItemBase>("Action");

                IItemActionData data = Utils.ItemActionUtils.CreateTileActioData(m_Item, m_Current.Type);
                m_ItemAction = Utils.ItemActionUtils.CreateTileAction(data);
                m_ItemAction.OnCancel(OnActionCancel);
                m_ItemAction.OnComplete(OnActionComplete);
                m_ItemAction.Start();
            }

            protected override void OnUpdate(IFsm<Selector> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
                m_ItemAction.Update();
            }

            protected override void OnLeave(IFsm<Selector> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);
            }

            private void OnActionCancel()
            {
                ChangeState<StartSelectState>(m_Fsm);
            }

            private void OnActionComplete()
            {
                if (m_Current.HasNext)
                {
                    m_Fsm.SetData<VarInt32>("ActionIndex", m_Current.Next);
                    m_Fsm.SetData<VarTileItemBase>("Tile", m_Item);
                    m_Fsm.SetData<VarTileBase>("Tile", m_Item.In);
                    ChangeState<StartSelectState>(m_Fsm);
                }
                else
                {
                    m_Fsm.RemoveData("Tile");
                    m_Fsm.RemoveData("Item");
                    m_Fsm.RemoveData("Action");
                    m_Fsm.RemoveData("ActionIndex");
                    ChangeState<WaitOperateState>(m_Fsm);
                }
            }
        }
    }
}
