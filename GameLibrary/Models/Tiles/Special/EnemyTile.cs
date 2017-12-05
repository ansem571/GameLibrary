using GameLibrary.Helpers;
using GameLibrary.Interfaces;
using GameLibrary.Models.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace GameLibrary.Models.Tiles.Special
{
    public class EnemyTile : ITile
    {
        public string RegionId { get; } = "Enemy";
        public IPoint Location { get; }
        public bool Visited { get; set; } = false;

        private bool Victory;
        public EnemyTile(IPoint loc, string regionId, bool visited = false)
        {
            Location = loc;
            RegionId = regionId;
            Visited = visited;
        }

        public void EnteredTile(IPlayer player, params object[] args)
        {
            IEnemy enemy = null;
            IBattleManager battleManager = null;
            foreach (var arg in args)
            {
                if (arg is IEnemy)
                    enemy = (IEnemy)arg;
                else if (arg is IBattleManager)
                    battleManager = (IBattleManager)arg;
            }

            EnteredTile(player, enemy ?? throw new Exception("Enemy not initialized"), battleManager ?? throw new Exception("Battle Manager not initialized"));

        }
        public void EnteredTile(IPlayer player, IEnemy enemy, IBattleManager battleManager)
        {
            if (!Visited)
                Visited = true;
            Console.WriteLine($"You entered a {GetType().Name}");
            Action victory = () => Victory = true;
            Action defeat = () => Victory = false;

            battleManager.InitActions(victory, defeat);

            battleManager.Battle(player, enemy);

            player.ResetHealthMana();
            StaticHelperClass.PrintException(null, 3);
        }

        public object[] GetAppropriateParams(params object[] args)
        {
            List<object> list = new List<object>();
            IPlayer player = null;
            IBattleManager battleManager = null;
            string assemblyName = null;

            foreach (var arg in args)
            {
                if (arg is IPlayer && player == null)
                    player = (IPlayer)arg;
                if (arg is string && assemblyName == null)
                    assemblyName = (string)arg;
                if (arg is IBattleManager && battleManager == null)
                    battleManager = (IBattleManager)arg;
            }

            Type type = GetEnemyType(assemblyName);

            IEnemy enemy = (IEnemy)Activator.CreateInstance(type, new object[] { 1 + (player.PlayerStats.Level / 3), false });

            list.Add(enemy);
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
