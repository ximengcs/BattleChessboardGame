using GameFramework.Resource;
using MeaponUnity.Core.Entry;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Try2
{
    public class Range : TileItemBase
    {
        private RangeData m_Data;
        private SpriteRenderer m_Render;
        private GameObject m_Object;

        public override TileItemType Type
        {
            get
            {
                return TileItemType.Range;
            }
        }

        public TileItemBaseData ItemData => null;

        public override void OnInit(GameObject gameObject, TileBase tile, object userData)
        {
            base.OnInit(gameObject, tile, userData);
            m_Object = gameObject;
            m_Data = userData as RangeData;
            m_Render = gameObject.GetComponent<SpriteRenderer>();
        }

        public override void OnRecycle()
        {
            base.OnRecycle();
            GameObject.Destroy(m_Object);
        }

        public override void OnShow()
        {
            m_Object.transform.position = m_Data.Pos;
            Vector3 pos = m_Object.transform.position;
            pos.z = Dream.Definition.Constant.Layer.SelectorRange;
            m_Object.transform.position = pos;

            LoadAssetCallbacks callback = new LoadAssetCallbacks(InternalLoadImage);
            MeaponEntry.Resource.LoadAsset(AssetsPathUtils.GetItem(m_Data.ResName), callback);
        }

        private void InternalLoadImage(string assetName, object asset, float duration, object userData)
        {
            m_Render.sprite = (Sprite)asset;
        }
    }
}
