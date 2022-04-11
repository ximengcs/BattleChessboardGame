using GameFramework.Fsm;
using GameFramework.Procedure;
using MeaponUnity.Core.Entry;
using UnityEngine;

namespace Try2
{
    public class GameMainProcedure : ProcedureBase
    {
        private Game m_Game;
        private IFsm<IProcedureManager> m_Owner;

        public Game GameInst
        {
            get
            {
                return m_Game;
            }
        }

        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);
            m_Owner = procedureOwner;
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_Game = m_Owner.GetData<VarGame>(Game.Name);
            m_Owner.RemoveData(Game.Name);
            m_Game.Start();

            MeaponEntry.CoreCom.Camera.MouseCanDrag();
        }
    }
}
