using GameFramework.DataNode;
using MeaponUnity.Core.Entry;
using System.Collections.Generic;
using UnityEngine;

namespace Try2
{
    public partial class TileUtils
    {
        public static Sprite CreateSprite(Texture2D tex)
        {
            return Sprite.Create(tex, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
        }

        public static IDataNode FindItemPath(TileItemBase item)
        {
            TilePathFinder finder = new TilePathFinder(item.Data.Props);
            finder.SetHelper(PropType.MovePower, new PropMoveHelper());
            finder.AddFindingFilter(new CampPathFilter());
            finder.StartFind(item, item.In);
            return finder.GetResultNode();
        }

        public static List<TileBase> FindCurrentAttackPath(TileItemBase item)
        {
            int range;
            if (!item.Data.Props.TryGetValue(PropType.MaxAttackRange, out range))
            {
                if (!item.Data.Props.TryGetValue(PropType.MinAttackRange, out range))
                {
                    return null;
                }
            }

            Dictionary<PropType, int> props = new Dictionary<PropType, int>();
            props[PropType.MovePower] = range;
            TilePathFinder finder = new TilePathFinder(props);
            finder.SetHelper(PropType.MovePower, new PropMoveOneGridHelper());
            finder.StartFind(item, item.In);
            finder.AddResultFilter(PropType.MovePower, new MinMaxAttackRangeHelper());

            return finder.GetResultList();
        }

        private class TilePathFinder
        {
            private List<IPathFilter> m_FindingFilters;  //路径过滤器
            private Dictionary<PropType, IPathListFilter> m_ResultFilters;  //结果集过滤器
            private Dictionary<PropType, IPropConsumeHelper> m_Helpers;  //属性计算器
            private Dictionary<PropType, int> m_Suplus;
            private Dictionary<TileBase, TileFindInfo> m_HasAdd;
            private TileItemBase m_Item;
            private IDataNode m_ResultPathNode;
            private List<TileBase> m_ResultList;

            public TilePathFinder(Dictionary<PropType, int> props)
            {
                m_FindingFilters = new List<IPathFilter>(5);
                m_ResultFilters = new Dictionary<PropType, IPathListFilter>(5);
                m_Suplus = new Dictionary<PropType, int>(props);
                m_HasAdd = new Dictionary<TileBase, TileFindInfo>();
                m_Helpers = new Dictionary<PropType, IPropConsumeHelper>(props.Count);
            }

            public void SetHelper(PropType type, IPropConsumeHelper helper)
            {
                m_Helpers[type] = helper;
            }

            public void AddFindingFilter(IPathFilter filter)
            {
                m_FindingFilters.Add(filter);
            }

            public void AddResultFilter(PropType type, IPathListFilter filter)
            {
                m_ResultFilters.Add(type, filter);
            }

            public void StartFind(TileItemBase item, TileBase tile)
            {
                m_ResultPathNode = MeaponEntry.DataNode.GetOrAddNode("Finder");
                m_ResultPathNode.Clear();
                m_Item = item;
                InternalFind(m_ResultPathNode, tile, m_Suplus);
            }

            public IDataNode GetResultNode()
            {
                return m_ResultPathNode;
            }

            public List<TileBase> GetResultList()
            {
                m_ResultList = new List<TileBase>(5);
                InternalCreateList();
                return m_ResultList;
            }

            private void InternalCreateList()
            {
                foreach (TileFindInfo info in m_HasAdd.Values)
                {
                    foreach (KeyValuePair<PropType, int> suplus in info.Suplus)
                    {
                        IPathListFilter filter;
                        if (m_ResultFilters.TryGetValue(suplus.Key, out filter))
                        {
                            if (filter.Can(m_Item, info.Tile, suplus.Value))
                            {
                                m_ResultList.Add(info.Tile);
                            }
                        }
                    }
                }
            }

            private void InternalFind(IDataNode node, TileBase tile, Dictionary<PropType, int> consumes)
            {
                TileBase top = tile.Top;
                TileBase bottom = tile.Bottom;
                TileBase left = tile.Left;
                TileBase right = tile.Right;

                TileFindInfo findInfo = new TileFindInfo();
                findInfo.Tile = tile;
                findInfo.Suplus = consumes;
                findInfo.Node = node;
                m_HasAdd.Add(tile, findInfo);
                node.SetData<VarTileBase>(tile);

                InternalFindCheck(node, top, consumes);
                InternalFindCheck(node, bottom, consumes);
                InternalFindCheck(node, left, consumes);
                InternalFindCheck(node, right, consumes);
            }

            private void InternalFindCheck(IDataNode node, TileBase tile, Dictionary<PropType, int> src)
            {
                if (tile == null)
                    return;

                Dictionary<PropType, int> suplus = InternalReachSuplus(tile, src);

                if (InternalCanReach(tile, suplus))
                {
                    if (m_HasAdd.TryGetValue(tile, out TileFindInfo info))
                    {
                        if (InternalGravity(suplus, info.Suplus))
                        {
                            InternalRemoveInfo(info);
                            InternalFind(node.GetOrAddChild(tile.Entity.Id.ToString()), tile, suplus);
                        }
                    }
                    else
                    {
                        InternalFind(node.GetOrAddChild(tile.Entity.Id.ToString()), tile, suplus);
                    }
                }
            }

            private void InternalRemoveInfo(TileFindInfo info)
            {
                InternalRemoveInfoChild(info.Node);
                info.Node.Parent.RemoveChild(info.Node.Name);
            }

            private void InternalRemoveInfoChild(IDataNode node)
            {
                m_HasAdd.Remove(node.GetData<VarTileBase>());
                for (int i = 0; i < node.ChildCount; i++)
                {
                    InternalRemoveInfoChild(node.GetChild(i));
                }
            }

            private bool InternalCanReach(TileBase tile, Dictionary<PropType, int> suplus)
            {
                foreach (var item in suplus)
                {
                    if (item.Value < 0)
                        return false;
                }

                foreach (var filter in m_FindingFilters)
                {
                    if (!filter.Can(m_Item, tile))
                        return false;
                }

                return true;
            }

            private Dictionary<PropType, int> InternalReachSuplus(TileBase tile, Dictionary<PropType, int> src)
            {
                Dictionary<PropType, int> suplus = new Dictionary<PropType, int>(tile.Data.Consume.Count);
                foreach (var prop in tile.Data.Consume.Keys)
                {
                    if (m_Helpers.TryGetValue(prop, out IPropConsumeHelper helper))
                    {
                        if (src.ContainsKey(prop))
                            suplus[prop] = helper.Comsume(tile, m_Item, src[prop]);
                        else
                            suplus[prop] = 0;
                    }
                    else
                    {
                        suplus[prop] = src[prop];
                    }
                }

                return suplus;
            }

            private bool InternalGravity(Dictionary<PropType, int> src, Dictionary<PropType, int> dest)
            {
                foreach (var item in src)
                {
                    if (item.Value <= dest[item.Key])
                        return false;
                }

                return true;
            }
        }

        private class TileFindInfo
        {
            public TileBase Tile;
            public IDataNode Node;
            public Dictionary<PropType, int> Suplus;
        }
    }
}
