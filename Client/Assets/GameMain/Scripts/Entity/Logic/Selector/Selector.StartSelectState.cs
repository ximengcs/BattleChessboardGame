using GameFramework.Fsm;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Try2
{
    public partial class Selector
    {
        /// <summary>
        /// Item等待选择操作的状态
        /// </summary>
        public partial class StartSelectState : FsmState<Selector>
        {
            private IFsm<Selector> m_Fsm;

            private TileBase m_Tile;
            private List<TileItemBase> m_Items;
            private Dictionary<ActionType, SelectData> m_CurrentList;
            private int m_ActionIndex;

            protected override void OnInit(IFsm<Selector> fsm)
            {
                base.OnInit(fsm);
                m_Fsm = fsm;
            }

            protected override void OnEnter(IFsm<Selector> fsm)
            {
                base.OnEnter(fsm);
                Log.Debug("Enter Start Select", Log.MC, "Selector");

                if (m_CurrentList == null)
                    m_CurrentList = new Dictionary<ActionType, SelectData>();
                m_CurrentList.Clear();

                m_Tile = fsm.GetData<VarTileBase>("Tile");
                m_Items = fsm.GetData<VarTileItemsBase>("Items");
                m_ActionIndex = fsm.GetData<VarInt32>("ActionIndex");

                ActionItemUIData data = new ActionItemUIData();
                data.Pos = m_Tile.Data.Pos;
                data.Items = new Dictionary<ActionType, SelectActionEventCallback>();

                SelectActionEventCallback callback = new SelectActionEventCallback();
                callback.OnSelectItem = OnSelectItem;

                foreach (TileItemBase item in m_Items)
                {
                    var list = item.Data.Actions.m_List;
                    if (list == null || list.Count <= m_ActionIndex)
                        continue;

                    foreach (var action in list[m_ActionIndex])
                    {
                        if (TileActionUtils.CheckCanDo(action.Key, item))
                        {
                            data.Items[action.Key] = callback;

                            SelectData sData = new SelectData();
                            sData.Item = item;
                            sData.Action = action.Value;
                            m_CurrentList[action.Key] = sData;
                        }
                    }
                }

                if (data.Items.Count == 0)
                {
                    m_Fsm.SetData<VarTileActionItemBase>("Action", null);
                    ChangeState<WaitOperateState>(m_Fsm);
                }
                else
                {
                    m_Fsm.Owner.UI.Show(data);
                }
            }

            private void OnSelectItem(ActionType type)
            {
                SelectData sData = m_CurrentList[type];
                m_Fsm.SetData<VarTileItemBase>("Item", sData.Item);
                m_Fsm.SetData<VarTileActionItemBase>("Action", sData.Action);
                ChangeState<DoActionState>(m_Fsm);
            }

            protected override void OnLeave(IFsm<Selector> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);
                m_Fsm.Owner.UI.Hide();
            }
        }
    }
}
