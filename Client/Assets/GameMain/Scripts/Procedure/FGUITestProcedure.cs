using FairyGUI;
using GameFramework.Fsm;
using GameFramework.Procedure;
using Meapon.UI;
using MeaponUnity.Core.Entry;
using UnityEngine;

namespace Try2
{
    public class FGUITestProcedure : ProcedureBase
    {
        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);
            //AssetBundle.LoadFromFile("Assets/StreamingAssets/ui");
            
            //UIPackage ui = UIPackage.AddPackage("Try2");
            //GRoot.inst.AddChild(ui.CreateObject("ActionUI"));
            //UIPanel panel = new UIPanel();
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            FGUIFormData data = new FGUIFormData();
            data.LogicType = typeof(ActionUI);
            //int seris = MeaponEntry.UI.OpenUIForm("ActionUI", "Assets/GameMain/UI/Try2_fui.bytes", "UI", data);
            MeaponEntry.Resource.LoadAsset("Assets/GameMain/Data/image.bytes", new GameFramework.Resource.LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
            {
                //Debug.LogWarning(assetName);
                TextAsset tex = asset as TextAsset;
                AssetBundle ab = AssetBundle.LoadFromMemory(tex.bytes);
                string[] assets = ab.GetAllAssetNames();
                foreach (string name in assets)
                    Debug.LogWarning(name);
                //Debug.LogWarning(ab == null);
            }));

        }
    }
}
