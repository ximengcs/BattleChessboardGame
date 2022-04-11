using GameFramework.DataNode;
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Resource;
using MeaponUnity.Core.Com;
using MeaponUnity.Core.Entry;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Try2
{
    public partial class Selector : EntityLogic
    {
        private SelectorData m_Data;
        private TileBase m_Current;
        private SpriteRenderer m_Render;
        private Animation m_Anim;
        private IFsm<Selector> m_State;

        private SelectAction m_SelectUI;

        private Action<TileBase> m_OnSelectChangeEvent;

        public event Action<TileBase> OnSelectChangeEvent
        {
            add { m_OnSelectChangeEvent += value; }
            remove { m_OnSelectChangeEvent -= value; }
        }

        public bool IsReady
        {
            get
            {
                return !(m_State.CurrentState is StartSelectState);
            }
        }

        public SelectAction UI
        {
            get
            {
                return m_SelectUI;
            }
        }

        public void Select(TileBase tile)
        {
            m_Current = tile;
            m_Current.Select();
            UpdateState();
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            gameObject.name = nameof(Selector);
            m_Data = (SelectorData)userData;
            m_Current = m_Data.Tile;
            m_Render = GetComponent<SpriteRenderer>();
            m_Anim = gameObject.AddComponent<Animation>();

            SelectActionData data = new SelectActionData();
            m_SelectUI = new SelectAction(Entity, data);

            MeaponEntry.CoreCom.InputController.Input.RegisterOpInputCode(OperateCode.Select, MouseInputHelper.Code.OnLeftClick);
            MeaponEntry.CoreCom.InputController.Input.RegisterOperateCallback(OperateCode.Select, OnTouchSelect);

            m_State = MeaponEntry.Fsm.CreateFsm(this, new List<FsmState<Selector>>()
            {
                new WaitOperateState(),
                new StartSelectState(),
                new DoActionState()
            });

            MeaponEntry.Event.Fire(SelectorInitEventArgs.EventId, SelectorInitEventArgs.Create(this));
        }


        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            Vector3 pos = CachedTransform.position;
            pos.z = Dream.Definition.Constant.Layer.Selector;
            CachedTransform.position = pos;

            m_State.Start<WaitOperateState>();
            LoadAssetCallbacks callback = new LoadAssetCallbacks(InternalLoadImage);
            MeaponEntry.Resource.LoadAsset(AssetsPathUtils.GetItemAnim("selector_idle"), callback);
        }

        private void OnTouchSelect(object obj)
        {
            MouseClickData data = (MouseClickData)obj;
            TileBase tile = (TileBase)data.Entity;
            m_OnSelectChangeEvent?.Invoke(tile);
            if (!IsReady)
                return;

            Select(tile);
        }

        private void UpdateState()
        {
            Vector3 pos = m_Current.Data.Pos;
            pos.z = CachedTransform.position.z;
            CachedTransform.position = pos;
        }

        private void InternalLoadImage(string assetName, object asset, float duration, object userData)
        {
            AnimationClip clip = (AnimationClip)asset;
            Debug.LogWarning(clip == null);
            m_Anim.AddClip(clip, "idle");
            m_Anim.Play("idle");
            //m_Anim.animateOnlyIfVisible
            //m_Render.sprite = (Sprite)asset;
        }
    }
}
