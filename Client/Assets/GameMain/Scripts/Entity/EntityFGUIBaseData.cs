
namespace Try2
{
    public abstract class EntityFGUIBaseData : EntityUIBaseData
    {
        private string m_ComponentName;

        public EntityFGUIBaseData(string name)
        {
            m_ComponentName = name;
        }

        public string ComponentName 
        {
            get
            {
                return m_ComponentName;
            }
        }
    }
}
