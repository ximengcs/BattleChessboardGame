using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Try2
{
    public class Game
    {
        public const string Name = nameof(Game);

        private TileMap m_Map;
        private Selector m_Selector;
        private GameAssets m_GameAssets;

        public TileMap Map
        {
            get
            {
                return m_Map;
            }
        }

        public GameAssets Assets
        {
            get
            {
                return m_GameAssets;
            }
        }

        public Selector Selector
        {
            get
            {
                return m_Selector;
            }

            set
            {
                m_Selector = value;
            }
        }

        public void Initialize(GameAssets assets, int width, int height)
        {
            m_GameAssets = assets;
            m_Map = new TileMap(width, height);
        }

        public void Start()
        {
            Log.Debug("Game Start...", Log.GC, "Game");
        }
    }

    public class TileMap
    {
        private List<TileBase> m_Tiles;
        private Vector2Int m_Size;
        private int m_Count;

        public TileMap(int width, int height)
        {
            m_Count = 0;
            m_Size = new Vector2Int(width, height);
            m_Tiles = new List<TileBase>(width * height);
            for (int i = 0; i < m_Tiles.Capacity; i++)
                m_Tiles.Add(null);
        }

        public Vector2Int Size
        {
            get
            {
                return m_Size;
            }
        }

        public bool IsFull
        {
            get { return m_Count == m_Tiles.Capacity; }
        }

        public List<TileBase>.Enumerator GetEnumerator()
        {
            return m_Tiles.GetEnumerator();
        }

        public TileBase Get(Vector2Int index)
        {
            if (index.x >= 0 && index.x < m_Size.x && index.y >= 0 && index.y < m_Size.y)
            {
                return m_Tiles[index.y * m_Size.x + index.x];
            }
            else
            {
                return null;
            }
        }

        public TileBase Get(int x, int y)
        {
            return Get(new Vector2Int(x, y));
        }

        public void Set(TileBase tile)
        {
            Vector2Int index = tile.Data.Index;
            m_Tiles[index.y * m_Size.x + index.x] = tile;
            m_Count++;
        }

        public TileBase Left(TileBase tile)
        {
            Vector2Int index = tile.Data.Index;
            index.x -= 1;
            return Get(index);
        }

        public TileBase Right(TileBase tile)
        {
            Vector2Int index = tile.Data.Index;
            index.x += 1;
            return Get(index);
        }

        public TileBase Top(TileBase tile)
        {
            Vector2Int index = tile.Data.Index;
            index.y += 1;
            return Get(index);
        }

        public TileBase Bottom(TileBase tile)
        {
            Vector2Int index = tile.Data.Index;
            index.y -= 1;
            return Get(index);
        }
    }
}
