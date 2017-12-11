using GameLibrary.Enums;
using GameLibrary.Helpers;
using GameLibrary.Interfaces;
using GameLibrary.Models;
using GameLibrary.Models.Tiles.Special;
using GameLibrary.Models.Tiles.Terrain;
using GameLibrary.Models.VictoryConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace GameLibrary.DocumentIO
{
    /// <summary>
    /// First attempt of reading document
    /// </summary>
    public class XmlReader : IReader
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public List<List<ITile>> Tiles { get; private set; } = new List<List<ITile>>();

        private IBattleManager _battleManager;

        public XmlReader(IBattleManager battleManager)
        {
            _battleManager = battleManager ?? throw new ArgumentNullException(nameof(battleManager));
        }

        public void ReadDocument(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            var child = xmlDoc.FirstChild.FirstChild;
            var dimensions = xmlDoc.GetElementsByTagName("Grid_Dimensions").Item(0);

            Width = Convert.ToInt32(dimensions.FirstChild.InnerXml);
            Height = Convert.ToInt32(dimensions.LastChild.InnerXml);

            var regions = xmlDoc.GetElementsByTagName("Regions").Item(0);

            Tiles.Add(GetTiles(TileTypeEnum.Desert, regions["Desert"].ChildNodes));
            Tiles.Add(GetTiles(TileTypeEnum.Forest, regions["Forest"].ChildNodes));
            Tiles.Add(GetTiles(TileTypeEnum.Mountain, regions["Mountain"].ChildNodes));
            Tiles.Add(GetTiles(TileTypeEnum.Plains, regions["Plains"].ChildNodes));
            Tiles.Add(GetTiles(TileTypeEnum.Seaside, regions["Seaside"].ChildNodes));

            //Special tiles to replace terrain
            Tiles.Add(GetTiles(TileTypeEnum.Enemy, xmlDoc.GetElementsByTagName("Enemy_Tiles").Item(0).ChildNodes));
            Tiles.Add(GetTiles(TileTypeEnum.Town, xmlDoc.GetElementsByTagName("Town_Tiles").Item(0).ChildNodes));
            Tiles.Add(GetTiles(TileTypeEnum.Dungeon, xmlDoc.GetElementsByTagName("Dungeon_Tiles").Item(0).ChildNodes));
            Tiles.Add(GetTiles(TileTypeEnum.POI, xmlDoc.GetElementsByTagName("POI_Tiles").Item(0).ChildNodes));

            if (Tiles.Any(x => x == null))
                throw new NullReferenceException(nameof(Tiles) + " has a null reference.");

        }

        private List<ITile> GetTiles(TileTypeEnum tileType, XmlNodeList tiles)
        {
            GetTileType(tileType, out List<ITile> list, out Type typeOfTile);
            try
            {
                //Follows schema of Location, RegionId, Visited, Cleared (Dungeon only)
                foreach (XmlNode tile in tiles)
                {
                    var locationNode = tile.FirstChild;
                    if (locationNode == null)
                    {
                        Console.WriteLine($"Invalid location node: {tile.ParentNode.Name}");
                        continue;
                    }
                    var x = Convert.ToInt32(locationNode.FirstChild.InnerXml);
                    var y = Convert.ToInt32(locationNode.LastChild.InnerXml);
                    IPoint location = new Point2D(x, y);

                    var regionNode = locationNode.NextSibling;
                    if (regionNode == null)
                    {
                        Console.WriteLine($"Invalid regionID node: {tile}");
                        continue;
                    }
                    var region = regionNode.InnerXml;

                    var visitedNode = regionNode.NextSibling;
                    var visited = visitedNode != null ? Convert.ToBoolean(visitedNode.InnerXml) : false;

                    var tileParams = new List<object>
                    {
                        location,
                        region,
                        visited
                    };

                    if (typeOfTile == typeof(DungeonTile))
                    {
                        var clearedNode = visitedNode.NextSibling;
                        if (clearedNode != null)
                            tileParams.Add(Convert.ToBoolean(clearedNode.InnerXml));
                        else
                            tileParams.Add(false);
                        //TODO: Add as field in xml doc
                        var numOfEnemies = 3;
                        tileParams.Add(numOfEnemies);
                    }
                    if (typeOfTile == typeof(EnemyTile) || typeOfTile == typeof(DungeonTile))
                    {
                        tileParams.Add(_battleManager);
                    }

                    var newTile = (ITile)Activator.CreateInstance(typeOfTile, tileParams.ToArray());
                    list.Add(newTile);
                }
                return list;
            }
            catch (Exception e)
            {
                StaticHelperClass.PrintException(e, 3);
            }
            return null;
        }

        private static void GetTileType(TileTypeEnum tileType, out List<ITile> list, out Type typeOfTile)
        {
            switch (tileType)
            {
                case TileTypeEnum.Enemy:
                    {
                        list = new List<ITile>();
                        typeOfTile = typeof(EnemyTile);
                    }
                    break;
                case TileTypeEnum.Town:
                    {
                        list = new List<ITile>();
                        typeOfTile = typeof(TownTile);
                    }
                    break;
                case TileTypeEnum.Dungeon:
                    {
                        list = new List<ITile>();
                        typeOfTile = typeof(DungeonTile);
                    }
                    break;
                case TileTypeEnum.POI:
                    {
                        list = new List<ITile>();
                        typeOfTile = typeof(POITile);
                    }
                    break;
                case TileTypeEnum.Desert:
                    {
                        list = new List<ITile>();
                        typeOfTile = typeof(DesertTile);
                    }
                    break;
                case TileTypeEnum.Forest:
                    {
                        list = new List<ITile>();
                        typeOfTile = typeof(ForestTile);
                    }
                    break;
                case TileTypeEnum.Mountain:
                    {
                        list = new List<ITile>();
                        typeOfTile = typeof(MountainTile);
                    }
                    break;
                case TileTypeEnum.Plains:
                    {
                        list = new List<ITile>();
                        typeOfTile = typeof(PlainsTile);
                    }
                    break;
                case TileTypeEnum.Seaside:
                    {
                        list = new List<ITile>();
                        typeOfTile = typeof(SeasideTile);
                    }
                    break;
                case TileTypeEnum.None:
                default:
                    {
                        list = new List<ITile>();
                        typeOfTile = typeof(ITile);
                    }
                    break;
            }
        }


    }
}
