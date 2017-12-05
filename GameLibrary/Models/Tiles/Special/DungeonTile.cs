using GameLibrary.Interfaces;
using GameLibrary.Models.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GameLibrary.Models.Tiles.Special
{
    public class DungeonTile : ITile
    {
        public string RegionId { get; } = "Dungeon";
        public IPoint Location { get; }
        public bool Visited { get; set; } = false;
        public bool Cleared { get; set; } = false;

        private bool Victory;
        public DungeonTile(IPoint loc, string regionId, bool visited = false, bool cleared = false)
        {
            Location = loc;
            RegionId = regionId;
            Visited = visited;
            Cleared = cleared;
        }

        public void EnteredTile(IPlayer player, params object[] args)
        {
            List<IEnemy> enemies = new List<IEnemy>();
            IBattleManager battleManager = null;
            foreach (var arg in args)
            {
                //Can handle if I put enemies as a list or not
                if (arg is IEnemy)
                    enemies.Add((IEnemy)arg);
                else if (arg is List<IEnemy> && enemies.Count == 0)
                    enemies = (List<IEnemy>)arg;
                else if (arg is IBattleManager && battleManager == null)
                    battleManager = (IBattleManager)arg;
            }

            EnteredTile(player, enemies, battleManager);

        }
        private void EnteredTile(IPlayer player, List<IEnemy> enemies, IBattleManager battleManager)
        {
            if (!Visited)
                Visited = true;

            if (Cleared)
            {
                Console.WriteLine($"You have already cleared this {GetType().Name}");
                Console.WriteLine("Nothing left to do here, so you make camp for the day.");
                return;
            }
            Console.WriteLine($"You entered a {GetType().Name}");
            Action victory = () => Victory = true;
            Action defeat = () => Victory = false;

            battleManager.InitActions(victory, defeat);


            foreach (var enemy in enemies)
            {
                battleManager.Battle(player, enemy);
                Console.Clear();
                if (!Victory)
                    return;
            }

            player.ResetHealthMana();
            Cleared = true;
            Console.WriteLine("Congrats. You cleared the dungeon.\r\nPress enter to continue...");
            Console.ReadLine();
            Console.Clear();
        }

        public object[] GetAppropriateParams(params object[] args)
        {
            List<object> list = new List<object>();
            IPlayer player = null;
            IBattleManager battleManager = null;
            string assemblyName = null;
            int? numOfEnemies = null;

            foreach (var arg in args)
            {
                if (arg is IPlayer && player == null)
                    player = (IPlayer)arg;
                if (arg is string && assemblyName == null)
                    assemblyName = (string)arg;
                if (arg is IBattleManager && battleManager == null)
                    battleManager = (IBattleManager)arg;
                if (arg is int && numOfEnemies == null)
                    numOfEnemies = (int)arg;
            }

            List<IEnemy> enemies = new List<IEnemy>();
            //Has the last enemy be the boss
            for (var i = 0; i < numOfEnemies + 1; i++)
            {
                Type type = GetEnemyType(assemblyName);

                IEnemy enemy = (IEnemy)Activator.CreateInstance(type, new object[] { 1 + (player.PlayerStats.Level / 3), i == numOfEnemies });

                enemies.Add(enemy);
            }
            list.Add(enemies);
            list.Add(battleManager ?? new BattleManager());

            return list.ToArray();
        }

        private static Type GetEnemyType(string assemblyName)
        {
            var random = new Random(new Random().Next(100) + 1);

            var enemies = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.Namespace.Contains(assemblyName) && !x.Name.Contains("Stats")).ToList();

            var enemyType = enemies[random.Next(enemies.Count)];
            return enemyType;
        }
    }
}
