using MeaponUnity.Core.Com;
using Try2;

namespace MeaponUnity.Core.Entry
{
    public partial class MeaponEntry
    {
        public static CoreComponent CoreCom
        {
            get;
            private set;
        }

        public static BuiltinDataComponent BuiltinData
        {
            get;
            private set;
        }

        

        private static void InitCustomsComponents()
        {
            BuiltinData = UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>();
            CoreCom = UnityGameFramework.Runtime.GameEntry.GetComponent<CoreComponent>();
        }
    }
}
