
using GameFramework.Resource;
using MeaponUnity.Core.Entry;
using UnityEngine;

namespace Try2
{
    public class MapEditorTileItem : TileItemBase
    {
        private GameObject m_Obj;
        private Sprite m_Image;
        private SpriteRenderer m_Render;
        private MapEditorTileItemData m_TileData;

        public override TileItemType Type
        {
            get
            {
                return TileItemType.Editor;
            }
        }

        public MapEditorTileItemData TileData
        {
            get
            {
                return m_TileData;
            }
        }

        public override void OnInit(GameObject gameObject, TileBase tile, object userData)
        {
            base.OnInit(gameObject, tile, userData);
            m_Obj = gameObject;
            m_TileData = (MapEditorTileItemData)userData;

            Vector3 pos = tile.Data.Pos;
            pos.z = Dream.Definition.Constant.Layer.MapEditorItem;
            m_Obj.transform.position = pos;

            m_Render = m_Obj.GetComponent<SpriteRenderer>();

            LoadAssetCallbacks callback = new LoadAssetCallbacks(InternalLoadImage);
            MeaponEntry.Resource.LoadAsset(AssetsPathUtils.GetTile(m_TileData.ResName), callback);
        }

        private void InternalLoadImage(string assetName, object asset, float duration, object userData)
        {
            m_Render.sprite = TileUtils.CreateSprite((Texture2D)asset);
        }

        public override void OnRecycle()
        {
            base.OnRecycle();
            GameObject.Destroy(m_Obj);
        }
    }
}
