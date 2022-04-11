using MeaponUnity.Core.Entry;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Try2
{
    public class MenuUIForm : UIFormLogic
    {
        private GameMenuProcedure m_Pro;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            RectTransform transform = GetComponent<RectTransform>();
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.anchoredPosition = Vector2.zero;
            transform.sizeDelta = Vector2.zero;

            m_Pro = (GameMenuProcedure)MeaponEntry.Procedure.CurrentProcedure;
            transform.Find("Start").GetComponent<Button>().onClick.AddListener(() =>
            {
                m_Pro.StartGame();
                MeaponEntry.UI.CloseUIForm(UIForm);
            });
            transform.Find("MapEditor").GetComponent<Button>().onClick.AddListener(() =>
            {
                m_Pro.StartEditor();
                MeaponEntry.UI.CloseUIForm(UIForm);
            });
        }
    }
}
