using GameLibrary.DocumentIO;
using GameLibrary.Interfaces;
using GameLibrary.Models;
using GameLibrary.Models.Managers;
using GameLibrary.Models.Maps;
using GameLibrary.Models.Player;
using GameLibrary.Models.Tiles.Special;
using GameLibrary.Models.VictoryConditions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CharacterConsole
{
    //Here is where I can be explicit on what objects types are.
    public class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            if (!IsWindows())
                throw new Exception("Not running on a windows os");

            //Works for getting files from desktop
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!dir.EndsWith(@"CharacterLibrary\Files"))
                dir = GetPathToGlobalFiles(dir);

            var xmlDocPath = dir + @"\config.xml";
            
            var mapPath = dir + @"\map.png";
            var appPath = dir + @"\ImageDisplayApp.exe";

            IBattleManager battleManager = new BattleManager();

            IReader reader = new XmlReader(battleManager);

            reader.ReadDocument(xmlDocPath);

            IPlayer player = SetupPlayer(reader.Width, reader.Height);

            IMap map = new GlobalMap(reader.Width, reader.Height, mapPath, appPath, reader.Tiles);

            var dungeons = GetDungeonTiles(map.GetMapGrid());

            IVictoryCondition victoryCondition = new DungeonVictoryCondition(map, player, dungeons);

            map.GetTileByLocation(player.GetCurrentLocation()).Visited = true;

            IGameManager game = new GameManager(map, victoryCondition, player);

            game.Play();
        }

        private static List<DungeonTile> GetDungeonTiles(Dictionary<IPoint, ITile> map)
        {
            List<DungeonTile> dungeons = new List<DungeonTile>();
            foreach(var tile in map)
            {
                if (tile.Value is DungeonTile)
                    dungeons.Add((DungeonTile)tile.Value);
            }
            return dungeons;
        }

        private static bool IsWindows()
        {
            var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");

            string productName = (string)reg.GetValue("ProductName");

            return productName.StartsWith("Windows");
        }

        private static string GetPathToGlobalFiles(string dllDir)
        {
            var path = Directory.GetParent(dllDir).Parent.Parent.FullName + @"\Files";
            return path;
        }

        private static string GetParent(string path)
        {
            return Path.GetDirectoryName(path);
        }

        private static IPlayer SetupPlayer(int w, int h)
        {
            string name = "Ansem571";
            IPoint spawn = new Point2D(5, 0);
            IPoint respawn = new Point2D(5, 0);
            IPlayerStats stats = new PlayerStats();
            ICharacterMovement movement = new PlayerMovement(w, h, spawn, respawn);
            ICombatActions combat = new CombatActions();

            IPlayer player = new Player(name, spawn, respawn, stats, movement, combat);

            return player;
        }
    }
}
