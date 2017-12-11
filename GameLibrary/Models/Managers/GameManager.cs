using GameLibrary.Helpers;
using GameLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GameLibrary.Models.Managers
{
    public class GameManager : IGameManager
    {
        public IMap Map { get; }
        public IVictoryCondition VictoryCondition { get; }
        public IPlayer Player { get; }
        public IBattleManager BattleManager { get; }
        public string ConfigPath { get; }

        private string EnemyAssemblyNamespace { get; }
        private int NumOfDungeonEnemies { get; }

        public GameManager(IMap map, IVictoryCondition victoryCondition, IPlayer player, IBattleManager battleManager, int numOfDungeonEnemies = 3, string assemblyName = "GameLibrary.Models.Enemies.", string configPath = null)
        {
            Map = map ?? throw new ArgumentNullException(nameof(map));
            VictoryCondition = victoryCondition ?? throw new ArgumentNullException(nameof(victoryCondition));
            Player = player ?? throw new ArgumentNullException(nameof(player));
            BattleManager = battleManager ?? throw new ArgumentNullException(nameof(player));
            NumOfDungeonEnemies = numOfDungeonEnemies;

            EnemyAssemblyNamespace = assemblyName;
            ConfigPath = configPath;
        }

        public void Play()
        {
            var loc = Player.CurrentLocation;
            Map.OpenMap(loc);
            while (!VictoryCondition.VictoryAchieved)
            {
                loc = Player.CurrentLocation;
                Player.PrintStats(true);
                try
                {
                    var option = StaticHelperClass.ReadWriteOptions(new List<string> { "Move", "Rest/Fight again", "Save", "Quit" });
                    Console.Clear();
                    switch (option)
                    {
                        case 1:
                            {
                                Move();
                            }
                            break;
                        case 2:
                            {
                                Rest();
                            }
                            break;
                        case 3:
                            {
                                Save();
                            }
                            break;
                        case 4:
                            {
                                Quit();
                            }
                            break;
                        default:
                            {
                                Console.WriteLine("Cannot perform action.\r\n");
                            }
                            break;
                    }
                    VictoryCondition.VictoryConditionAchieved(Player, Map);
                }
                catch (Exception e)
                {
                    StaticHelperClass.PrintException(e, 1);
                }
            }
            VictoryCondition.DisplayVictoryMessage(Player, Map);
            Quit();
        }
        public void Save()
        {
            throw new NotImplementedException("Not finished yet. Will serialize data");
        }
        public void Quit()
        {
            Map.CloseMap(Player.CurrentLocation);
            Console.WriteLine("Good bye");

            Thread.Sleep(1000);
            Console.Write("3...");
            Thread.Sleep(1000);
            Console.Write("2...");
            Thread.Sleep(1000);
            Console.Write("1...");
            Thread.Sleep(1000);

            Environment.Exit(0);
        }

        private void Move()
        {
            Player.PerformMovement();

            UpdateMap();
        }

        private void UpdateMap()
        {
            var newLoc = Player.CurrentLocation;
            var tile = Map.Grid[newLoc];

            object[] list = GetTileByTile(tile);

            tile.EnteredTile(Player, list);
            newLoc = Player.CurrentLocation;
            Map.RedrawMap(newLoc);
        }

        private object[] GetTileByTile(ITile tile)
        {
            var list = tile.GetAppropriateParams(Player, BattleManager, EnemyAssemblyNamespace, NumOfDungeonEnemies);

            return list.ToArray();
        }

        private void Rest()
        {
            TimeSpan threeSeconds = TimeSpan.FromSeconds(3);
            Console.WriteLine("You decide to look at your monster death toll(s) while resting.");
            Player.OpenMonsterCollection();
            Thread.Sleep(threeSeconds);

            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();

            UpdateMap();
        }
    }
}
