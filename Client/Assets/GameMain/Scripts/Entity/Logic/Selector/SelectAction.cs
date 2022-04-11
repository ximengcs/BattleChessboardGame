using Dream.DataTable;
using FairyGUI;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Try2
{
    public class SelectAction : EntityFGUIBase
    {
        private GComponent m_View;
        private GList m_List;
        private GLoader m_Loader;
        private SelectActionData m_InstData;
        private ActionItemUIData m_Data;

        public SelectAction(Entity entity, object userData) : base(entity, userData)
        {
            m_InstData = userData as SelectActionData;
        }

        protected override void OnInit(GComponent view)
        {
            m_View = view;
            m_List = view.GetChild("n0").asList;
        }

        public void SetData(SelectActionData data)
        {
            m_InstData = data;
        }

        public override void Show(object userData)
        {
            base.Show(userData);
            m_Data = (ActionItemUIData)userData;
            InternalShow();
        }

        public override void Hide()
        {
            base.Hide();
            m_List.RemoveChildrenToPool();
        }

        private void InternalShow()
        {
            m_List.RemoveChildrenToPool();
            m_List.width = 16 * m_Data.Items.Count;
            foreach (var item in m_Data.Items)
            {
                GButton btn = m_List.AddItemFromPool().asButton;
                DRAction drData = TileActionUtils.GetDR(item.Key);
                m_Loader = btn.GetChild("n4").asLoader;
                m_Loader.url = $"ui://Try2/{drData.Icon}";
                btn.onClick.Set(() =>
                {
                    item.Value.OnSelectItem?.Invoke(item.Key);
                });
            }
        }
    }
}
