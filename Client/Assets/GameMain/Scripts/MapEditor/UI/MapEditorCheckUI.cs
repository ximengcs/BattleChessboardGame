using Dream.DataTable;
using FairyGUI;
using GameFramework.DataTable;
using GameFramework.Resource;
using Meapon.UI;
using MeaponUnity.Core.Entry;
using UnityEngine;

namespace Try2
{
    public class MapEditorCheckUI : FGUIFormLogic
    {
        private FGUIForm m_UIForm;
        private GComponent m_View;
        private GList m_TileList;
        private GList m_EntityList;
        private GList m_ItemList;

        private GButton m_Save;

        protected internal override void OnInit(FGUIForm uiForm, object userData)
        {
            base.OnInit(uiForm, userData);
            m_UIForm = uiForm;
            m_View = (GComponent)uiForm.Handle;

            m_TileList = m_View.GetChild("n1").asList;
            m_EntityList = m_View.GetChild("n4").asList;
            m_ItemList = m_View.GetChild("n3").asList;
            m_Save = m_View.GetChild("n8").asButton;

            IDataTable<DRTile> table = MeaponEntry.DataTable.GetDataTable<DRTile>();
            DRTile[] datas = table.GetAllDataRows();
            foreach (DRTile tile in datas)
            {
                new Item(m_TileList.AddItemFromPool().asButton, tile);
            }

            m_Save.onClick.Set(() =>
            {
                MapEditorProcedure pro = (MapEditorProcedure)MeaponEntry.Procedure.CurrentProcedure;
                pro.Save();
            });
        }

        private class Item
        {
            private GButton m_View;
            private DRTile m_Data;

            public Item(GButton view, DRTile data)
            {
                m_View = view;
                m_Data = data;

                LoadAssetCallbacks callback = new LoadAssetCallbacks(InternalLoadImage);
                MeaponEntry.Resource.LoadAsset(AssetsPathUtils.GetTile(data.Asset), callback);
            }

            private void InternalLoadImage(string assetName, object asset, float duration, object userData)
            {
                GLoader loader = m_View.GetChild("n3").asLoader;
                loader.texture = new NTexture(((Texture2D)asset));
                m_View.onClick.Set(() =>
                {
                    MeaponEntry.Event.Fire(MapEditorSelectChangeEventArgs.EventId, MapEditorSelectChangeEventArgs.Create(m_Data.Id));
                });
            }
        }
    }
}
