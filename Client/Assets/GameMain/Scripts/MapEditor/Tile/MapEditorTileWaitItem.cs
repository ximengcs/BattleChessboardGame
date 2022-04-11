
using GameFramework.Resource;
using MeaponUnity.Core.Entry;
using UnityEngine;

namespace Try2
{
    public class MapEditorTileWaitItem : TileItemBase
    {
        private GameObject m_Obj;
        private Sprite m_Image;
        private SpriteRenderer m_Render;
        private MapEditorTileWaitItemData m_WaitData;

        public override TileItemType Type
        {
            get
            {
                return TileItemType.EditorWait;
            }
        }

        public MapEditorTileWaitItemData WaitData
        {
            get
            {
                return m_WaitData;
            }
        }

        public override void OnInit(GameObject gameObject, TileBase tile, object userData)
        {
            base.OnInit(gameObject, tile, userData);
            m_Obj = gameObject;
            m_WaitData = (MapEditorTileWaitItemData)userData;

            Vector3 pos = tile.Data.Pos;
            pos.z = Dream.Definition.Constant.Layer.MapEditorWait;
            m_Obj.transform.position = pos;

            m_Render = m_Obj.GetComponent<SpriteRenderer>();
            m_Render.sprite = m_Image;
            m_Render.color -= new Color(0, 0, 0, 0.5f);

            LoadAssetCallbacks callback = new LoadAssetCallbacks(InternalLoadImage);
            MeaponEntry.Resource.LoadAsset(AssetsPathUtils.GetTile(m_WaitData.ResName), callback);
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
