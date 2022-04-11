using UnityGameFramework.Runtime;

namespace Try2
{
    public class Player : EntityLogic
    {
        public CommonTile Parent;

        public void Select()
        {
            Log.Debug("OnSelect", Log.RG, "PLAYER");
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            //TileData data = (TileData)userData;
            //SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            //sprite.sprite = data.Sprite;
            //GetComponent<SpriteRenderer>().sortingLayerName = "Character";
            //
            //transform.position = data.Pos;
            
            //ProcedureTest pro = (ProcedureTest)MeaponEntry.Procedure.CurrentProcedure;
            //pro.Inst.Player = this;
            //pro.Inst.Map[(int)transform.position.x, (int)transform.position.y].Items = this;
            //Parent = pro.Inst.Map[(int)transform.position.x, (int)transform.position.y];
        }
    }
}
