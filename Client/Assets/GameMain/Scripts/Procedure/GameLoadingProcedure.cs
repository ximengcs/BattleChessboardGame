using Dream.DataTable;
using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.FileSystem;
using GameFramework.Fsm;
using GameFramework.Procedure;
using GameFramework.Resource;
using MeaponUnity.Core.Entry;
using UnityEngine;
using UnityEngine.U2D;
using UnityGameFramework.Runtime;

namespace Try2
{
    /// <summary>
    /// 加载资源
    /// </summary>
    public class GameLoadingProcedure : ProcedureBase
    {
        private IFsm<IProcedureManager> m_Owner;
        private FlagScan m_LoadingFlag;
        private GameAssets m_Assets;

        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);
            m_Owner = procedureOwner;
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_Assets = new GameAssets();
            m_LoadingFlag = new FlagScan(7);

            //加载数据表
            //实体表
            MeaponEntry.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId, OnDataTableSuccess);
            string entityTableData = AssetsPathUtils.GetDataTable("Entity");
            DataTableBase entityTable = MeaponEntry.DataTable.CreateDataTable(typeof(DREntity));
            entityTable.ReadData(entityTableData, 0);
            m_LoadingFlag.Add();
            //地块表
            string tileTableData = AssetsPathUtils.GetDataTable("Tile");
            DataTableBase tileTable = MeaponEntry.DataTable.CreateDataTable(typeof(DRTile));
            tileTable.ReadData(tileTableData, 0);
            m_LoadingFlag.Add();
            //Buff表
            string buffTableData = AssetsPathUtils.GetDataTable("Buff");
            DataTableBase buffTable = MeaponEntry.DataTable.CreateDataTable(typeof(DRBuff));
            buffTable.ReadData(buffTableData, 0);
            m_LoadingFlag.Add();
            //行为表
            string actionTableData = AssetsPathUtils.GetDataTable("Action");
            DataTableBase actionTable = MeaponEntry.DataTable.CreateDataTable(typeof(DRAction));
            actionTable.ReadData(actionTableData, 0);
            m_LoadingFlag.Add();

            //加载地图
            string path = Application.persistentDataPath + "/test.txt";
            IFileSystem file = MeaponEntry.FileSystem.LoadFileSystem(path, FileSystemAccess.ReadWrite);
            m_Assets.PutMapTileAssets(Utility.Converter.GetString(file.ReadFile("Map.json")));

            //string mapData = AssetsPathUtils.GetMap("Map");
            //LoadAssetCallbacks mapCallback = new LoadAssetCallbacks(OnMapSuccess);
            //MeaponEntry.Resource.LoadAsset(mapData, mapCallback);
            //m_LoadingFlag.Add();

            //加载地图实体
            string mapEntityData = AssetsPathUtils.GetMap("Items");
            LoadAssetCallbacks mapEntityCallback = new LoadAssetCallbacks(OnMapEntitySuccess);
            MeaponEntry.Resource.LoadAsset(mapEntityData, mapEntityCallback);
            m_LoadingFlag.Add();
        }

        private void OnMapSuccess(string assetName, object asset, float duration, object userData)
        {
            m_Assets.PutMapTileAssets(((TextAsset)asset).text);
            InternalMinusCheckIfOK();
        }

        private void OnMapEntitySuccess(string assetName, object asset, float duration, object userData)
        {
            m_Assets.PutMapItemAssets(((TextAsset)asset).text);
            InternalMinusCheckIfOK();
        }

        private void OnDataTableSuccess(object sender, GameEventArgs args)
        {
            InternalMinusCheckIfOK();
        }

        private void InternalMinusCheckIfOK()
        {
            m_LoadingFlag.Minus();
            if (m_LoadingFlag.Yes())
            {
                InternalComplete();
            }
        }

        private void InternalComplete()
        {
            MeaponEntry.Event.Unsubscribe(LoadDataTableSuccessEventArgs.EventId, OnDataTableSuccess);

            m_Owner.SetData<VarGameAssets>(GameAssets.Name, m_Assets);
            ChangeState<GameMenuProcedure>(m_Owner);
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            m_LoadingFlag = null;
            m_Owner = null;
        }
    }
}
