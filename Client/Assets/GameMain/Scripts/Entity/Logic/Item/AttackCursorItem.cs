
using GameFramework.Resource;
using MeaponUnity.Core.Entry;
using UnityEngine;

namespace Try2
{
    public class AttackCursorItem : TileItemBase
    {
        private GameObject m_Object;
        private SpriteRenderer m_Render;
        private AttackCursorItemData m_Data;

        public override TileItemType Type
        {
            get
            {
                return TileItemType.AttackCursor;
            }
        }

        public override void OnInit(GameObject gameObject, TileBase tile, object userData)
        {
            base.OnInit(gameObject, tile, userData);
            m_Object = gameObject;
            m_Data = userData as AttackCursorItemData;
            m_Render = gameObject.GetComponent<SpriteRenderer>();
        }

        public override void OnShow()
        {
            base.OnShow();
            m_Object.transform.position = m_Data.Pos;
            Vector3 pos = m_Object.transform.position;
            pos.z = Dream.Definition.Constant.Layer.Selector;
            m_Object.transform.position = pos;

            LoadAssetCallbacks callback = new LoadAssetCallbacks(InternalLoadImage);
            MeaponEntry.Resource.LoadAsset(AssetsPathUtils.GetItem("attack_cursor"), callback);
        }

        public override void OnRecycle()
        {
            base.OnRecycle();
            GameObject.Destroy(m_Object);
            m_Data = null;
        }

        private void InternalLoadImage(string assetName, object asset, float duration, object userData)
        {
            m_Render.sprite = (Sprite)asset;
        }
    }
}
