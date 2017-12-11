using GameLibrary.Helpers;
using GameLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;

namespace GameLibrary.Models.Managers
{
    public class GameManager : IGameManager
    {
        private IMap Map;
        private IVictoryCondition VictoryCondition;
        private IPlayer Player;
        private string ConfigPath;

        private int NumOfDungeonEnemies;

        public GameManager(IMap map, IVictoryCondition victoryCondition, IPlayer player, int numOfDungeonEnemies = 3, string configPath = null)
        {
            Map = map ?? throw new ArgumentNullException(nameof(map));
            VictoryCondition = victoryCondition ?? throw new ArgumentNullException(nameof(victoryCondition));
            Player = player ?? throw new ArgumentNullException(nameof(player));
            NumOfDungeonEnemies = numOfDungeonEnemies;

            ConfigPath = configPath;
        }

        public void Play()
        {
            var loc = Player.GetCurrentLocation();
            Map.OpenMap(loc);
            while (!VictoryCondition.VictoryConditionAchieved())
            {
                loc = Player.GetCurrentLocation();
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
                    VictoryCondition.VictoryConditionAchieved();
                }
                catch (Exception e)
                {
                    StaticHelperClass.PrintException(e, 1);
                }
            }
            VictoryCondition.DisplayVictoryMessage();
            Quit();
        }
        public void Save()
        {
            throw new NotImplementedException("Not finished yet. Will serialize data");
        }
        public void Quit()
        {
            Map.CloseMap(Player.GetCurrentLocation());
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
            var newLoc = Player.GetCurrentLocation();
            var tile = Map.GetTileByLocation(newLoc);

            tile.SetupParamsForTile(Player);

            tile.EnteredTile(Player);
            newLoc = Player.GetCurrentLocation();
            Map.RedrawMap(newLoc);
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
