using MeaponUnity.Core.Entry;

namespace Try2
{
    public class EntityUtils
    {
        public const string GRID_GROUP = "Grids";
        public const string Item_GROUP = "Player";

        public static void ShowTile(TileType type, TileBaseData data)
        {
            switch(type)
            {
                case TileType.Common:
                    MeaponEntry.Entity.ShowEntity<CommonTile>(GameUtils.GetSeries(), AssetsPathUtils.GetTilePrefab(), GRID_GROUP, 0, data);
                    break;

                case TileType.MapEditor:
                    MeaponEntry.Entity.ShowEntity<MapEditorTile>(GameUtils.GetSeries(), AssetsPathUtils.GetTilePrefab(), GRID_GROUP, 0, data);
                    break;
            }
        }

        public static void ShowSelector(SelectorData data)
        {
            MeaponEntry.Entity.ShowEntity<Selector>(GameUtils.GetSeries(), AssetsPathUtils.GetTileItemPrefab(), Item_GROUP, 0, data);
        }
    }
}
