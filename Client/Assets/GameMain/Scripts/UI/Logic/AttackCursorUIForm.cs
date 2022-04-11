
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Try2
{
    public class AttackCursorUIForm : UIFormLogic
    {
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Vector2 pos = (Vector2)userData;
            CachedTransform.position = pos;
        }
    }
}
