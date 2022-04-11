using GameFramework;

namespace Try2
{
    public class AssetsPathUtils
    {
        public static string GetTileItemPrefab()
        {
            return "Assets/GameMain/Data/Entity/Sprite.prefab";
        }

        public static string GetTilePrefab()
        {
            return "Assets/GameMain/Data/Entity/Tile.prefab";
        }

        public static string GetTile(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Data/Tile/{0}.{1}", assetName, "png");
        }

        public static string GetEntity(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Data/Entity/{0}.{1}", assetName, "png");
        }

        public static string GetItem(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Data/Item/{0}.{1}", assetName, "png");
        }

        public static string GetItemAnim(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Data/Item/Anim/{0}.{1}", assetName, "anim");
        }

        public static string GetDataTable(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/DataTables/{0}.{1}", assetName, "txt");
        }

        public static string GetMap(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Data/Map/{0}.{1}", assetName, "json");
        }

        public static string GetPackage(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Data/Package/{0}.{1}", assetName, "bytes");
        }
    }
}
