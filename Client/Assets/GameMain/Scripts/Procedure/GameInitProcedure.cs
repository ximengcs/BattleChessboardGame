using Dream.DataTable;
using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using MeaponUnity.Core.Entry;

namespace Try2
{
    /// <summary>
    /// 游戏初始化流程
    /// </summary>
    public class GameInitProcedure : ProcedureBase
    {
        private Game m_Game;
        private IFsm<IProcedureManager> m_Owner;

        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);
            m_Owner = procedureOwner;
            m_Game = new Game();
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);

            IDataTable<DRTile> tileTable = MeaponEntry.DataTable.GetDataTable<DRTile>();
            GameAssets assets = m_Owner.GetData<VarGameAssets>(GameAssets.Name);
            m_Owner.RemoveData(GameAssets.Name);
            MapStruture map = assets.MapTileStructure;
            m_Game.Initialize(assets, map.Width, map.Height);

            // 初始化地图
            MeaponEntry.Event.Subscribe(TileInitEventArgs.EventId, OnTileInit);
            for (int i = 0; i < map.Height; i++)
            {
                for (int j = 0; j < map.Width; j++)
                {
                    MapItem itemData = map.Data[i * map.Width + j];
                    DRTile tileRow = tileTable.GetDataRow(itemData.ID);
                    CommonTileData data = new CommonTileData(tileRow, j, i);
                    EntityUtils.ShowTile(TileType.Common, data);
                }
            }

            MeaponEntry.Event.Subscribe(TileMapInitEventArgs.EventId, OnTileMapInit);
        }

        private void OnTileMapInit(object sender, GameEventArgs args)
        {
            MeaponEntry.Event.Unsubscribe(TileMapInitEventArgs.EventId, OnTileMapInit);

            TileMapInitEventArgs e = (TileMapInitEventArgs)args;
            TileMap map = e.Map;

            //初始化地图物品
            {
                IDataTable<DREntity> entityTable = MeaponEntry.DataTable.GetDataTable<DREntity>();
                MapEntityStructure items = m_Game.Assets.MapEntityStructure;
                var it = items.Data.GetEnumerator();
                while (it.MoveNext())
                {
                    EntityDataItem item = it.Current;
                    DREntity row = entityTable.GetDataRow(item.Id);
                    TileBase tile = map.Get(item.X, item.Y);
                    CommonItemData data = new CommonItemData(row);
                    tile.PutItem<CommonTileItem>(data);
                }
            }

            //初始化建筑
            {
                TileBase tile = map.Get(1, 1);
                CastleData data = new CastleData("castle_0");
                tile.PutItem<Castle>(data);
            }

            //初始化选择器
            {
                SelectorData data = new SelectorData(map.Get(0, 0), "selector");
                EntityUtils.ShowSelector(data);
                MeaponEntry.Event.Subscribe(SelectorInitEventArgs.EventId, OnSelectorInit);
            }
        }

        private void OnTileInit(object sender, GameEventArgs args)
        {
            TileInitEventArgs e = (TileInitEventArgs)args;
            m_Game.Map.Set(e.Tile);

            if (m_Game.Map.IsFull)
            {
                MeaponEntry.Event.Fire(TileMapInitEventArgs.EventId, TileMapInitEventArgs.Create(m_Game.Map));
                MeaponEntry.Event.Unsubscribe(TileInitEventArgs.EventId, OnTileInit);
            }
        }

        private void OnSelectorInit(object sender, GameEventArgs args)
        {
            MeaponEntry.Event.Unsubscribe(SelectorInitEventArgs.EventId, OnSelectorInit);
            SelectorInitEventArgs e = (SelectorInitEventArgs)args;
            m_Game.Selector = e.Selector;

            m_Owner.SetData<VarGame>(Game.Name, m_Game);
            ChangeState<GameMainProcedure>(m_Owner);
        }
    }
}
