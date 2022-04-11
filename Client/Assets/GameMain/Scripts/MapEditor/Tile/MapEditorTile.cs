using Dream.DataTable;
using GameFramework.DataTable;
using GameFramework.Resource;
using MeaponUnity.Core.Com;
using MeaponUnity.Core.Entry;
using System.Collections.Generic;
using UnityEngine;

namespace Try2
{
    public class MapEditorTile : TileBase
    {
        private MapEditorTileData m_TileData;
        private SpriteRenderer m_Render;
        private MapEditorProcedure m_Procedure;

        public override TileType Type
        {
            get
            {
                return TileType.MapEditor;
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_TileData = (MapEditorTileData)userData;
            m_Render = GetComponent<SpriteRenderer>();
            m_Procedure = (MapEditorProcedure)MeaponEntry.Procedure.CurrentProcedure;
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            Vector3 pos = CachedTransform.position;
            pos.z = Dream.Definition.Constant.Layer.Map;
            CachedTransform.position = pos;

            LoadAssetCallbacks callback = new LoadAssetCallbacks(InternalLoadImage);
            MeaponEntry.Resource.LoadAsset(AssetsPathUtils.GetTile(m_TileData.ResName), callback);
        }

        protected override void OnSelected()
        {
            if (m_Procedure.Current != null)
            {
                int id = m_Procedure.Current.WaitData.TileId;
                IDataTable<DRTile> table = MeaponEntry.DataTable.GetDataTable<DRTile>();
                DRTile raw = table.GetDataRow(id);
                MapEditorTileItemData data = new MapEditorTileItemData(id, raw.Asset);

                List<TileItemBase> tmp = GetItem(TileItemType.Editor);
                if (tmp != null)
                {
                    List<TileItemBase> items = new List<TileItemBase>(tmp);
                    foreach (TileItemBase item in items)
                    {
                        PopItem(item);
                        item.OnRecycle();
                    }
                }


                PutItem<MapEditorTileItem>(data);
            }
        }

        protected override void OnMouseEnterArea()
        {
            TileItemBase item = m_Procedure.Current;
            if (item != null)
            {
                item.In.PopItem(item);
                MoveItem(item);
            }
        }

        private void InternalLoadImage(string assetName, object asset, float duration, object userData)
        {
            m_Render.sprite = TileUtils.CreateSprite((Texture2D)asset);
        }
    }
}
