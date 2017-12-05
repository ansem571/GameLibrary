using GameLibrary.DocumentIO;
using GameLibrary.Interfaces;
using GameLibrary.Models;
using GameLibrary.Models.Managers;
using GameLibrary.Models.Maps;
using GameLibrary.Models.Player;
using System;
using System.IO;

namespace CharacterConsole
{
    public class Program
    {
        private static void Main(string[] args)
        {
            string wanted_path = GetParent(GetParent(Directory.GetCurrentDirectory()));
            Console.WriteLine(wanted_path);

            var dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var xmlDocPath = dir + $"\\config.xml";
            var mapPath = dir + $"\\map.png";

            IReader reader = new XmlReader();
            reader.ReadDocument(xmlDocPath);

            IMap map = new GlobalMap(reader.Width, reader.Height, mapPath, reader.Tiles);

            IPlayer player = SetupPlayer(reader.Width, reader.Height);

            map.Grid[player.CurrentLocation].Visited = true;

            IBattleManager battleManager = new BattleManager();

            IGameManager game = new GameManager(map, player, battleManager);

            game.Play();
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
            IStats stats = new PlayerStats();
            ICharacterMovement movement = new PlayerMovement(w, h);
            ICombatActions combat = new CombatActions();

            IPlayer player = new Player(name, spawn, respawn, stats, movement, combat);

            return player;
        }
    }
}
