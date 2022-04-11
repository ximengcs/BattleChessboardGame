using UnityEngine;

namespace MeaponUnity.Core.Entry
{
    public partial class MeaponEntry : MonoBehaviour
    {
        private void Start()
        {
            InitBuiltinComponents();
            InitCustomsComponents();
        }
    }
}
