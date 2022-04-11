using FairyGUI;
using Meapon.UI;
using MeaponUnity.Core.Entry;
using UnityEngine;

namespace Try2
{
    public class MenuUI : FGUIFormLogic
    {
        private FGUIForm m_UIForm;
        private GComponent m_View;
        private GButton m_GameBtn;
        private GButton m_EditorBtn;

        private GameMenuProcedure m_Pro;

        protected internal override void OnInit(FGUIForm uiForm, object userData)
        {
            base.OnInit(uiForm, userData);
            m_UIForm = uiForm;
            m_View = (GComponent)uiForm.Handle;

            m_GameBtn = m_View.GetChild("game").asButton;
            m_EditorBtn = m_View.GetChild("editor").asButton;

            m_Pro = (GameMenuProcedure)MeaponEntry.Procedure.CurrentProcedure;
            m_GameBtn.onClick.Set(() =>
            {
                m_Pro.StartGame();
                MeaponEntry.UI.CloseUIForm(m_UIForm);
            });

            m_EditorBtn.onClick.Set(() =>
            {
                m_Pro.StartEditor();
                MeaponEntry.UI.CloseUIForm(m_UIForm);
            });
        }
    }
}
