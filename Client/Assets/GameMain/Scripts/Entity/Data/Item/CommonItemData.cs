using Dream.DataTable;
using System.Collections.Generic;
using UnityEngine;

namespace Try2
{
    public class CommonItemData : TileItemBaseData
    {
        private ItemActionList m_Actions;
        private Dictionary<PropType, int> m_Props;
        private DREntity m_DRData;

        public CommonItemData(DREntity data)
        {
            m_DRData = data;
            m_Actions = new ItemActionList(data.Actions);
            m_Props = new Dictionary<PropType, int>(data.Prop.Count);
            
            foreach (var item in data.Prop)
            {
                m_Props[(PropType)item.Key] = item.Value;
            }
        }

        public DREntity DRData
        {
            get
            {
                return m_DRData;
            }
        }

        public override ItemActionList Actions
        {
            get
            {
                return m_Actions;
            }
        }

        public override Dictionary<PropType, int> Props
        {
            get
            {
                return m_Props;
            }
        }
    }
}
