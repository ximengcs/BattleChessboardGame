using FairyGUI;
using GameFramework.UI;
using Meapon.UI;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// FGUI界面辅助器。
    /// </summary>
    public class FGUIFormHelper : UIFormHelperBase
    {
        private ResourceComponent m_ResourceComponent = null;
        private UIPackage m_Package = null;
        private UIPanel m_SceneUIRoot = null;

        private void Awake()
        {
            
        }

        /// <summary>
        /// 实例化界面。
        /// </summary>
        /// <param name="uiFormAsset">要实例化的界面资源。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>实例化后的界面。</returns>
        public override object InstantiateUIForm(string uiFormName, object uiFormAsset, object userData)
        {
            if (m_Package == null)
            {
                TextAsset text = uiFormAsset as TextAsset;
                AssetBundle bundle = AssetBundle.LoadFromMemory(text.bytes);
                m_Package = UIPackage.AddPackage(bundle);
            }

            GObject inst = m_Package.CreateObject(uiFormName);
            return inst;
        }

        /// <summary>
        /// 创建界面。
        /// </summary>
        /// <param name="uiFormInstance">界面实例。</param>
        /// <param name="uiGroup">界面所属的界面组。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面。</returns>
        public override IUIForm CreateUIForm(object uiFormInstance, IUIGroup uiGroup, object userData)
        {
            GObject inst = uiFormInstance as GObject;
            if (inst == null)
            {
                Log.Error("FGUI GObject is invalid.");
                return null;
            }

            GRoot.inst.AddChild(inst);
            return FGUIForm.Create(inst);
        }

        /// <summary>
        /// 释放界面。
        /// </summary>
        /// <param name="uiFormAsset">要释放的界面资源。</param>
        /// <param name="uiFormInstance">要释放的界面实例。</param>
        public override void ReleaseUIForm(object uiFormAsset, object uiFormInstance)
        {
            m_ResourceComponent.UnloadAsset(uiFormAsset);

            GObject obj = (GObject)uiFormInstance;
            obj.Dispose();
        }

        private void Start()
        {
            m_ResourceComponent = GameEntry.GetComponent<ResourceComponent>();
            if (m_ResourceComponent == null)
            {
                Log.Fatal("Resource component is invalid.");
                return;
            }
        }
    }
}
