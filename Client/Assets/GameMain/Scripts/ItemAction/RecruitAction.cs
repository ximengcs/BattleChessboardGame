using Dream.DataTable;
using GameFramework.DataTable;
using MeaponUnity.Core.Entry;
using System;
using UnityGameFramework.Runtime;

namespace Try2
{
    public class RecruitAction : IItemAction
    {
        private RecruitActionData m_Data;
        private Action m_CompleteCallback;

        public void Initialize(IItemActionData userData)
        {
            m_Data = (RecruitActionData)userData;
        }

        public void OnCancel(Action callback)
        {

        }

        public void OnComplete(Action callback)
        {
            m_CompleteCallback = callback;
        }

        public void Start()
        {
            Log.Debug("Recruit Action Do.", Log.BC, "Action");

            IDataTable<DREntity> entityTable = MeaponEntry.DataTable.GetDataTable<DREntity>();
            DREntity row = entityTable.GetDataRow(100001);
            TileBase tile = m_Data.Item.In;
            CommonItemData data = new CommonItemData(row);
            CommonTileItem item = tile.PutItem<CommonTileItem>(data);

            MeaponEntry.Event.Fire(this, TileItemStartActionEventArgs.Create(item));
            m_CompleteCallback?.Invoke();
        }

        public void Update()
        {

        }
    }
}
