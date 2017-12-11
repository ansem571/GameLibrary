using GameLibrary.Helpers;
using GameLibrary.Interfaces;
using System;
using System.Linq;
using System.Reflection;

namespace GameLibrary.Models.Tiles.Special
{
    public class EnemyTile : ITile
    {
        public string RegionId { get; } = "Enemy";
        public IPoint Location { get; }
        public bool Visited { get; set; } = false;

        private IBattleManager _battleManager;
        private readonly string _enemyAssemblyName = "GameLibrary.Models.Enemies.";
        private IEnemy _enemy;

        public EnemyTile(IPoint loc, string regionId, bool visited, IBattleManager battleManager)
        {
            Location = loc;
            RegionId = regionId;
            Visited = visited;
            _battleManager = battleManager;
        }

        public void EnteredTile(IPlayer player)
        {
            if (!Visited)
                Visited = true;
            Console.WriteLine($"You entered a {GetType().Name}");
            var Victory = false;

            Action victory = () => Victory = true;
            Action defeat = () => Victory = false;

            _battleManager.Battle(player, _enemy, victory, defeat);
            if (Victory)
                player.ResetHealthMana();
            StaticHelperClass.PrintException(null, 3);
        }

        public void SetupParamsForTile(IPlayer player)
        {
            Type type = GetEnemyType(_enemyAssemblyName);

            _enemy = (IEnemy)Activator.CreateInstance(type, new object[] { 1 + (player.GetCurrentStats().Level / 3), false });
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
