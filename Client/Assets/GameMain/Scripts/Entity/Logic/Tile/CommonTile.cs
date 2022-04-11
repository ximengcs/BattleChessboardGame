using GameFramework.Resource;
using MeaponUnity.Core.Entry;
using UnityEngine;

namespace Try2
{
    public class CommonTile : TileBase
    {
        private CommonTileData m_Data;
        private SpriteRenderer m_Render;

        public override TileType Type
        {
            get
            {
                return TileType.Common;
            }
        }

        public CommonTileData CommonData
        {
            get { return m_Data; }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_Data = userData as CommonTileData;
            m_Render = GetComponent<SpriteRenderer>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            Vector3 pos = CachedTransform.position;
            pos.z = Dream.Definition.Constant.Layer.Map;
            CachedTransform.position = pos;

            LoadAssetCallbacks callback = new LoadAssetCallbacks(InternalLoadImage);
            MeaponEntry.Resource.LoadAsset(AssetsPathUtils.GetTile(m_Data.DRData.Asset), callback);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        private void InternalLoadImage(string assetName, object asset, float duration, object userData)
        {
            m_Render.sprite = TileUtils.CreateSprite((Texture2D)asset);
        }
    }
}
