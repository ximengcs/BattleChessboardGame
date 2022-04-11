using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Try2
{
    public class ItemActionList
    {
        public List<Dictionary<ActionType, TileActionItem>> m_List;

        public ItemActionList(List<Dictionary<int, int>> list)
        {
            m_List = new List<Dictionary<ActionType, TileActionItem>>(list.Count);
            foreach (Dictionary<int, int> actions in list)
            {
                Dictionary<ActionType, TileActionItem> item = new Dictionary<ActionType, TileActionItem>(actions.Count);
                m_List.Add(item);
                foreach (KeyValuePair<int, int> action in actions)
                {
                    TileActionItem real = new TileActionItem();
                    real.Next = action.Value;
                    real.Type = (ActionType)action.Key;
                    item.Add(real.Type, real);
                }
            }
        }

        public ItemActionList(int capacity)
        {
            m_List = new List<Dictionary<ActionType, TileActionItem>>(capacity);
        }

        /// <summary>
        /// 添加一种行为
        /// </summary>
        /// <param name="action">行为类型</param>
        /// <param name="step">第几步，-1表示在当前最后一项中添加</param>
        /// <param name="next">下一步行为，-1表示没有下一步</param>
        public void Add(ActionType action, int step = -1, int next = -1)
        {
            Dictionary<ActionType, TileActionItem> list;
            if (step != -1)
            {
                if (m_List.Count <= step)
                {
                    list = new Dictionary<ActionType, TileActionItem>();
                    m_List.Add(list);
                }
                else
                {
                    list = m_List[step];
                }
            }
            else
            {
                if (m_List.Count > 0)
                    list = m_List[m_List.Count - 1];
                else
                {
                    list = new Dictionary<ActionType, TileActionItem>();
                    m_List.Add(list);
                }
            }

            TileActionItem item = new TileActionItem();
            item.Type = action;
            item.Next = next;
            list[action] = item;
        }
    }

    public class TileActionItem
    {
        public ActionType Type { get; set; }
        public int Next { get; set; }
        public bool HasNext { get { return Next != -1; } }
    }
}
