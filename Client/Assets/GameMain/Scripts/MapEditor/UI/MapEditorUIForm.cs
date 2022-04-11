using MeaponUnity.Core.Entry;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Try2
{
    public class MapEditorUIForm : UIFormLogic
    {
        private Button m_Btn;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            RectTransform transform = GetComponent<RectTransform>();
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.anchoredPosition = Vector2.zero;
            transform.sizeDelta = Vector2.zero;

            Transform tf = transform.Find("Tiles").Find("Viewport").Find("Content").Find("Image");
            tf.GetComponent<Button>().onClick.AddListener(() =>
            {
                MeaponEntry.Event.Fire(this, MapEditorSelectChangeEventArgs.Create(200003));
            });

            transform.Find("SaveBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                MapEditorProcedure pro = (MapEditorProcedure)MeaponEntry.Procedure.CurrentProcedure;
                pro.Save();
            });

            transform.Find("OpenBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                MapEditorProcedure pro = (MapEditorProcedure)MeaponEntry.Procedure.CurrentProcedure;
                pro.Open();
            });
        }
    }
}
