
using System;

namespace Meapon.UI
{
    public class FGUIFormData
    {
        public Type LogicType { get; set; }
        public FGUIFormType UIType { get; set; }
    }

    public enum FGUIFormType
    {
        SceneUI,
        UI
    }
}
