using FairyGUI;
using GameFramework.UI;
using System;

namespace Meapon.UI
{
    public class FGUIForm : IUIForm
    {
        private int m_SerialId;
        private string m_Name;
        private GObject m_Root;
        private IUIGroup m_Group;
        private int m_DepthInUIGroup;
        private bool m_PauseCoveredUIForm;
        private FGUIFormLogic m_Logic;

        public int SerialId
        {
            get
            {
                return m_SerialId;
            }
        }

        public string UIFormAssetName
        {
            get
            {
                return m_Name;
            }
        }

        public object Handle
        {
            get
            {
                return m_Root;
            }
        }

        public IUIGroup UIGroup
        {
            get
            {
                return m_Group;
            }
        }

        public int DepthInUIGroup
        {
            get
            {
                return m_DepthInUIGroup;
            }
        }

        public bool PauseCoveredUIForm
        {
            get
            {
                return m_PauseCoveredUIForm;
            }
        }

        public void OnClose(bool isShutdown, object userData)
        {
            m_Logic.OnClose(isShutdown, userData);
        }

        public void OnCover()
        {
            m_Logic.OnCover();
        }

        public void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            m_DepthInUIGroup = uiGroupDepth;
            m_Logic.OnDepthChanged(uiGroupDepth, depthInUIGroup);
        }

        public void OnInit(int serialId, string uiFormAssetName, IUIGroup uiGroup, bool pauseCoveredUIForm, bool isNewInstance, object userData)
        {
            m_SerialId = serialId;
            m_Name = uiFormAssetName;
            m_Group = uiGroup;
            m_DepthInUIGroup = 0;
            m_PauseCoveredUIForm = pauseCoveredUIForm;

            if (!isNewInstance)
                return;

            FGUIFormData data = userData as FGUIFormData;
            m_Logic = (FGUIFormLogic)Activator.CreateInstance(data.LogicType);
            m_Logic.OnInit(this, userData);
        }

        public void OnOpen(object userData)
        {
            m_Logic.OnOpen(userData);
        }

        public void OnPause()
        {
            m_Logic.OnPause();
        }

        public void OnRecycle()
        {
            m_Logic.OnRecycle();
            m_SerialId = 0;
            m_DepthInUIGroup = 0;
            m_PauseCoveredUIForm = true;
        }

        public void OnRefocus(object userData)
        {
            m_Logic.OnRefocus(userData);
        }

        public void OnResume()
        {
            m_Logic.OnResume();
        }

        public void OnReveal()
        {
            m_Logic.OnReveal();
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            m_Logic.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        internal static FGUIForm Create(GObject root)
        {
            FGUIForm uiForm = new FGUIForm();
            uiForm.m_Root = root;
            return uiForm;
        }
    }
}
