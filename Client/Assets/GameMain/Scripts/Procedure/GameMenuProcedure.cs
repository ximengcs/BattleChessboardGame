using GameFramework.Fsm;
using GameFramework.Procedure;
using Meapon.UI;
using MeaponUnity.Core.Entry;
using UnityEngine;

namespace Try2
{
    public class GameMenuProcedure : ProcedureBase
    {
        private IFsm<IProcedureManager> m_Owner;

        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);
            m_Owner = procedureOwner;
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_Owner = procedureOwner;
            FGUIFormData data = new FGUIFormData();
            data.LogicType = typeof(MenuUI);
            MeaponEntry.UI.OpenUIForm("MenuUI", AssetsPathUtils.GetPackage("ui"), "UI", data);
        }

        public void StartGame()
        {
            ChangeState<GameInitProcedure>(m_Owner);
        }

        public void StartEditor()
        {
            ChangeState<MapEditorProcedure>(m_Owner);
        }
    }
}
