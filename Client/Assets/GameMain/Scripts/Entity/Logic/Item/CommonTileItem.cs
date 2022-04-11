using GameFramework.Resource;
using MeaponUnity.Core.Entry;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Try2
{
    public class CommonTileItem : TileItemBase
    {
        private CommonItemData m_Data;
        private SpriteRenderer m_Render;
        private GameObject m_Object;

        public override TileItemType Type
        {
            get
            {
                return TileItemType.Character;
            }
        }

        public CommonItemData ItemData
        {
            get
            {
                return m_Data;
            }
        }

        public override void Attack(TileItemBase other)
        {
            base.Attack(other);
            other.In.PopItem(other);
            other.OnRecycle();
        }

        public override void OnInit(GameObject gameObject, TileBase tile, object userData)
        {
            base.OnInit(gameObject, tile, userData);
            m_Data = (CommonItemData)userData;
            m_Object = gameObject;
            m_Render = gameObject.GetComponent<SpriteRenderer>();
        }

        public override void OnShow()
        {
            base.OnShow();

            Vector3 pos = m_Object.transform.position;
            pos.z = Dream.Definition.Constant.Layer.Character;
            m_Object.transform.position = pos;

            LoadAssetCallbacks callback = new LoadAssetCallbacks(InternalLoadImage);
            MeaponEntry.Resource.LoadAsset(AssetsPathUtils.GetEntity(m_Data.DRData.Asset), callback);
        }

        public override void OnRecycle()
        {
            base.OnRecycle();
            GameObject.Destroy(m_Object);
        }

        private void InternalLoadImage(string assetName, object asset, float duration, object userData)
        {
            m_Render.sprite = (Sprite)asset;
        }
    }
}
