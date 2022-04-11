using System.Collections.Generic;

namespace Try2
{
    public class CastleData : TileItemBaseData
    {
        private string m_ResName;
        private ItemActionList m_Actions;

        public CastleData(string resName) : base()
        {
            m_ResName = resName;
            m_Actions = new ItemActionList(1);
            m_Actions.Add(ActionType.Recruit);
        }

        public string ResName
        {
            get
            {
                return m_ResName;
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
            get;
        }
    }
}
