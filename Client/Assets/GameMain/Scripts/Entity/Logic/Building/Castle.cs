using GameFramework.Resource;
using MeaponUnity.Core.Entry;
using UnityEngine;

namespace Try2
{
    public class Castle : TileItemBase
    {
        private GameObject m_Object;
        private SpriteRenderer m_Render;
        private CastleData m_Data;

        public override TileItemType Type
        {
            get
            {
                return TileItemType.Building;
            }
        }

        public override void OnInit(GameObject gameObject, TileBase tile, object userData)
        {
            base.OnInit(gameObject, tile, userData);
            m_Object = gameObject;
            m_Render = gameObject.GetComponent<SpriteRenderer>();
            m_Data = (CastleData)userData;
        }

        public override void OnShow()
        {
            base.OnShow();

            Vector3 pos = m_Object.transform.position;
            pos.z = Dream.Definition.Constant.Layer.Building;
            m_Object.transform.position = pos;

            LoadAssetCallbacks callback = new LoadAssetCallbacks(InternalLoadImage);
            MeaponEntry.Resource.LoadAsset(AssetsPathUtils.GetItem(m_Data.ResName), callback);
        }

        public override void OnSelect()
        {
            base.OnSelect();
        }

        private void InternalLoadImage(string assetName, object asset, float duration, object userData)
        {
            m_Render.sprite = (Sprite)asset;
        }
    }
}
