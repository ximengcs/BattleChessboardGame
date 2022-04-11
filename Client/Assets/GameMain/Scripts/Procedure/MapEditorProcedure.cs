using Dream.DataTable;
using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.FileSystem;
using GameFramework.Fsm;
using GameFramework.Procedure;
using Meapon.UI;
using MeaponUnity.Core.Com;
using MeaponUnity.Core.Entry;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Try2
{
    public class MapEditorProcedure : ProcedureBase
    {
        private GameAssets m_Assets;
        private MapEditorTileWaitItem m_CurrentSelect;
        private TileMap m_Map;

        public GameAssets Assets
        {
            get
            {
                return m_Assets;
            }
        }

        public MapEditorTileWaitItem Current
        {
            get
            {
                return m_CurrentSelect;
            }
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_Assets = procedureOwner.GetData<VarGameAssets>(GameAssets.Name);
            m_Map = new TileMap(25, 20);
            MeaponEntry.Event.Subscribe(TileInitEventArgs.EventId, OnTileInit);
            for (int i = 0; i < m_Map.Size.y; i++)
            {
                for (int j = 0; j < m_Map.Size.x; j++)
                {
                    Vector2 pos = new Vector2(j, i);
                    Vector2Int index = new Vector2Int(j, i);
                    MapEditorTileData data = new MapEditorTileData("tile_tree_0", pos, index);
                    EntityUtils.ShowTile(TileType.MapEditor, data);
                }
            }

            {
                MapEditorCheckUIData data = new MapEditorCheckUIData();
                data.LogicType = typeof(MapEditorCheckUI);
                MeaponEntry.UI.OpenUIForm("MapEditorCheckUI", AssetsPathUtils.GetPackage("ui"), "UI", data);
            }

            MeaponEntry.CoreCom.Camera.MouseCanDrag();
            MeaponEntry.Event.Subscribe(MapEditorSelectChangeEventArgs.EventId, OnSelectChangeEvent);
            MeaponEntry.CoreCom.InputController.Input.RegisterOpInputCode(EditorOpCode.Select, MouseInputHelper.Code.OnLeftClick);
            MeaponEntry.CoreCom.InputController.Input.RegisterOperateCallback(EditorOpCode.Select, InternalOnSelected);
        }

        private void OnTileInit(object sender, GameEventArgs args)
        {
            TileInitEventArgs e = (TileInitEventArgs)args;
            m_Map.Set(e.Tile);

            if (m_Map.IsFull)
            {
                MeaponEntry.Event.Fire(TileMapInitEventArgs.EventId, TileMapInitEventArgs.Create(m_Map));
                MeaponEntry.Event.Unsubscribe(TileInitEventArgs.EventId, OnTileInit);
            }
        }

        private void OnSelectChangeEvent(object sender, GameEventArgs args)
        {
            MapEditorSelectChangeEventArgs e = (MapEditorSelectChangeEventArgs)args;
            IDataTable<DRTile> table = MeaponEntry.DataTable.GetDataTable<DRTile>();
            DRTile dr = table.GetDataRow(e.TileId);

            TileBase tile = m_Map.Get(Vector2Int.zero);
            MapEditorTileWaitItemData data = new MapEditorTileWaitItemData(e.TileId, dr.Asset);


            if (m_CurrentSelect != null)
            {
                m_CurrentSelect.In.PopItem(m_CurrentSelect);
                m_CurrentSelect.OnRecycle();
            }

            m_CurrentSelect = tile.PutItem<MapEditorTileWaitItem>(data);
        }

        public void Save()
        {
            string path = Application.persistentDataPath + "/test.txt";
            IFileSystem file;
            if (!MeaponEntry.FileSystem.HasFileSystem(path))
                file = MeaponEntry.FileSystem.CreateFileSystem(path, FileSystemAccess.ReadWrite, 5, 1024);
            else
                file = MeaponEntry.FileSystem.GetFileSystem(path);

            MapStruture map = new MapStruture();
            map.Height = m_Map.Size.y;
            map.Width = m_Map.Size.x;
            map.Data = new List<MapItem>(100);
            var it = m_Map.GetEnumerator();
            while (it.MoveNext())
            {
                TileBase tile = it.Current;
                List<TileItemBase> items = tile.GetItem(TileItemType.Editor);
                if (items != null && items.Count == 1)
                {
                    MapItem item = new MapItem();
                    item.ID = ((MapEditorTileItem)items[0]).TileData.TileId;
                    map.Data.Add(item);
                }
                else
                {
                    MapItem item = new MapItem();
                    item.ID = 200001;
                    map.Data.Add(item);
                }
            }

            if (file.WriteFile("Map.json", Utility.Converter.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(map))))
                Debug.LogWarning("write success");
            file.SaveAsFile("Map.json", Application.persistentDataPath + "/Map.json");
        }

        public void Open()
        {
            string path = Application.persistentDataPath + "/test.txt";
            string mapPath = Application.persistentDataPath + "/map.txt";
            IFileSystem file = MeaponEntry.FileSystem.LoadFileSystem(path, FileSystemAccess.ReadWrite);
            byte[] data = file.ReadFile("Map.json");
            Debug.LogWarning(data.Length);
            Debug.LogWarning(Utility.Converter.GetString(data));
        }

        private void InternalOnSelected(object userData)
        {
            MouseClickData data = (MouseClickData)userData;
            MapEditorTile inst = (MapEditorTile)data.Entity;
            if (inst != null)
            {
                inst.Select();
            }
        }
    }
}
